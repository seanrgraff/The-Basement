using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class DullCopperHammer : RunicHammer1
	{

		[Constructable]
		public DullCopperHammer() : this( 50 )
		{
		}		

		[Constructable]
		public DullCopperHammer( int uses ) : base( CraftResource.DullCopper )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Dull Copper Runic Hammer";
		}
		public DullCopperHammer( Serial serial ) : base( serial )
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