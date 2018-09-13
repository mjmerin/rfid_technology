//***************************************************************************************//
//  Team ECE 08.5 - RFID Technology for Ultrasound                                       // 
//  June 5, 2008                                                                         //
//                                                                                       //
//  Team Members: Riley Higa, Andrew Kurniadi, Min Kyeong Lee, Mark Merin, Robert Nguon  //
//  Liason Engineer: Matthew Rieger                                                      //
//  Faculty Advisor: Dr. Robert Heeren                                                   //
//  Seattle University / Philips Healthcare                                              //
//                                                                                       //
//  Built with IAR Embedded Workbench IDE                                                //
//                                                                                       //
//  main.c                                                                               //
//***************************************************************************************//

#include "msp430x16x.h"
#include <stdbool.h>
#include "uart_functions.h"
#include "rfid_functions.h"
#include "process_data.h"

/* Function Headers */
/* Description: Set Up Microcontroller and TRF7960 */
void InitializeHardware ( void );

/* Description: Handles TRF7960 Interrupt Service Routine */
void TRFInterruptHandler( unsigned char *reg );

/* Main Program */
void main( void )
{	
    WDTCTL = WDTPW + WDTHOLD;  // Stop Watchdog Timer
    
    InitializeHardware();      // Set Up Microcontroller and TRF7960
    InitializeUART();          // Set Up UART Functionality
    InitializeFlags();         // Initialize Flags and Counters
    wait_ms(10);
    __enable_interrupt();      // Enable interrupts
    
    while(1)
    {
        if(DATA_IN_BUFFER) 
        { 
            ProcessData();
        }
        LPM0;                    // Put microcontroller in Low Power Mode while it's waiting
    }
}


/***************************************************************
 * Description: Set Up Microcontroller and TRF7960
***************************************************************/
void InitializeHardware( void )
{
    /* Set up Microcontroller pins to work with TRF7960 */
    P1SEL = 0x00;   // Select all P1 ports as I/O
    P1DIR = 0xFF;   // Set all P1 ports as Output
    P1DIR &= ~BIT5; // Set only P1.5 as input for IRQ from TRF7960
    P1IES &= ~BIT5; // Set interrupt on the rising edge
    onIRQ;          // Set interrupt pin on.
    P2SEL = 0x00;   // Set Port 2, Bit 0 - Bit 7 as I/O ports with TRF7960
    SetPinIO_OUT;
    
    /* Setting up pins to light LEDs */
    P5SEL = 0x00;  // Set all P5 ports as I/O
    P5DIR = 0xFF;  // Set all P5 ports as Output for LEDs
    P5OUT = 0x00;  // Make sure all outputs are off
    
    /* TRF7960 Initialization */
    P1OUT = 0xC0;                        // Enable EN and EN2 for full power mode - TRF7960 Datasheet (page 14)
    Write_Address(SYSCLKControl, 0x21);  // Change clock rate to RF/2 (6.78 MHz) and 100% OOK Mod
    Write_Address(ChipStatControl,0x21); // Specify the TRF7960 to run in full active mode in 5V operation (default)
    Write_Address(ISOControl,0x02);      // Select ISO-15693 high bit rate mode
    RXGainAdjust;                        // Perform Receiver Gain Adjust to reduce noise in system
    ResetFIFO;
    
    unsigned int ii1;
    
    /* Set up Microcontroller Clock */
    BCSCTL1 |= XTS + XT2OFF;  // Set the LF clock to HF
    BCSCTL1 &= ~(RSEL0 + RSEL1 + RSEL2);
    
    do
    {
          IFG1 &= ~OFIFG;                   // Clear OSCFault flag
          for (ii1 = 0xFF; ii1 > 0; ii1--); // Time delay for flag to set
    }
    while ((IFG1 & OFIFG) == OFIFG);    // OSCFault flag still set?
    
    BCSCTL2 |= SELM1 + SELM0 + SELS;
}


/*==============================================================
                  INTERRUPT SERVICE ROUTINES                 
===============================================================*/

/***************************************************************
 * Description: UART0 RX Interrupt Service Routine
 ***************************************************************/
#pragma vector=UART0RX_VECTOR
__interrupt void UART0RX (void)
{
      DATA_IN_BUFFER = true;
      __low_power_mode_off_on_exit();
}


/***************************************************************
 * Description: Timer A0 Interrupt Service Routine
 ***************************************************************/
#pragma vector=TIMERA0_VECTOR
__interrupt void TimerAhandler(void)
{	
	unsigned char reg;
	stopCounter;
      
        reg = IRQStatus;
	clearIRQ;		
        Read_Address(&reg);	
	reg = reg & 0xF7;	// Set parity flag to 0 in IRQ Status
	
	if(reg == 0x00)
        {
	        irq_reg = 0x00;
        }
	else
        {
		irq_reg = 0x01;
        }
	__low_power_mode_off_on_exit();
}


/***************************************************************
 * Description: TRF7960 Interrupt Service Routine
 ***************************************************************/
#pragma vector=PORT1_VECTOR
__interrupt void TRF_IRQ (void)
{
      unsigned char reg[2];
      stopCounter;
      
      do
      {
            clearIRQ;  
            reg[0] = IRQStatus;
            Read_Address(reg);
            if(*reg == 0xA0)
            {
                  goto FINISH;
            }
            TRFInterruptHandler(&reg[0]);     
      } while((P1IN & IRQPin) == BIT5);
      
      FINISH:
      __low_power_mode_off_on_exit(); 
}


