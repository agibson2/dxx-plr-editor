using System;
using System.IO;

namespace dxxplreditor
{
	class MainClass
	{
		public static int Main (string[] args)
		{
			ParseArgs pargs = new ParseArgs();
			int pargsRetval = pargs.Parse (args);
			if (pargsRetval == -1) {
				return(1);
			}

			PlrClass plr = new PlrClass();

			if(!(File.Exists(pargs.filename))) {
				Console.WriteLine ("ERROR: {0} file not found", pargs.filename);
				return(1);
			}

			plr.ImportFromFile (pargs.filename);

			if (pargs.overwrite) {
				plr.overwriteExistingFile = true;
			}

			if (pargs.quiet) {
				plr.displayInfoMessages = false;
			}
				
			if (pargs.debug) {
				plr.debugEnabled = true;
			}

			if (plr.debugEnabled) {
				Console.WriteLine ("*********** Debug: Dump of PLR file contents BEFORE changes ************");
				plr.Dump ();
			}

			if (pargs.f9 != null) {
				if (plr.SetMultiplayer_macro_f9 (pargs.f9) == -1) {
					return(1);
				}
			}

			if (pargs.f10 != null) {
				if (plr.SetMultiplayer_macro_f10 (pargs.f10) == -1) {
					return(1);
				}
			}

			if (pargs.f11 != null) {
				if (plr.SetMultiplayer_macro_f11 (pargs.f11) == -1) {
					return(1);
				}
			}

			if (pargs.f12 != null) {
				if (plr.SetMultiplayer_macro_f12 (pargs.f12) == -1) {
					return(1);
				}
			}

			if (pargs.primaryautoselect != null) {
				if (plr.SetPrimary_auto_select (pargs.primaryautoselect) == -1) {
					Console.WriteLine ("ERROR: Problem setting primaryautoselect");
					return(1);
				}
			}

			if (pargs.secondaryautoselect != null) {
				if (plr.SetSecondary_auto_select (pargs.secondaryautoselect) == -1) {
					Console.WriteLine ("ERROR: Problem setting secondaryautoselect");
					return(1);
				}
			}

			if (pargs.cleanupmissions) {
				if (plr.CleanupMissions () == -1) {
					Console.WriteLine ("ERROR: Problem removing mission status entries with maximum level at 1 or less.");
					return(1);
				}
			}

			if (plr.debugEnabled) {
				Console.WriteLine ("*********** Debug: Dump of PLR file contents AFTER changes ************");
				plr.Dump ();
			}

			plr.ExportToFile (pargs.filename);
			if (plr.debugEnabled) {
				Console.WriteLine ("Press a key to exit.");
				Console.ReadKey ();
			}

			return(0);
		}
	}
}
