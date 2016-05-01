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
   (supports color too... run program with /help to see valid colors)
   Example...  dxx-plr-editor.exe /f9 "/rThis is dark red/wThis is white" "C:\games\descent 2\myname.plr"
   
Valid primary weapon names for D2 are:
    laser vulcan spreadfire plasma fusion superlaser gauss helix phoenix omega
Valid secondary weapon names for D2 are:
    concussion homing proximity smartmissile mega flash guided smartmine mercury earthshaker
	
The shareware version saves things differently so for now I try to
determine that it is shareware PLR file and stop importing the .PLR file.

This is the command line I use to create my macros and primary / secondary auto select...

```
dxx-plr-editor.exe /overwrite /f9 "/R}/O|/R->" /f10 "/R}/O|/w./R-(/w..." /f11 "/R}/O|/R-//" /f12 "/O*** /RTIMEOUT! TIMEOUT! /O***" /primaryautoselect omega,gauss,plasma,spreadfire,vulcan,superlaser,laser /secondaryautoselect mega,smart,mercury,homing "z:\games\descent2\static.plr"
```

Help output via dxx-plr-editor.exe /help

```
dxx-plr-editor.exe v0.2.0 - Command line Descent 1 and 2 .PLR file editor tool

dxx-plr-editor.exe [/primaryautoselect weaponlist] [/secondaryautoselect weaponlist
                   [/f9 text] [/f10 text] [/f11 text] [/f12 text] [/overwrite]
                   [/debug] filename.plr

  Options:
    /primaryautoselect weaponlist  Change primary autoselect list (, separated list)
         d2 primary weaponlist: laser,vulcan,spreadfire,plasma,fusion
                                superlaser,gauss,helix,phoenix,omega

    /secondaryautoselect weaponlist Change secondary autoselect list (, separated list
         d2 secondary weaponlist: concussion,homing,proximity,smartmissile,mega
                                  flash,guided,smartmine,mercury,earthshaker

    /f9 <text>     Sets F9 macro text (supports color using color format below)
    /f10 <text>    Sets F10 macro text (supports color using color format below)
    /f11 <text>    Sets F11 macro text (supports color using color format below)
    /f12 <text>    Sets F12 macro text (supports color using color format below)

    /debug         Print lots of debug output
    /overwrite     Overwrite the existing .PLR instead of creating a .new file

  Examples:
    dxx-plr-editor.exe /primaryautoselect omega,plasma /secondaryautoselect mercury,smartmissile "C:\descent 2\adam.plr"
      Sets the primary autoselect so that omega is first then plasma.
      Sets the secondary autoselect so that mercury is first and then smartmissile.

    dxx-plr-editor.exe /f9 "/rDARKRED/wWHITE/OBRIGHTORANGE/pDARKPURPLE" /f10 "/OTHIS is ORANGE" C:\adam.plr
      Sets f9 macro to 'DARKRED WHITE BRIGHTORANGE DARKPURPLE' with each word
        a different color.

    Macro f9 through f12 color format:  (more colors to come soon)
      /r = dark red, /R = bright red, /y = yellow, /w = white, /o = orange
      /p = dark purple, /P = bright purple, /g = dark green
      *To print a / character use two //... /f9 "this is a //"

   NOTES:  By default the new .plr file is written to a new file in the same directory as
           as the original .PLR file with a .new extension to be safe.  You can make it
           write over the existing file with the /overwrite option.  Make sure you
           backup your existing .PLR file just in case there is a bug and this editor
           corrupts your .PLR file.  I am not aware of any issues but just in case...

           /primaryautoselect and /secondaryautoselect only support d2 .PLR files currently
```
