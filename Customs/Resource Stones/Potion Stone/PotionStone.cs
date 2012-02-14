using System;
using Server.Items;

namespace Server.Items
{
	public class PotionStone : Item
	{
		public override string DefaultName
		{
			get { return "A Potion Stone"; }
		}

		[Constructable]
		public PotionStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 1403;
		}

		public override void OnDoubleClick( Mobile from )
		{
			BagOfPotions potionBag = new BagOfPotions( 50 );

			if ( !from.AddToBackpack( potionBag ) )
			{
				from.SendMessage("Your backpack has too many items.");
				potionBag.Delete();
			}
		}

		public PotionStone( Serial serial ) : base( serial )
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