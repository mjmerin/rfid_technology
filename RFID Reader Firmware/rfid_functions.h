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
//  rfid_functions.h                                                                     //
//***************************************************************************************//

#ifndef RFID_FUNCTIONS_H
#define RFID_FUNCTIONS_H

#include "msp430x16x.h"
#include "uart_functions.h"

/* Global Variables */
bool found;
signed char RXTXstate;
unsigned char RXErrorFlag; 
unsigned char coll_pos;
unsigned char irq_reg;
unsigned char buf[20];

/* MSP430F169 Pin Definitions */
#define SetPinIO_OUT      P2DIR = 0xFF;
#define SetPinIO_IN       P2DIR = 0x00;
#define DataClockHigh     P1OUT |= BIT4;
#define DataClockLow      P1OUT &= ~BIT4;
#define IRQPin            BIT5
#define onIRQ             P1IE |= BIT5  // Turn IRQ pin on; Pin P1.5
#define offIRQ            P1IE &= ~BIT5 // Turn IRQ pin off; Pin P1.5
#define clearIRQ          P1IFG = 0x00  // Clear IRQ Request

/* TRF7960 Register Addresses Definitions - TRF7960 Datasheet (pages 22 to 30) */
#define ChipStatControl   0x00
#define ISOControl        0x01
#define SYSCLKControl     0x09
#define IRQStatus         0x0C
#define CollisionPos      0x0E
#define InterruptMask     0x0D
#define RSSILevels        0x0F
#define FIFOStatus        0x1C
#define TXLength          0x1D
#define TXLength2         0x1E
#define FIFO              0x1F

/* TRF7960 Direct Commands Definitions - TRF7960 Datasheet (page 31) */
#define ResetFIFO         Write_Command(0x0F);
#define TXNextTimeSlot    Write_Command(0x14);
#define BlockReceivers    Write_Command(0x16);
#define StartReceivers    Write_Command(0x17);
#define RXGainAdjust      Write_Command(0x1A);

/* Counter Definitions */
#define startCounter	TACTL |= (MC0 + MC1)	// start counter in up/down mode
#define stopCounter	TACTL &= ~(MC0 + MC1)	// stops the counter
#define countValue	TACCR0			// 16-bit counter register

/* Function Headers */
/* Description: Start Condition for communication with TRF7960 */
void StartCond( void );           

/* Description: Stop Condition for communication with TRF7960 */
void StopCond( void ); 

/* Description: Stop Condition for continuous communication with TRF7960 */
void StopCond_Cont( void );      

/* Description: Write Direct Command to TRF7960 */
void Write_Command( unsigned char command );            

/* Description: Write to one TRF7960 register */ 
void Write_Address( unsigned char address, unsigned char command_str ); 

/* Description: Write to starting register and continues to write data to each following registers */
void Write_AddressCont( unsigned char address, unsigned char *dBuffer, unsigned int length ); 

/* Description: Read from one TRF7960 register */
void Read_Address( unsigned char *address );

/* Description: Read from starting register and continues to read data from each following registers */
void Read_AddressCont( unsigned char address, unsigned char rBuffer[],  unsigned int length );

/* Description: Able to perform multiple operations on TRF7960 registers */
void Direct_Write( unsigned char buffer[], unsigned int length );

/* Description: Enables "No Tag Response Interrupt" on TRF7960 */
void EnableSlotCounter(void);

/* Description: Disables "No Tag Response Interrupt" on TRF7960 */
void DisableSlotCounter(void);

/* Description: Set up Timer A counter on MSP430F169*/
void CounterSet(void);

/* Description: Time Delay function */
void wait_ms(unsigned int n_ms);

/* Description: Detect tags and request tag UID (Unique ID) */
void CheckInventory();

/* Description: Request read from tag or write to tag */
unsigned char RequestCommand(unsigned char length, unsigned char brokenBits);


/* Function Implementations */
/***************************************************************
 * Description: Start Condition for communication with TRF7960
 ***************************************************************/
void StartCond( void )
{
   P2OUT = 0x00;   // A/D[7] is low
   DataClockHigh;
   P2OUT = 0xFF;   // A/D[7] is high [Start Condition]
   DataClockLow;
}


/***************************************************************
 * Description: Stop Condition for communication with TRF7960
 ***************************************************************/
void StopCond( void )
{
   P2OUT |= 0x80;  // A/D[7] is high
   DataClockHigh;
   P2OUT = 0x00;   // A/D[7] is low [Stop Condition]
   DataClockLow; 
}


/***************************************************************
 * Description: Stop Condition for continuous communication
 *              with TRF7960
 ***************************************************************/
void StopCond_Cont( void )
{
   P2OUT = 0x00;  // A/D[7] is low
   SetPinIO_OUT;
   P2OUT = 0x80;  // A/D[7] is high
   __no_operation();
   P2OUT = 0x00;  // A/D[7] is low
}


/***************************************************************
 * Description: Write Direct Command to TRF7960
 ***************************************************************/

void Write_Command( unsigned char command )
{
    StartCond();
    command |= 0x80;  // Add "write this command" instruction
    command &= 0x9F;//
    P2OUT = command;
    DataClockHigh;
    DataClockLow;
    StopCond();
}


