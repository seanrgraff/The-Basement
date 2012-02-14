using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class BagOfPotions : Bag 
	{ 
		[Constructable] 
		public BagOfPotions() : this( 1000 ) 
		{ 
		} 

		[Constructable] 
		public BagOfPotions( int amount ) 
		{ 

			Name = "Bag of Potions";	

			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new TotalRefreshPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterExplosionPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterHealPotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterCurePotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterStrengthPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );
			DropItem( new GreaterAgilityPotion() );

		} 

		public BagOfPotions( Serial serial ) : base( serial ) 
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
