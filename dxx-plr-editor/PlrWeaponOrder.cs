using System;

namespace dxxplreditor
{
	public class PlrWeaponOrder
	{
		public byte primaryOrder;
		public byte secondaryOrder;

		public PlrWeaponOrder ()
		{
		}

		public PlrWeaponOrder ( byte newPrimaryOrder, byte newSecondaryOrder )
		{
			primaryOrder = newPrimaryOrder;
			secondaryOrder = newSecondaryOrder;
		}
	}
}
