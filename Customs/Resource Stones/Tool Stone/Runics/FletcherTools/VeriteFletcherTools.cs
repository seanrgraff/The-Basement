using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class VeriteFletcherTools : RunicFletcherTools
	{

		[Constructable]
		public VeriteFletcherTools() : this( 50 )
		{
		}		

		[Constructable]
		public VeriteFletcherTools( int uses ) : base( CraftResource.Verite )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Runic Fletcher Tools";
		}
		public VeriteFletcherTools( Serial serial ) : base( serial )
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