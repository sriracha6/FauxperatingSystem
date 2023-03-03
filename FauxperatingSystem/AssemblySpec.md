# Assembly Spec
Same syntax as regular assembly.  

```asm
; data section
.data: ; comment
msg: DB 'Hello world!', 10

.text:


int 0x67E0 ; print function
```

INTERRUPTS: These include the interrupt prefix (11100)  
| Code   |  Description   | Params |
| ------ | -------------- | ------ |
| 0x6400 | Clear screen  | None |
| 0x6401 | Print text  | R0 is the location in memory of a variable. Print the text at that variable.  |
| 0x6410 | Change foreground color  | R0 is a number from 0-15. The numbers correspond to the default Windows console color codes. |
| 0x6411 | Change background color  | R0 is a number from 0-15. The numbers correspond to the default Windows console color codes. |
| 0x6412 | Reset color | None |
| 0x6420 | Set cursor position | Set cursor position to (R0, R1). Center is top left. |
| 0x6421 | Get cursor position | Put cursor position into registers (R0, R1). |
| 0x6440 | Set window width and height | Set window width and height to (R0, R1). |
| 0x6441 | Get window width and height | Put value of window width and height into (R0, R1). |
| 0x6442 | Set window top and left | Set window top and left to (R0, R1). |
| 0x6443 | Get window top and left | Put value of window top and left into (R0, R1). |
| 0x6480 | Read line | Let user type a line and submit it with [enter]. Stored value is at the memory location at R0. Beware of segfaults. |
| 0x6481 | Read line (limit length) | Let user type a line and submit it with [enter]. Stored value is at the memory location at R0. Maximum length is the value of R1. |
| 0x6482 | Read key (shown) | Let user press a key. The key is shown. Stored value is at the memory location at R0 as a short corresponding to the windows key codes. Max number is 254. |
| 0x6483 | Read key (hidden) | Let user press a key. The key is NOT shown. Stored value is at the memory location at R0 |
| 0x64FF | Shutdown emulator | None |
| 0x6200 | Get current directory | Put the value of the current directory into R0 |
| 0x6201 | Create file | Create file with the path equal to the value of the location in memory at R0 |
| 0x6202 | Delete file | Delete file at the path equal to the value of the location in memory at R0 |
| 0x6203 | Create directory | Create directory with the path equal to the value of the location in memory at R0 |
| 0x6204 | Delete empty directory | Delete an empty directory with the path equal to the value of the location in memory at R0 |
| 0x6205 | Delete full directory | Delete a full directory with the path equal to the value of the location in memory at R0 |
| 0x6206 | Change current directory | Change current directory to value at memory location R0 |
| 0x6210 | Directory count | Put the value of the number of files in direct at R0 into R1 |
| 0x6211 | Directory size | Put the value of the total size of files in bytes at R0 into R1 |
| 0x6212 | Find file | Put the value of the first instance of file with name containing R0 into R1 |
| 0x6213 | File size | Put the value of the total size of a file at memory location R0 into R1 |
| 0x6214 | Move file | Move file of name at memory address at R0 to name at memory address R1 |
| 0x6220 | Open file | Open file at memory location R0 |
| 0x6221 | Write file (replace) | Write to the file currently open, replacing. Replace with variable at memory location R0. |
| 0x6222 | Append file | Append to the file currently open. Append with variable at memory location R0. |
| 0x6223 | Load file (into memory) | Load first R1 bytes into memory location R0. |
| 0x622F | Close file | None |