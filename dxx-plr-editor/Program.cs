using System;
using System.IO;

namespace dxxplreditor
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			const string default_filename = "Z:/Games/Descent2/static.plr";

			string filename;

			Console.WriteLine ("args length =" + args.Length);

			if (args.Length > 0) {
				filename = args [0];
			} else {
				filename = default_filename;
			}

			PlrClass plr = new PlrClass();
			plr.ImportFromFile (filename);
			plr.Dump ();
			plr.ExportToFile (filename);
			Console.ReadKey ();
		}
	}
}
