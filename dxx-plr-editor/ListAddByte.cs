using System;
using System.Collections.Generic;

namespace dxxplreditor
{
	public static class MyExtensions
	{
		public static void AddByte(this List<byte[]> list, byte item)
		{
			byte[] tempByte = new byte[1];
			tempByte[0] = (byte) item;
			list.Add (tempByte);
		}
	}
}

