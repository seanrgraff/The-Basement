using System;
using Server;

namespace Server.Items
{
	public class GreaterPoisonExplosionPotion : BasePoisonExplosionPotion
	{
		//public override int MinDamage { get { return Core.AOS ? 20 : 15; } }
		//public override int MaxDamage { get { return Core.AOS ? 40 : 30; } }

		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 20; } }
		
		[Constructable]
		public GreaterPoisonExplosionPotion() : base( PotionEffect.ExplosionGreater )
		{
			Name = "a Greater Poison Explosion Potion";
			Hue = 68;
		}

		public GreaterPoisonExplosionPotion( Serial serial ) : base( serial )
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