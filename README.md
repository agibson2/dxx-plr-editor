# dxx-plr-editor
Descent 1 and 2 PLR file editor

The goal is to create a command line utility that can be used to change the multiplayer macros
(f9 through f12 including color, change weapon autoselect order, etc.  I have been hex editing
the PLR files to get color in my macros which is a pain to say the least.  The idea is to be able
to create batch files to launch D1 or D2 and change the multiplayer macros and weapon search order for
different scenarios.  The batch file would first call this program to change the macros and then
launch d1 or d2.  Sometimes you need macros for group messages like TIME OUT, etc.  Other times
you just might want messages for fun like :), etc.

Current status...

- Reads d1 and d2 .PLR files of full version (not shareware PLR files)
- Prints parsed information from .PLR file and other debug info with /debug option
- Writes a new PLR file with a .new extension to the same directory by default to be safe
   (can overwrite existing .PLR file with /overwrite option   BACK UP YOUR .PLR file first just in case!)
- Can change primary autoselect for Descent 2 .PLR files using /primaryautoselect option
   example:  dxx-plr-editor.exe /primaryautoselect gauss,plasma,superlaser "C:\descent 2\myname.plr"
                    The order would be gauss then plasma then superlaser and all other primary
					weapons will not be autoselected.
- Can change secondary autoselect for Descent 2 .PLR files using /secondaryautoselect option
   example:  dxx-plr-editor.exe /secondaryautoselect mercury,smartmissile,mega "C:\descent 2\myname.plr"
                    The order would be mercury then smartmissile then mega and all other secondary
					weapons will not be autoselected.
- Can change f9, f10, f11, and f12 macros with /f9 "Oh no you got me!" /f10 "Ouch!", etc
   (supports color too... run program with /help to list valid colors)
   Example...  dxx-plr-editor.exe /f9 "/rThis is dark red/wThis is white" "C:\games\descent 2\myname.plr"
   
Valid primary weapon names for D2 are:
    laser vulcan spreadfire plasma fusion superlaser gauss helix phoenix omega
Valid secondary weapon names for D2 are:
    concussion homing proximity smartmissile mega flash guided smartmine mercury earthshaker
	
The shareware version saves things differently so for now I try to
determine that it is shareware PLR file and stop importing the .PLR file.