using System;
using Server;

namespace Server.Items
{
	public class LesserPoisonExplosionPotion : BasePoisonExplosionPotion
	{
		public override int MinDamage { get { return 5; } }
		public override int MaxDamage { get { return 7; } }

		[Constructable]
		public LesserPoisonExplosionPotion() : base( PotionEffect.ExplosionLesser )
		{
			Name = "a Lesser Poison Explosion Potion";
			Hue = 68;
		}

		public LesserPoisonExplosionPotion( Serial serial ) : base( serial )
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