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
//  process_data.h                                                                       //
//***************************************************************************************//

#ifndef PROCESS_DATA_H
#define PROCESS_DATA_H

#include "uart_functions.h"
#include "rfid_functions.h"

/* Function Headers */
/* Description: Process Commands/Data coming from UART communications */
void ProcessData( void );

/* Description: After writing or reading patient data, clear the buffer */
void ClearPatientData( void );

/* Description: Write all the patient data to tag */
void WriteToTag( void );

/* Description: Write a block of patient data to the tag */
void WriteBlock(unsigned char address, char *data);

/* Description: Read all the patient data from the tag */
void ReadFromTag( void );

/* Description: Read a block of data from the tag */
void ReadBlock(unsigned char address);

/* Description: Check to see if data is padding */
bool CheckPad(unsigned char current_address);


/* Function Implementations */
/**********************************************************************
 * Description: Process Commands/Data coming from UART communications
 **********************************************************************/
void ProcessData( void )
{ 
  char byIn;
  int i;

  byIn = RXBUF0;
  received_buffer[size] = byIn;
  size++;
  
  /* We always assume all data or command will end with 5 \r's */
  if( byIn == '\r')
  {
        end_counter++;
        
        if(end_counter ==  5) /* Check for five '\r' delimiters to signify the end of incoming data */
        {
              if(!DATA_MODE)  /* If not in data mode, it must be a command */
              {
                  offIRQ;  
                  RXErrorFlag = 0x00; /* Reset TRF7960 RX error flag */
                  received_buffer[size-5] = '\0'; /* Add null at the end of the command to make strcmp work */
                 
                  if(!strcmp("debug_on",received_buffer))
                  {
                        DEBUG = true;
                        SendStr("Debug Mode: on\r\n");
                  }  
                  else if(!strcmp("debug_off",received_buffer))
                  {
                        DEBUG = false;
                        SendStr("Debug Mode: off\r\n");
                  }  
                  
                  /* Commands meant to come from Patient Data GUI */
                  else if(!strcmp("findtag",received_buffer))
                  {
                        CheckInventory();
                        if(found)
                          SendStr("YES");
                        else
                          SendStr("NO");
                  }  
                  else if(!strcmp("writetag",received_buffer)) /* write patient data to tag */
                  {
                    CheckInventory();
                    if(found) 
                    {
                          SendStr("OK");
                          DATA_MODE = true;
                    }
                    else
                    {
                          SendStr("FAIL");
                          DATA_MODE = false;
                    }
                  }
                  else if(!strcmp("readtag",received_buffer)) /* read patient data from tag */
                  {
                      ReadFromTag();
                      for(i=0; i < data_len; i++)
                      {
                           TXChar(patient_data[i]);
                      }
                      AddEOF();
                      ClearPatientData();
                  }
                  
                  /* Commands meant for debugging to be viewed from a terminal program */
                  else if(!strcmp("inventory",received_buffer)) /* check tag uid */
                  {
                        SendT("\nISO 15693 Inventory Request\n\r\n");
                        CheckInventory();
                        SendT("\nDone\n\r\n");
                  }
                  else if(!strcmp("write",received_buffer)) /* test write one block of data to tag */
                  {
                        SendT("\nISO 15693 Write Request\n\r\n");
                        buf[5] = 0x42;  /* ISO-15693 Flag = High Data Rate + Option Select */
                        buf[6] = 0x21;  /* ISO-15693 Write Block Command */
                        buf[7] = 0x02;  /* Tag Block Address */
                        buf[8] = 0x12;  /* Data block to be written is in buf[8] to buf[11] */
                        buf[9] = 0x34;
                        buf[10] = 0x56;
                        buf[11] = 0x78;
                        RequestCommand(7, 0x00);
                        SendT("\nDone\n\r\n");
                  }
                  else if(!strcmp("read",received_buffer)) /* test read one block of data from tag */
                  {
                        SendT("\nISO 15693 Read Request\n\r\n");
                        buf[5] = 0x42;  /* ISO-15693 Flag = High Data Rate + Option Select */
                        buf[6] = 0x20;  /* ISO-15693 Read Block Command */
                        buf[7] = 0x02;  /* Tag Block Address */ 
                        RequestCommand(3, 0x00);
                        SendT("\nDone\n\r\n");
                  }
                  else  /* command not recognized */
                  {
                        SendStr("Unsupported Command");
                  }
                  size = 0;  /* reset received_buffer size */
            }
            else    /* DATA_MODE is true, processing patient data */
            {
                  data_len = size - 5;  /* The size minus the five '\r' delimiters */
                                    
                  for (i = 0; i < data_len; i++)  /* Copy data from received_buffer to patient_data */
                  {
                      patient_data[i] =  received_buffer[i];
                  }
                  
                  size = 0;  /* reset received_buffer size */
                  
                  WriteToTag();  /* write data to tag */
                  ClearPatientData();
                
                  DATA_MODE = false;

            } 
            end_counter = 0;  /* reset the '\r' counter */
      }
  }
  else
  {
      end_counter = 0;    /* data is not a '\r', resets end counter */
      //Can get stuck in here if you send command/data without 5 '\r' delimiters. 
  }
  
  DATA_IN_BUFFER = false;  /* reset flag to allow more incoming data */
}


