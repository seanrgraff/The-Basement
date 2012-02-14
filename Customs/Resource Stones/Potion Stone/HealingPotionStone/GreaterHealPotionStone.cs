using System;
using Server.Items;

namespace Server.Items
{
	public class GreaterHealPotionStone : Item
	{
		public override string DefaultName
		{
			get { return "A GreaterHealPotion Stone"; }
		}

		[Constructable]
		public GreaterHealPotionStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 53;
		}

		public override void OnDoubleClick( Mobile from )
		{
			for ( int x = 0; x < 10; x++ )
			{
				GreaterHealPotion healPot = new GreaterHealPotion();

				if ( !from.AddToBackpack( healPot ) && x == 9 )
				{
					from.SendMessage("Your backpack has too many items.");
					healPot.Delete();
				}
				else if ( !from.AddToBackpack( healPot ) )
				{
					healPot.Delete();
				}
			}
		}

		public GreaterHealPotionStone( Serial serial ) : base( serial )
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