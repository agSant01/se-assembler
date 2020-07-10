//sprt5Test5B.asm
//Pruebas dispositivos de I/O
//Se presume semaforo en address 50
//			 7-segment en address 60
//			 ASCII Display en address 70
//			 keyboard en Address 90
    org 0
    JMPADDR start
    org 100
semaforo db 0
    org 110
sevenseg db 0	
    org 120
asciidisp1 db 0
asciidisp2 db
    org 130
keyb db 0
    org 10   
start:	
    LOADIM R5,#0A8      //RyGrYg
    STORE semaforo,R5 

    LOADIM R5,#0FF      //Todas intermitentes
    STORE semaforo,R5 

    LOADIM R5,#0F2
    STORE sevenseg,R5	//3 en dÃ­gito izquierdo 
    LOAD R5,#0B7
    STORE sevenseg,R5	//5 en dÃ­gito derecho

    LOAD R5,keyb	    //Read from keyboard (test with 1)
    LOAD R6,keyb 	    //Read from keyboard (test with 2)
    ADDIM R5,#40	    //Generates ASCII for 'A' into R5
    ADDIM R6,#40	    //Generates ASCII for 'B' intor R6
    STORE asciidisp1,R5	//'A' into first ASCII character
    STORE asciidisp2,R6	//'A' into second ASCII character