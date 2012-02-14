using System;
using Server.Items;

namespace Server.Items
{
	public class LeatherStone : Item
	{
		public override string DefaultName
		{
			get { return "A Leather Stone"; }
		}

		[Constructable]
		public LeatherStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 1403;
		}

		public override void OnDoubleClick( Mobile from )
		{
			BagOfLeather leatherBag = new BagOfLeather( 200 );

			if ( !from.AddToBackpack( leatherBag ) )
				leatherBag.Delete();
		}

		public LeatherStone( Serial serial ) : base( serial )
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