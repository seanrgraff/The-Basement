using System;
using Server;
using Server.Engines.Craft;


namespace Server.Items
{
	public class ValoriteHammer : RunicHammer1
	{

		[Constructable]
		public ValoriteHammer() : this( 50 )
		{
		}		

		[Constructable]
		public ValoriteHammer( int uses ) : base( CraftResource.Valorite )
		{
			Weight = 1.0;
			UsesRemaining = uses;
			Name = "Valorite Runic Hammer";
		}
		public ValoriteHammer( Serial serial ) : base( serial )
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