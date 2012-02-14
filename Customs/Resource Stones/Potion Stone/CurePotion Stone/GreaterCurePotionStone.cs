using System;
using Server.Items;

namespace Server.Items
{
	public class GreaterCurePotionStone : Item
	{
		public override string DefaultName
		{
			get { return "A GreaterCurePotion Stone"; }
		}

		[Constructable]
		public GreaterCurePotionStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 43;
		}

		public override void OnDoubleClick( Mobile from )
		{
			for ( int x = 0; x < 10; x++ )
			{
				GreaterCurePotion curePot = new GreaterCurePotion();

				if ( !from.AddToBackpack( curePot ) && x == 9 )
				{
					from.SendMessage("Your backpack has too many items.");
					curePot.Delete();
				}
				else if ( !from.AddToBackpack( curePot ) )
				{
					curePot.Delete();
				}
			}
		}

		public GreaterCurePotionStone( Serial serial ) : base( serial )
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