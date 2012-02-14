using Server;
using Server.Items;
using Server.Mobiles;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Server.Accounting;
using Server.Menus;
using Server.Menus.Questions;
using Server.Menus.ItemLists;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Targets;
using Server.Gumps;
using Server.Commands.Generic;
using Server.Diagnostics;

namespace Server.RabbitsVsSheep
{
		[FlipableAttribute( 0x170b, 0x170c )]
		public class SpeedBoots : BaseShoes
		{
			public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

			[Constructable]
			public SpeedBoots() : this( 0 )
			{
			}

			[Constructable]
			public SpeedBoots( int hue ) : base( 0x170B, hue )
			{
				Name = "Speed Boots";
				Hue = 1260;
				Weight = 3.0;
			}
		
			public override bool OnEquip( Mobile m ) 
			{ 
				m.Send( SpeedControl.MountSpeed );
				m.SendMessage( "You feel lighter than air!" );
				return base.OnEquip( m );
			}	 

			public override void OnRemoved( object parent ) 
			{ 
				if ( parent is Mobile ) 
				{ 
					Mobile m = (Mobile)parent;
					m.Send( SpeedControl.Disable );
				} 
			} 


			public SpeedBoots( Serial serial ) : base( serial )
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
