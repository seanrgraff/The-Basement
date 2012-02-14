using System;
using Server;

namespace Server.Items
{
	public class FragmentationPotion : BaseFragmentationPotion
	{
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 20; } }

		[Constructable]
		public FragmentationPotion() : base( PotionEffect.Explosion )
		{
			Name = "a Fragmentation Potion";
			Hue = 1167;
		}

		public FragmentationPotion( Serial serial ) : base( serial )
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