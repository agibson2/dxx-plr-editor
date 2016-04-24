using System;

namespace dxxplreditor
{
	public class PlrMission
	{
		public string mission_name;
		public byte maximum_level;

		public PlrMission ()
		{
		}

		public PlrMission ( string newMission_name, byte newMaximum_level )
		{
			mission_name = newMission_name;
			maximum_level = newMaximum_level;
		} 
	}
}