/***************************************************************
 * Description: Write to one TRF7960 register
 ***************************************************************/
void Write_Address( unsigned char address, unsigned char options )
{
    StartCond();
    address &= 0x1F;
    P2OUT = address;  // Specify address to write to
    DataClockHigh;
    DataClockLow;
    P2OUT = options;  // Writing data to the specified address
    DataClockHigh;
    DataClockLow;
    StopCond();
}


/***************************************************************
 * Description: Write data to starting address and continues to 
 *              write data to each address after the previous
 *              address. Amount of addresses written to is 
 *              determined by the length
 ***************************************************************/
void Write_AddressCont( unsigned char address, unsigned char *dBuffer, unsigned int length )
{
    StartCond();
    address |= 0x20;
    address &= 0x3F;
    P2OUT = address;
    DataClockHigh;
    DataClockLow;
    while(length > 0)
    {
      P2OUT = *dBuffer;
      DataClockHigh;
      DataClockLow;
      dBuffer++;
      length--;
    }
    StopCond_Cont();
}


/***************************************************************
 * Description: Read from one TRF7960 Register
 ***************************************************************/
void Read_Address( unsigned char *buffer )
{ 
   StartCond();
   *buffer |= 0x40; // Add "read from address" instruction
   *buffer &= 0x5F;//
   P2OUT = *buffer; // Specify address to read from
   DataClockHigh;
   DataClockLow;
   
   SetPinIO_IN;   
   DataClockHigh;
   __no_operation();
   *buffer = P2IN;  // Store read data from specified address
   DataClockLow;
   P2OUT = 0x00;
   SetPinIO_OUT;   
   
   StopCond();
}


/***************************************************************
 * Description: Read data from starting address and continues to 
 *              read data from each address after the previous
 *              address. Amount of addresses read from is
 *              determined by the length
 ***************************************************************/
void Read_AddressCont( unsigned char address, unsigned char *rBuffer, unsigned int length )
{
   StartCond();
   address |= 0x60; // Specify that we want to "read from an address"
   address &= 0x7F;
   P2OUT = address; // Send address instruction across 8-bit parallel bus
   DataClockHigh;
   DataClockLow;
   
   SetPinIO_IN;
    
   while(length > 0)
   {
     DataClockHigh;
     __no_operation();
     *rBuffer = P2IN;
     DataClockLow;
     rBuffer++;
     length--;
   }
   SetPinIO_OUT;
   StopCond_Cont();
}

/***************************************************************
 * Description: Able to perform multiple operations on TRF7960 registers
 ***************************************************************/
void Direct_Write( unsigned char *buffer, unsigned int length )
{
    StartCond();
    while(length > 0)
    {
      P2OUT = *buffer;	/* send command */
      DataClockHigh;
      DataClockLow;
      buffer++;
      length--;
    }
    StopCond_Cont();
}


/***************************************************************
 * Description: Enables "No Tag Response Interrupt" on TRF7960
 ***************************************************************/
void EnableSlotCounter(void)
{
    unsigned char reg;
    reg = InterruptMask;
    Read_Address(&reg);
    reg |= BIT0;	/* set BIT0 in register 0x01 */
    Write_Address(InterruptMask, reg);
}


/***************************************************************
 * Description: Disables "No Tag Response Interrupt" on TRF7960
 ***************************************************************/
void DisableSlotCounter(void)
{
    unsigned char reg;
    reg = InterruptMask;
    Read_Address(&reg);
    reg &= 0xfe;	/* clear BIT0 in register 0x01 */
    Write_Address(InterruptMask, reg);
}


/***************************************************************
 * Description: Set Up Timer A on MSP430F169
 ***************************************************************/
void CounterSet(void)
{
    TACTL |= TACLR;
    TACTL &= ~TACLR;			//reset the timerA
    TACTL |= TASSEL0 + ID1 + ID0;	//ACLK, clk/8
    
    TAR = 0x0000;
    TACCTL0 |= CCIE;			//compare interrupt enable
}


/***************************************************************
 * Description: Time Delay Function
 ***************************************************************/
void wait_ms(unsigned int n_ms) {
    unsigned int ii1, ii0;
    for(ii0=n_ms; ii0>0; ii0--) {
        ii1 = 0x07FF;                    // Delay Variable
        do (ii1--);
        while (ii1 != 0);
    }
}


/***************************************************************
 * Description: Detect tags and request tag UID (Unique ID)
 ***************************************************************/
