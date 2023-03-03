# Assembly Spec
16-bit  
Each instruction is 2 bytes wide  
There are 8 general purpose registers. R0-R7  
Each instruction has a 5 bit opcode.  
The stack has a length of 8.  
If required, it has (a) 3 bit field(s) to specify one or more of the registers.  
Format:   
Register input: R  
11 bit immediate: I  
Destination: D (usually a register)  

| Instruction | Description |  OPCode    |
| ----------- | ----------- | ---------- |
| J I        | Jump to PC + signed I | 00000 |
| JR R        | Set PC = value in register | 00001 |
| JZ R, R     | Set PC = value in R(1) if R(2) is zero | 00010 |
| JNZ R, R     | Set PC = value in R(1) if R(2) is not zero | 00011 |
| NOP        | No operation | 11111 |
| MUL R, R, D | Multiply R(1) by R(2) and store in D | 10000 |
| DIV R, R, D | Divide R(1) by R(2) and store in D | 10001 |
| ADD R, R, D | Add R(1) and R(2) and store in D | 10010 |
| SUB R, R, D | Subtract R(2) from R(1) and store in D | 10011 |
| AND R, R, D | Store in D the bitwise AND of R(1) and R(2) | 10100 |
| OR R, R, D | Store in D the bitwise OR of R(1) and R(2) | 10101 |
| XOR R, R, D | Store in D the bitwise XOR of R(1) and R(2) | 10110 |
| XNOR R, R, D | Store in D the bitwise XNOR of R(1) and R(2) | 10111 |
| NOT R, D | Store in D the bitwise NOT of R | 11000 |
| SHR R, D | Store in D the SHR of R | 11001 |
| SHL R, D | Store in D the SHL of R | 11010 |
| INC R | Increment R | 01101 |
| DEC R | Decrement R | 01110 |
| SEQ R, R, D | Store a 1 in D if R(1) = R(2), else 0 | 01001 |
| SGT R, R, D | Store a 1 in D if R(1) > R(2), else 0 | 01010 |
| SLT R, R, D | Store a 1 in D if R(1) < R(2), else 0 | 01011 |
| INT I | Invoke an interrupt I | 01100 |
| CALL R | Call subroutine at the location in memory at R | 01111 |
| RET | Return from subroutine | 11011 |
| MOV R, R | Move R(1) to R(2) | 11100 |
| MOVV R, D | Move the location of the variable (D) in memory to R | 11101 |

Program (max length 64K):  
+---------------+  
|    Header:    |
|   2 bytes:    |  
|  .text loc.   |  
+---------------+  
|    Program    |  
|    .data      |  
|    .text      |  
|               |  
+---------------+  

Hard Drive:  
Stored in the same way as hard drives. Maximum size: 32mb.  
There are the headers and address list.  
This is stored on windows as regular files and converted into the hard drive file.  
Max path length (including file extension, file name, directory, and prefix (c:\\)) is 64