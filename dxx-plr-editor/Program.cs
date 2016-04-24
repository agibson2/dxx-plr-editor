using System;
using System.IO;

namespace dxxplreditor
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string filename = "Z:/Games/Descent2/static.plr";

			PlrClass plr = new PlrClass("STATIC.PLR");
			plr.ImportFromFile( filename );
			plr.Dump ();
			Console.ReadKey ();
		}
	}
}
