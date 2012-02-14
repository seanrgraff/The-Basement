using System; 
using Server; 
using Server.Items;

namespace Server.Items 
{ 
	public class BagOfLeather : Bag 
	{ 
		[Constructable] 
		public BagOfLeather() : this( 1000 ) 
		{ 
		} 

		[Constructable] 
		public BagOfLeather( int amount ) 
		{ 

			Name = "Bag of Leather";	

			DropItem( new HornedLeather( amount ) );
			DropItem( new BarbedLeather( amount ) );
			DropItem( new SpinedLeather( amount ) );
			DropItem( new Leather( amount ) );

		} 

		public BagOfLeather( Serial serial ) : base( serial ) 
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
