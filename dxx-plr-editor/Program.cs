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

			if (args.Length > 0) {
				filename = args [1];
			} else {
				filename = default_filename;
			}

			PlrClass plr = new PlrClass();
			plr.ImportFromFile( filename );
			plr.Dump ();
			Console.ReadKey ();
		}
	}
}
