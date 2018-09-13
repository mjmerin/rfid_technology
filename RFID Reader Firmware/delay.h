//  Riley Higa, Andrew Kurniadi, Min Kyeong Lee, Mark Merin, Robert Nguon.
//  ECE 08.5: RFID Technology for Ultrasound
//  Liason Engineer: Matthew Rieger
//  Faculty Advisor: Dr. Robert Heeren
//  Seattle University / Philips Healthcare
//  Built with IAR Embedded Workbench IDE

#ifndef DELAY_H
#define DELAY_H

void wait_ms(unsigned int n_ms);

void wait_ms(unsigned int n_ms) {
    unsigned int ii1, ii0;
    for(ii0=n_ms; ii0>0; ii0--) {
        ii1 = 0x07FF;                    // Delay
        do (ii1--);
        while (ii1 != 0);
    }
}

#endif
