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
//  uart_functions.h                                                                     //
//***************************************************************************************//

#ifndef UART_FUNCTIONS_H
#define UART_FUNCTIONS_H

#include "msp430x16x.h"
#include <stdbool.h>
#include <string.h>

/* Global Variables */
bool DATA_IN_BUFFER;
bool DATA_MODE;
bool DEBUG = false;
unsigned int data_len;
unsigned int end_counter;
unsigned int size;
char patient_data[256];
char received_buffer[270];

/* Function Headers */
/* Description: Initialize UART Functionality and UART I/O pins */ 
void InitializeUART( void );

/* Description: Initialize UART control flags and data counters */
void InitializeFlags( void );

/* Description: Outputs a string through UART */
void SendStr(const char *string_to_sent);

/* Description: Outputs characters set up for terminal view when Debug Mode is ON */
void SendT(const char *string_to_sent);

/* Description: Outputs Hex values in Ascii */
void SendByte(unsigned char abyte);

/* Description: Convert Hex Nibble to Ascii */
unsigned char Nibble2Ascii(unsigned char anibble);

/* Description: Adds EOF character (five '\r') */
void AddEOF(void);

/* Description: Puts character byte into the UART transmit buffer */
void TXChar(unsigned char data);

/* Function Implementations */
/**********************************************************************
 * Description: Initialize UART functionality, set for 4800 baud rate with
 *              6.78 Mhz clock, 8 Data Bits, Odd Parity, 2 Stop Bits
 **********************************************************************/
void InitializeUART( void )
{
      /* Enable UART functionality */
      U0CTL |= SWRST; /* disable UART (to allow changes to register) */
      ME1 |= (UTXE0 + URXE0);  /* enable transmitter and receiver registers */
      U0CTL = (CHAR+PENA+SPB); /* Set 8 data bits, Odd parity, 2 stop bits */
      U0CTL &= ~SWRST; /* enable UART */
      
      /* Setup modulation of 6.78 Mhz clock to work with 4800 baud rate */
      U0TCTL = (TXEPT+SSEL0);  /* Select ACLK as clock source (6.78 Mhz) */
      U0BR0 = 0x84;  /* Baud rate control register 1 */
      U0BR1 = 0x05;  /* Baud rate control register 2 */
      U0MCTL = 0x55; /* Modulation control register */
      
      IE1 |= (URXIE0); /* Enable interrupts for UART RX */
        
       /* Enable pins for UART communication */
      P3DIR |= (BIT4); //Pin 3, bit 4 set to output direction 
      P3SEL |= (BIT4 + BIT5); //Pin 3, bit 4-5 set to UART mode
}


/***************************************************************
 * Description: Initialize UART control flags and data counters
 ***************************************************************/
void InitializeFlags( void )
{
      DATA_IN_BUFFER = false;
      size = 0;
      end_counter = 0;
      data_len = 0;
}


/***************************************************************
 * Description: Outputs a string through UART
 ***************************************************************/
void SendStr(const char *string_to_sent)
{
      while(*string_to_sent != '\0')
      {
            TXChar(*string_to_sent++);
      }
      AddEOF();
}


/***************************************************************
 * Description: Sends characters set up for terminal view when 
 *              Debug Mode is ON
 ***************************************************************/
void SendT(const char *string_to_sent)
{
  if(DEBUG)
  {    
      while(*string_to_sent != '\0')
      {
            TXChar(*string_to_sent++);
      }
  }

}


/***************************************************************
 * Description: Outputs Hex values in Ascii
 ***************************************************************/
void SendByte(unsigned char abyte)
{
    if(DEBUG)
    {
      unsigned char temp1, temp2;
     
      temp1 = (abyte >> 4) & 0x0F;
      temp2 = Nibble2Ascii(temp1);
      TXChar(temp2);
     
      temp1 = abyte & 0x0F;
      temp2 = Nibble2Ascii(temp1);
      TXChar(temp2);
    }
}


/***************************************************************
 * Description: Convert Hex Nibble to Ascii
 ***************************************************************/
unsigned char Nibble2Ascii(unsigned char anibble)
{
     unsigned char AsciiOut = anibble;
     if(anibble > 9)
     {
          AsciiOut = AsciiOut + 0x07;
     }
     AsciiOut = AsciiOut + 0x30;
     return(AsciiOut);
}

/***************************************************************
 * Description: Adds EOF character (five '\r')
 ***************************************************************/
void AddEOF(void)
{
      int j=0;
      for(j = 0; j < 5; j++)
      {
            TXChar('\r');
      }
}



/***************************************************************
 * Description: Puts character byte into the UART transmit buffer
 ***************************************************************/
void TXChar(unsigned char data)
{
      while(!(IFG1 & UTXIFG0)); /* Wait for TX buffer flag to be empty */
      TXBUF0 = data;
}




#endif