void CheckInventory()
{
	unsigned char	i = 1, NoSlots, m = 0;
        unsigned char   command;
	int	        size;
	unsigned int	k = 0;

        found = false; 
        
	NoSlots = 17;		/* 16 slots */
	EnableSlotCounter();

	size = 3;                               /* Amount of data written to FIFO is 3 */

	buf[0] = 0x8F;                          /* Reset FIFO */
	buf[1] = 0x91;				/* Command to Transmit with CRC */
	buf[2] = 0x3D;				/* Write continous from register 1D - TXLength1 and TXLength2 */
	buf[3] = (char) (size >> 8);            
	buf[4] = (char) (size << 4);
	buf[5] = 0x06;				/* ISO15693 Flag = High Bit Data Rate + Inventory */
	buf[6] = 0x01;				/* Inventory request command code */
	buf[7] = 0x00;			        /* masklenght */

        command = IRQStatus;
	Read_Address(&command);

	CounterSet();				/* TimerA set */
	countValue = 0x5000;			
	Direct_Write(&buf[0], 8);               /* Writing to FIFO */
        
	clearIRQ;				
	onIRQ;
        
        irq_reg = 0x01;
	startCounter;				/* start timer */
        

	LPM0;		                        /* wait for the end of TX interrupt */

	for(i = 1; i < NoSlots; i++)
	{	
          	/* the first UID will be stored from buf[1] upwards */
                RXTXstate = 1;			/* prepare the status counter of FIFO */


		CounterSet();			/* TimerA set */
		countValue = 0x5000; 
		startCounter;			/* start timer */
                
                LPM0;

		k = 0;
		while(irq_reg == 0x01)
		{				/* wait for RX complete */
			k++;
			if(k == 0xFFF0)
			{
				irq_reg = 0x00;    
				RXErrorFlag = 0x00;  /* Reset RXErrorFlag */
				break;
			}
		}

                command = RSSILevels;
		Read_Address(&command);  /* read rssi levels */
                
		if(irq_reg == 0xFF)
		{		
                      /* recieved UID in buffer */
                      found = true;
                      
                      /* Output UID to terminal */
                      SendT("     UID:");
                      for(m = 10; m>2; m--) //Put UID in output terminal
                      {
                         SendByte(buf[m]); 
                      }
                      SendT("\r\n"); // add new line
		}
		else if(irq_reg == 0x02)
		{	/* collision occured */
                      SendT("     Collision Error\r\n");
		}
		else if(irq_reg == 0x00)
		{	/* timer interrupt */
		      SendT("     Timer Interrupt\r\n");
		}
		else
                      ;			

		ResetFIFO;

		if((NoSlots == 17) && (i < 16))
		{	
                        /* if 16 slots used send EOF(next slot) */
			BlockReceivers;
                        StartReceivers;
                        TXNextTimeSlot; 
		}
		else if((NoSlots == 17) && (i == 16))
		{					/* at the end of slot 16 stop the slot counter */
                        DisableSlotCounter();
		}
                
	} /* end for */

        if(found)
        {					
               P5OUT |= 0x01;   // Light up LED at P5.0
        }
        else
        {
              P5OUT &= ~0x01;   // Off LED
        }
	offIRQ;
}


/***************************************************************
 * Description: Request read from tag or write to tag 
 ***************************************************************/
unsigned char RequestCommand(unsigned char length, unsigned char brokenBits)
{
        unsigned char j;
          
        buf[0] = 0x8F;                          /* Reset FIFO */
        buf[1] = 0x91;                          /* Command to Transmit with CRC */
        buf[2] = 0x3D;                          /* Write continous from register 1D - TXLength1 and TXLength2 */
        buf[3] = length >> 4;                   /* Length is how many bytes will be written to the FIFO */
        buf[4] = (length << 4) | brokenBits;

        Direct_Write(&buf[0], length + 5);     	/* writing to FIFO */
	clearIRQ;				
	onIRQ;
       
        irq_reg = 0x01;                         /* wait for end of TX */
        RXTXstate = 1;	                        /* the response will be stored in buf[1] upwards */
        
	/* wait for end of transmit */
	while(irq_reg == 0x01)
	{
		CounterSet();                   /* Set up Timer A */
		countValue = 0xF000;	
		startCounter;		        /* start timer */
		LPM0;
	}
        
        CounterSet();                           /* Set Up Timer A */
        countValue = 0xF000; 
        startCounter;		                /* start timer */
              
      
        if                                      /* Check to see if we are writing to a tag */
	(
		(((buf[5] & BIT6) == BIT6) && ((buf[6] == 0x21) || (buf[6] == 0x24) || (buf[6] == 0x27) || (buf[6] == 0x29)))
	||	(buf[5] == 0x00 && ((buf[6] & 0xF0) == 0x20 || (buf[6] & 0xF0) == 0x30 || (buf[6] & 0xF0) == 0x40))
	)
        {
                wait_ms(20);
                ResetFIFO;
                TXNextTimeSlot;
        }     /* end if */
  
        while(irq_reg == 0x01)                  /* wait for RX complete */
        {			
        }
      
        if(irq_reg == 0xFF)
        {	/* received response */
                SendT("     [");
                for(j = RXTXstate-1; j > 2; j--)
                {
                  SendByte(buf[j]);
                }	  /* for */

                SendT("]\r\n");
                return(0);
        }
        else if(irq_reg == 0x02)
        {	/* collision occured */
                SendT("     Collision Occured\r\n");
                return(1);
        }
        else if(irq_reg == 0x00)
        {	/* timer interrupt */
                SendT("     Timer Interrupt\r\n");
                return(1);
        }
        else
                ;
        
        ResetFIFO;
	offIRQ;
	return(1);
}

#endif
