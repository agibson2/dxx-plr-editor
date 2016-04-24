# dxx-plr-editor
Descent 1 and 2 PLR file editor

The goal is to create a command line editor that can be used to change the multiplayer macros (f9 through f12) and handle color.  I have been hex editing the PLR files to get color in my macros which is a pain to say the least.  The idea is to be able to create batch files to launch D1 or D2 and change the multiplayer macros for different scenarios.  The batch file would first call this program to change the macros and then launch d1 or d2.  Sometimes you need macros for group messages like TIME OUT, etc.  Other times you just might want messages for fun like :), etc.

I wanted to be able to read in the entire PLR file and then write it back out exactly as it was before to make sure I am parsing the PLR file properly.  Descent 1 and Descent 2 store data differently in different sections of the PLR file and I want to be able to handle both in one binary.

Current status is that it can read in the D1 and D2 PLR files from a full version of Descent 1 or Descent 2 from d2x rebirth and d2x retro.  It also displays what was parsed.  The shareware version saves things differently so for now it just gives an error for them.

I am new to object oriented programming and c#.  My experience has been with straight C and I am rusty with it so don't expect too much as far as top quality coding :).
