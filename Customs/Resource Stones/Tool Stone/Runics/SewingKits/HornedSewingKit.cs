using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class HornedSewingKit : RunicSewingKit1
	{

		[Constructable]
		public HornedSewingKit() : this( 50 )
		{
		}		

		[Constructable]
		public HornedSewingKit( int uses ) : base( CraftResource.HornedLeather )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Horned Runic Sewing Kit";

		}
		public HornedSewingKit( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}