using System;

namespace dxxplreditor
{
	public class ParseArgs
	{
		public string filename = "Z:\\Games\\Descent2\\static.plr";

		public bool debug = false;
		public bool quiet = false;
		public string[] primaryautoselect;
		public string[] secondaryautoselect;
		public string f9;
		public string f10;
		public string f11;
		public string f12;
		public bool overwrite = false;
		public bool cleanupmissions = false;

		public ParseArgs()
		{
		}

		public int Parse (string[] args)
		{
			if (args.Length > 0) {
				int count = 0;

				while (count < args.Length) {
					if (args [count].Equals ("/primaryautoselect")) {
						if (debug) {
							Console.WriteLine ("OPTION /primaryautoselect");
						}
						count++;
						if (count < args.Length) {
							primaryautoselect = args [count].Split (',');
						} else {
							Console.WriteLine ("ERROR: /primaryautoselect option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/secondaryautoselect")) {
						if (debug) {
							Console.WriteLine ("OPTION /secondaryautoselect");
						}
						count++;
						if (count < args.Length) {
							secondaryautoselect = args [count].Split (',');
						} else {
							Console.WriteLine ("ERROR: /secondaryautoselect option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/f9")) {
						if (debug == true) {
							Console.WriteLine ("OPTION /f9");
						}
						count++;
						if (count < args.Length) {
							f9 = args [count];
							if (f9 == null) {
								Console.WriteLine ("ERROR: /f9 argument is null (bug!)");
								return(-1);
							}
						} else {
							Console.WriteLine ("ERROR: /secondaryautoselect option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/f10")) {
						if (debug == true) {
							Console.WriteLine ("OPTION /f10");
						}
						count++;
						if (count < args.Length) {
							f10 = args [count];
							if (f10 == null) {
								Console.WriteLine ("ERROR: /f10 argument is null (bug!)");
								return(-1);
							}
						} else {
							Console.WriteLine ("ERROR: /f10 option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/f11")) {
						if (debug == true) {
							Console.WriteLine ("OPTION /f11");
						}
						count++;
						if (count < args.Length) {
							f11 = args [count];
							if (f11 == null) {
								Console.WriteLine ("ERROR: /f11 argument is null (bug!)");
								return(-1);
							}
						} else {
							Console.WriteLine ("ERROR: /f11 option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/f12")) {
						if (debug == true) {
							Console.WriteLine ("OPTION /f12");
						}
						count++;
						if (count < args.Length) {
							f12 = args [count];
							if (f12 == null) {
								Console.WriteLine ("ERROR: /f12 argument is null (bug!)");
								return(-1);
							}
						} else {
							Console.WriteLine ("ERROR: /f12 option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/overwrite")) {
						if (debug == true) {
							Console.WriteLine ("OPTION: /overwrite");
						}
						overwrite = true;
					} else if (args [count].Equals ("/cleanupmissions")) {
						if (debug == true) { Console.WriteLine ("OPTION /cleanupmissions"); }
						cleanupmissions = true;
					} else if (args [count].Equals ("/debug")) {
						Console.WriteLine ("OPTION: /debug");
						debug = true;
					} else if (args [count].Equals ("/quiet")) {
						if (debug == true) { Console.WriteLine ("OPTION: /quiet"); }
						quiet = true;
					} else if (args [count].Equals ("/help")) {
						if (debug == true) { Console.WriteLine ("OPTION /help"); }
						Console.WriteLine ("");
						Console.WriteLine ("dxx-plr-editor.exe v0.2.2 - Command line Descent 1 and 2 .PLR file editor tool");
						Console.WriteLine ("");
						Console.WriteLine ("dxx-plr-editor.exe [/primaryautoselect weaponlist] [/secondaryautoselect weaponlist");
						Console.WriteLine ("                   [/f9 text] [/f10 text] [/f11 text] [/f12 text] [/overwrite]");
						Console.WriteLine ("                   [/debug] filename.plr");
						Console.WriteLine ("");
						Console.WriteLine ("  Options:");
						Console.WriteLine ("    /primaryautoselect weaponlist  Change primary autoselect list (, separated list)");
						Console.WriteLine ("         d2 primary weaponlist: laser,vulcan,spreadfire,plasma,fusion");
						Console.WriteLine ("                                superlaser,gauss,helix,phoenix,omega");
						Console.WriteLine ("");
						Console.WriteLine ("    /secondaryautoselect weaponlist Change secondary autoselect list (, separated list");
						Console.WriteLine ("         d2 secondary weaponlist: concussion,homing,proximity,smartmissile,mega");
						Console.WriteLine ("                                  flash,guided,smartmine,mercury,earthshaker");
						Console.WriteLine ("");
						Console.WriteLine ("    /f9 <text>        Sets F9 macro text (supports color using color format below)");
						Console.WriteLine ("    /f10 <text>       Sets F10 macro text (supports color using color format below)");
						Console.WriteLine ("    /f11 <text>       Sets F11 macro text (supports color using color format below)");
						Console.WriteLine ("    /f12 <text>       Sets F12 macro text (supports color using color format below)");
						Console.WriteLine ("");
						Console.WriteLine ("    /cleanupmissions  Removes level status from .plr file for levels with last");
						Console.WriteLine ("                      maximum level achieved equal to 1 which is not needed.");
						Console.WriteLine ("");
						Console.WriteLine ("    /debug            Print lots of debug output and force press a key to exit program");
						Console.WriteLine ("    /overwrite        Overwrite the existing .PLR instead of creating a .new file");
						Console.WriteLine ("");
						Console.WriteLine ("  Examples:");
						Console.WriteLine ("    dxx-plr-editor.exe /primaryautoselect omega,plasma /secondaryautoselect mercury,smartmissile \"C:\\descent 2\\adam.plr\"");
						Console.WriteLine ("      Sets the primary autoselect so that omega is first then plasma.");
						Console.WriteLine ("      Sets the secondary autoselect so that mercury is first and then smartmissile.");
						Console.WriteLine ("");
						Console.WriteLine ("    dxx-plr-editor.exe /f9 \"/rDARKRED/wWHITE/OBRIGHTORANGE/pDARKPURPLE\" /f10 \"/OTHIS is ORANGE\" C:\\adam.plr");
						Console.WriteLine ("      Sets f9 macro to 'DARKRED WHITE BRIGHTORANGE DARKPURPLE' with each word");
						Console.WriteLine ("        a different color.");
						Console.WriteLine ("");
						Console.WriteLine ("    Macro f9 through f12 color format:");
						Console.WriteLine ("      /r = dark red, /R = bright red, /Y = bright yellow, /w = white, /o = orange");
						Console.WriteLine ("      /O = bright orange, /p = dark purple, /P = bright purple, /g = dark green,");
						Console.WriteLine ("      /G = bright green");
						Console.WriteLine ("      *To print a / character use two //... /f9 \"this is a //\"");
						Console.WriteLine ("");
						Console.WriteLine ("   NOTES:  By default the new .plr file is written to a new file in the same directory as");
						Console.WriteLine ("           as the original .PLR file with a .new extension to be safe.  You can make it");
						Console.WriteLine ("           write over the existing file with the /overwrite option.  Make sure you");
						Console.WriteLine ("           backup your existing .PLR file just in case there is a bug and this editor");
						Console.WriteLine ("           corrupts your .PLR file.  I am not aware of any issues but just in case...");
						Console.WriteLine ("");
						Console.WriteLine ("           /primaryautoselect and /secondaryautoselect only support d2 .PLR files currently");
						Console.WriteLine ("");
						return(-2);  // return failure to make sure the the caller knows that they should not proceed doing things
					} else {
						if (args [count] [0] == '/') {
							Console.WriteLine ("ERROR: '{0}' is not a valid command line option", args [count]);
							return(-1);
						}
						if (debug == true) { Console.WriteLine ("FILENAME: {0}", args[count]); }
						filename = args [count];
					}
				
					count++;
				}
			}

			return(1);
		}
	}
}