/************************************************************************
 * Description: After writing or reading patient data, clear the buffer
 ************************************************************************/
void ClearPatientData( void )
{
    int i;
    for(i=0;i<256;i++)
    {
        patient_data[i] = 0x00;
    }
}


/***************************************************************
 * Description: Write all the patient data to tag
 ***************************************************************/
void WriteToTag( void )
{
   int len = 0;
   char *current_data = &patient_data[0];
   unsigned char current_address = 0x00;
   while ( len < data_len)   
   {
     WriteBlock(current_address, current_data);
     current_data += 4;  /* go to the next block of data */
     current_address++;
     len++;
   }
   
   /* Add padding if there is less than 256 bytes of data */
    if(data_len != 256)
    {
      int count =  (256 - data_len) / 4;
      char pad[4] = {0x00,0x00,0x00,0x00};
      
      while (count >= 0)
      {
        WriteBlock(current_address, pad);
        current_address++;
        count--;
      }
    }

}


/***************************************************************
 * Description: Write a block of patient data to the tag
 ***************************************************************/
void WriteBlock(unsigned char address, char *data)
{
      buf[5] = 0x42;      /* ISO-15693 Flag = High Data Rate + Option Select */
      buf[6] = 0x21;      /* ISO-15693 Write Block Command */
      buf[7] = address;   /* Tag Block Address */
      buf[8] = *(data);   /* Data block to be written is in buf[8] to buf[11] */
      buf[9] = *(data+1);
      buf[10] = *(data+2);
      buf[11] = *(data+3);
      RequestCommand(7, 0x00);
}


/***************************************************************
 * Description: Read all the patient data from the tag
 ***************************************************************/
void ReadFromTag( void )
{
      int i;
      int count = 0;
      unsigned char current_address = 0x00;
      
      while(current_address < 0x40)
      {
            SendT("Reading Block ");
            SendByte(current_address);
            SendT("\r\n");
            
            ReadBlock(current_address);
            if(!CheckPad(current_address))
            {
                for(i=3; i<7; i++)
                {
                      patient_data[4*current_address+i-3] = buf[i]; 
                }
                current_address++;
                count++;
            }
            else
              break;
      }
      data_len = 4*count;
}


/***************************************************************
 * Description: Read a block of data from the tag
 ***************************************************************/
void ReadBlock(unsigned char address)
{
      buf[5] = 0x42;    /* ISO-15693 Flag = High Data Rate + Option Select */
      buf[6] = 0x20;    /* ISO-15693 Read Block Command */
      buf[7] = address; /* Tag Block Address */
      RequestCommand(3, 0x00);
      wait_ms(5);
}

/***************************************************************
 * Description: Check to see if data is padding
 ***************************************************************/
bool CheckPad(unsigned char current_address)
{
    int i;
    for(i=0; i<4; i++)
    {
        if(buf[i+3] != 0x00)
        {
            return false;
        }
    }
    return true;   /* encounters 4 0x00's, must be a pad */
}

#endif
