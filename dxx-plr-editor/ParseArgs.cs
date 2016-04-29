using System;

namespace dxxplreditor
{
	public class ParseArgs
	{
		public string filename = "Z:\\Games\\Descent\\static.plr";

		public bool debug = false;
		public bool quiet = false;
		public string[] primaryautoselect;
		public string[] secondaryautoselect;
		public string f9;
		public string f10;
		public string f11;
		public string f12;

		public ParseArgs()
		{
		}

		public int Parse (string[] args)
		{
			if (args.Length > 0) {
				int count = 0;

				while (count < args.Length) {
					if (args [count].Equals ("/primaryautoselect")) {
						count++;
						if (debug) {
							Console.WriteLine ("OPTION /primaryautoselect");
						}
						if (count < args.Length) {
							primaryautoselect = args [count++].Split (',');
						} else {
							Console.WriteLine ("ERROR: /primaryautoselect option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/secondaryautoselect")) {
						count++;
						if (debug) {
							Console.WriteLine ("OPTION /secondaryautoselect");
						}
						if (count < args.Length) {
							secondaryautoselect = args [count++].Split (',');
						} else {
							Console.WriteLine ("ERROR: /secondaryautoselect option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/f9")) {
						Console.WriteLine ("ERROR: /f9 not implemented yet");
						return(-1);
					} else if (args [count].Equals ("/f10")) {
						Console.WriteLine ("ERROR: /f10 not implemented yet");
						return(-1);
					} else if (args [count].Equals ("/f11")) {
						Console.WriteLine ("ERROR: /f11 not implemented yet");
						return(-1);
					} else if (args [count].Equals ("/f12")) {
						Console.WriteLine ("ERROR: /f12 not implemented yet");
						return(-1);
					} else if (args [count].Equals ("/debug")) {
						debug = true;
						count++;
					} else if (args [count].Equals ("/quiet")) {
						quiet = true;
						count++;
					} else if (args [count].Equals ("/help")) {
						Console.WriteLine ("");
						Console.WriteLine ("dxx-plr-editor.exe [/autoselectprimary <weapon,list>] [/autoselectsecondary <weapon,list>] player.plr");
						Console.WriteLine ("  v0.1.4");
						Console.WriteLine ("  Command line .PLR file editor tool");
						Console.WriteLine ("  /primaryautoselect  (requires , separated parameter list of primary weapons)");
						Console.WriteLine ("         d2: laser,vulcan,spreadfire,plasma,fusion,superlaser,gauss");
						Console.WriteLine ("             helix,phoenix,omega");
						Console.WriteLine ("");
						Console.WriteLine ("  /secondaryautoselect  (requires , serparated parameter list of primary weapons)");
						Console.WriteLine ("         d2: concussion,homing,proximity,smartmissile,mega,flash,guided");
						Console.WriteLine ("             smartmine,mercury,earthshaker");
						Console.WriteLine ("");
						Console.WriteLine ("  example: dxx-plr-editor.exe /primaryautoselect omega,plasma /secondaryautoselect mercury,smartmissile adam.plr");
						Console.WriteLine ("");
						Console.WriteLine ("   NOTES:  Currently the .plr file is written to a new file in the same directory as");
						Console.WriteLine ("           as the original .PLR file with a .new extension until I am confident that");
						Console.WriteLine ("           the new file generated is safe so you don't lose your original .PLR data.");
						Console.WriteLine ("   /primaryautoselect and /secondaryautoselect only support d1 .PLR files currently");
						Console.WriteLine ("");
					} else {
						if (args [count] [0] == '/') {
							Console.WriteLine ("ERROR: '{0}' is not a valid command line option", args [count]);
							return(-1);
						}
						filename = args [count];
						count++;
					}
				}
			}

			return(1);
		}
	}
}