/***************************************************************
 * Description: Handles TRF7960 Interrupt Service Routine
 ***************************************************************/
void TRFInterruptHandler( unsigned char *reg )
{
    unsigned char len;
        
      SendByte(*reg);
      SendT(": ");
     
      if(*reg == 0xA0) // TX active and less than 4 bytes in FIFO
      {
          irq_reg = 0x00; // Say that TX is complete
      }
      else if(*reg == BIT7) // TX complete
      {
          irq_reg = 0x00; // Say that TX is complete
          SendT("TX Done\r\n");
      }
      else if((*reg & BIT1) == BIT1) // Collision Error
      {
          irq_reg = 0x02; // Say that RX is complete
          BlockReceivers; // Block Receivers
          coll_pos = CollisionPos;
          Read_Address(&coll_pos);
          len = coll_pos - 0x20; // number of valid bits in FIFO
          
            SendT("Collision Error {");
            SendByte(coll_pos);
            SendT("}\r\n");
          
          if((len & 0x0F) != 0x00)
          {
            len = len + 0x10; // Add one byte if broken byte is received
          }
          len = len >> 4;
          
          if(len != 0x00)
          {
            Read_AddressCont(FIFO, &buf[RXTXstate],len); // write the received bytes to the correct place in the buffer;
            RXTXstate = RXTXstate + len;
          }

          ResetFIFO;
          *reg = IRQStatus;
          Read_Address(reg);
          clearIRQ;
      }
      else if(*reg == 0x40) // RX active and that EOF has been received ... need to check FIFO for unread contents
      {
          if(RXErrorFlag == 0x02)
          {
            irq_reg = 0x02; // Say that CRC or Byte Framing Error (collission) Occured
            return;
          }
          *reg = FIFOStatus;
          Read_Address(reg);
          *reg = (0x0F & *reg) + 0x01; // masking the fifo_status to determines number of bytes left in FIFO
          Read_AddressCont(FIFO, &buf[RXTXstate], *reg); 
  
          RXTXstate = RXTXstate + *reg;
          
          *reg = TXLength2;
          Read_Address(reg); // reading from TXLengthByte2 to see if there are any broken bytes
          if((*reg & BIT0) == BIT0) // found broken byte
          {
            *reg = (*reg >> 1) & 0x07; 
            *reg = 8 - *reg;
            buf[RXTXstate - 1] &= (0xFF << *reg);
          }
          ResetFIFO;
          irq_reg = 0xFF; // says that these are the last bytes
          
          SendT("EOF Received\r\n");
      }
      else if(*reg == 0x60) // RX active and 9 bytes already in FIFO
      {
          irq_reg = 0x01;
          Read_AddressCont(FIFO, &buf[RXTXstate],9); // Read 9 bytes from FIFO
          RXTXstate = RXTXstate + 9;
          
          SendT("FIFO Full\r\n");

          if(P1IN & IRQPin)
          {
            *reg = IRQStatus;
            Read_Address(reg);
            clearIRQ;
            if(*reg == 0x40) // RX active and that EOF has been received ... need to check FIFO for unread contents
            {
              *reg = FIFOStatus;
              Read_Address(reg);
              *reg = 0x0F & (*reg + 0x01);
              
              Read_AddressCont(FIFO, &buf[RXTXstate],*reg);
              RXTXstate = RXTXstate + *reg;
              
              *reg = TXLength2;
              Read_Address(reg); // reading from TXLengthByte2 to see if there are any broken bytes
              if((*reg & BIT0) == BIT0) // found broken byte
              {
                *reg = (*reg >> 1) & 0x07;
                *reg = 8 - *reg;
                buf[RXTXstate - 1] &= 0xFF << *reg;
              }
              
              ResetFIFO;
              irq_reg = 0xFF;
              
              SendT("     EOF Received\r\n");
            }
            else if(*reg == 0x50) // RX EOF error
            {
              irq_reg = 0x02; // end of RX
              SendT("     EOF Receive Error\r\n");
            }
          }
          else
          {
            *reg = IRQStatus;
            Read_Address(reg);
            *(reg + 1) = FIFOStatus;
            Read_Address(reg+1);
            if(*reg == 0x00) 
              irq_reg = 0xFF;
          }
      }
      else if((*reg & BIT4) == BIT4) // CRC Error
      {
          if((*reg & BIT5) == BIT5) // FIFO is underflow or overflow
          {
            irq_reg = 0x01; //RX Active
            RXErrorFlag = 0x02;
            SendT("CRCE + RX Active\r\n");
          }
          else
             irq_reg = 0x02; // End of RX
      }
      else if((*reg & BIT2) == BIT2) // Byte Framing Error
      {
          if((*reg & BIT5) == BIT5) // FIFO is underflow or overflow
          {
            irq_reg = 0x01; //RX Active
            RXErrorFlag = 0x02;
          }
          else
            irq_reg = 0x02; // End of RX
      }
      else if(*reg == BIT0) // No Response Interrupt
      {
          irq_reg = 0x00;
          SendT("No Response\r\n");
      }
      else // Interrupt Register Not Properly Set
      {
          if(DEBUG) SendByte(*reg);
        
          irq_reg = 0x02;
          BlockReceivers; // Block Receivers
          ResetFIFO;
          *reg = IRQStatus;
          Read_Address(reg);
          SendT("Unknown Interrupt Error\r\n");
          clearIRQ;
      }
}
