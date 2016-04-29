using System;
using System.IO;

namespace dxxplreditor
{
	class MainClass
	{
		public static int Main (string[] args)
		{
			ParseArgs pargs = new ParseArgs();
			if (pargs.Parse(args) == -1) {
				Console.WriteLine ("ERROR: ParseArgs failed");
				return(1);
			}

			PlrClass plr = new PlrClass();

			if(!(File.Exists(pargs.filename))) {
				Console.WriteLine ("ERROR: {0} file not found", pargs.filename);
				return(1);
			}

			plr.ImportFromFile (pargs.filename);

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
