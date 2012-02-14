using System;
using Server.Items;

namespace Server.Items
{
	public class GreaterExplosionPotionStone : Item
	{
		public override string DefaultName
		{
			get { return "A GreaterExplosionPotion Stone"; }
		}

		[Constructable]
		public GreaterExplosionPotionStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 18;
		}

		public override void OnDoubleClick( Mobile from )
		{
			for ( int x = 0; x < 10; x++ )
			{
				GreaterExplosionPotion expPot = new GreaterExplosionPotion();

				if ( !from.AddToBackpack( expPot ) && x == 9 )
				{
					from.SendMessage("Your backpack has too many items.");
					expPot.Delete();
				}
				else if ( !from.AddToBackpack( expPot ) )
				{
					expPot.Delete();
				}
			}
		}

		public GreaterExplosionPotionStone( Serial serial ) : base( serial )
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