using System;
using Server.Items;

namespace Server.Items
{
	public class PouchStone : Item
	{
		public override string DefaultName
		{
			get { return "A Trapped Pouch Stone"; }
		}

		[Constructable]
		public PouchStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = 2207;
		}

		public override void OnDoubleClick( Mobile from )
		{
			for ( int x = 0; x < 5; x++ )
			{
				Pouch trappedBag = new Pouch();
				trappedBag.TrapType = TrapType.MagicTrap;
				trappedBag.TrapLevel = 1;
				trappedBag.TrapPower = 1;
	
				if ( !from.AddToBackpack( trappedBag ) )
				{
					from.SendMessage("Your backpack has too many items.");
					trappedBag.Delete();
				}
			}
		}

		public PouchStone( Serial serial ) : base( serial )
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