/////////////////////////////////////////////////
//
// Automatically generated by the
// AddonGenerator script by Arya
//
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BrownStoneFountainAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new BrownStoneFountainAddonDeed();
			}
		}

		[ Constructable ]
		public BrownStoneFountainAddon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent( 6595 );
			AddComponent( ac, -1, 2, 0 );
			ac = new AddonComponent( 6596 );
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 6597 );
			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 6598 );
			AddComponent( ac, 2, 2, 0 );
			ac = new AddonComponent( 6599 );
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 6600 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 6601 );
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 6602 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 6603 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 6604 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 6605 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 6606 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 6607 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 6608 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 6609 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 6611 );
			AddComponent( ac, -1, -1, 0 );

		}

		public BrownStoneFountainAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class BrownStoneFountainAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new BrownStoneFountainAddon();
			}
		}

		[Constructable]
		public BrownStoneFountainAddonDeed()
		{
			Name = "BrownStoneFountain";
		}

		public BrownStoneFountainAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}