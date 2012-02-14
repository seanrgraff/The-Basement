using System;
using Server;

namespace Server.Items
{
	public class PoisonExplosionPotion : BasePoisonExplosionPotion
	{
		public override int MinDamage { get { return 7; } }
		public override int MaxDamage { get { return 15; } }

		[Constructable]
		public PoisonExplosionPotion() : base( PotionEffect.Explosion )
		{
			Name = "a Poison Explosion Potion";
			Hue = 68;
		}

		public PoisonExplosionPotion( Serial serial ) : base( serial )
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