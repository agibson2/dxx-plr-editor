# dxx-plr-editor
Descent 1 and 2 PLR file editor

The goal is to create a command line utility that can be used to change the multiplayer macros
(f9 through f12 including color, change weapon autoselect order, etc.  I have been hex editing
the PLR files to get color in my macros which is a pain to say the least.  The idea is to be able
to create batch files to launch D1 or D2 and change the multiplayer macros and weapon search order for
different scenarios.  The batch file would first call this program to change the macros and then
launch d1 or d2.  Sometimes you need macros for group messages like TIME OUT, etc.  Other times
you just might want messages for fun like :), etc.

I wanted to be able to read in the entire PLR file and then write it back out exactly as it was
before to make sure I am parsing the PLR file properly.  Descent 1 and Descent 2 store data
differently in different sections of the PLR file and I want to be able to handle both in one
binary.

Current status...

- Reads d1 and d2 PLR files of full version (not shareware PLR files)
- Prints parsed information from the PLR file (debug output)
- Writes a new PLR file with a .new extension to the same directory
- Can change primary autoselect for Descent 2 PLR files using /primaryautoselect option
   example:  dxx-plr-editor.exe /primaryautoselect gauss,plasma,superlaser C:\descent2\myname.plr
                    The order would be gauss then plasma then superlaser and all other primary
					weapons will not be autoselected.

Valid primary weapon names for D2 are:
    laser vulcan spreadfire plasma fusion superlaser gauss helix phoenix omega

The shareware version saves things differently so for now I try to
determine that it is shareware PLR file and stop importing the PLR file.