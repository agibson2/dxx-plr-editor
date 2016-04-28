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
			plr.Dump ();

			if (pargs.primaryautoselect != null) {
				if (plr.SetPrimary_auto_select (pargs.primaryautoselect) == -1) {
					Console.WriteLine ("ERROR: Problem setting primaryautoselect");
					return(1);
				}
			}

			plr.Dump ();
			plr.ExportToFile (pargs.filename);
			Console.ReadKey ();

			return(0);
		}
	}
}
