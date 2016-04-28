using System;

namespace dxxplreditor
{
	public class ParseArgs
	{
		public string filename = "Z:\\Games\\Descent\\static.plr";

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
						Console.WriteLine ("OPTION /primaryautoselect");
						if (count < args.Length) {
							primaryautoselect = args [count++].Split (',');
						} else {
							Console.WriteLine ("ERROR: /primaryautoselect option requires a parameter with a list of weapons separated by ,");
							return(-1);
						}
					} else if (args [count].Equals ("/secondaryautoselect")) {
						Console.WriteLine("ERROR: /secondaryautoselect not implemented yet");
						return(-1);

						//count++;
						//Console.WriteLine ("OPTION /secondaryautoselect (not implemented yet)");
						//if (count < args.Length) {
						//	secondaryautoselect = args [count++].Split (',');
						//} else {
						//	Console.WriteLine ("ERROR: /primaryautoselect option requires a parameter with a list of weapons separated by ,");
						//	return(-1);
						//}

						return(-1);
					} else if (args [count].Equals ("/f9")) {
						Console.WriteLine("ERROR: /f9 not implemented yet");
						return(-1);
					} else if (args [count].Equals ("/f10")) {
						Console.WriteLine("ERROR: /f10 not implemented yet");
						return(-1);
					} else if (args [count].Equals ("/f11")) {
						Console.WriteLine("ERROR: /f11 not implemented yet");
						return(-1);
					} else if (args [count].Equals ("/f12")) {
						Console.WriteLine("ERROR: /f12 not implemented yet");
						return(-1);
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

