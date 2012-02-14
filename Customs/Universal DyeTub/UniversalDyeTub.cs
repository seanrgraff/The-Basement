using System;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{

     public class UnivTubTarget : Target
     {
            private Item m_Item;

            public UnivTubTarget( Item item ) : base( 12, false, TargetFlags.None )
            {
                   m_Item = item;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
						if ( targeted is UniversalDyeTub )
						{
							from.SendGump( new UniversalDyeGump( (PlayerMobile)from, (UniversalDyeTub)targeted ));
						}
                        else if ( targeted is Item )
                        {
                             Item targ = (Item)targeted;
                             if ( !targ.IsChildOf (from.Backpack))
                             {
                                  from.SendMessage( "The item is not in your pack!" );
                             }
                             else
                             {
                                 targ.Hue=m_Item.Hue;
                                 from.PlaySound( 0x23F );
                             }
                        }

            }
     }


     public class UniversalDyeTub : Item
     {

            private bool m_Redyable;


            [Constructable]
            public UniversalDyeTub() : base( 0xFAB )
            {
                   Weight = 0.0;
                   Hue = 0;
                   Name = "Universal Dye Tub";
                   m_Redyable = false;
            }

            public UniversalDyeTub( Serial serial ) : base( serial )
            {
            }

            public override void OnDoubleClick( Mobile from )
            {

                   from.Target = new UnivTubTarget( this );
                   from.SendMessage( "What do you wish to dye? Select tub for hue menu." );

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


	public class UniversalDyeGump : Gump
	{
		private PlayerMobile m_From;
		private UniversalDyeTub m_Tub;
		
		private int hue;
				
		public UniversalDyeGump( PlayerMobile from, UniversalDyeTub tub ): base( 50, 50 )
		{
			m_From = from;
			m_Tub = tub;
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(50, 50, 250, 215, 2620);
			this.AddLabel(120, 67, 1160, "Tub Hue Selection");
			this.AddLabel(114, 96, 1160, "Choose your Hue");
			this.AddLabel(84, 156, 1152, "HUE = ");
			this.AddTextEntry(134, 156, 50, 20, 1359, 0, tub.Hue.ToString() );
			this.AddButton(200, 221, 238, 240, 4, GumpButtonType.Reply, 0);
			
//  Swap these comment lines if using custom Cap variable.  Then set the number in the gump display here
//  Remember to comment out ALL 3 lines, and uncomment the 1 line.
			this.AddLabel(60, 200, 1152, "* Universal Dye Tub By - Doobs. *");
//			this.AddLabel(114, 200, 1152, "* Stat totals should equal 200 *");

		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Tub.Deleted )
				return;

//	Uncomment and change the 200 to the maximum value that people can set for the 3 stats combined
//			Cap = 200;
						
            TextRelay h = info.GetTextEntry( 0 );
            try
            {
           		 hue = Convert.ToInt32(h.Text);
            }
            catch
            {
                 m_From.SendMessage("Bad hue entry. A number was expected.");
            }
			
			if ( hue > -1 )
			{
				if ( hue > 9999 ) 
				{
					m_From.SendMessage( "Your choice must be between 0 and 9999. Please try again!" );
				}	
				else
				{
					m_Tub.Hue = hue;
				}
			}
		}				
	}
}