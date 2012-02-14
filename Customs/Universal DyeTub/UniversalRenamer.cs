using System;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{

     public class UnivPenTarget : Target
     {
            private UniversalRenamer m_Item;

            public UnivPenTarget( Item item ) : base( 12, false, TargetFlags.None )
            {
                   m_Item = (UniversalRenamer) item;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
			
						if ( targeted is UniversalRenamer )
						{
							from.SendGump( new UniversalRenamerGump( (PlayerMobile)from, (UniversalRenamer)targeted ));
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
                                 targ.Name=m_Item.penName;
                                 from.PlaySound( 0x23F );
                             }
                        }

            }
     }


     public class UniversalRenamer : Item
     {

            private bool m_Redyable;
			public String penName = "UoLamers Crybrid";

            [Constructable]
            public UniversalRenamer() : base( 0x0FBF )
            {
                   Weight = 0.0;
                   Hue = 0;
                   Name = "Universal Renamer";
                   m_Redyable = false;
            }

            public UniversalRenamer( Serial serial ) : base( serial )
            {
            }

            public override void OnDoubleClick( Mobile from )
            {

                   from.Target = new UnivPenTarget( this );
                   from.SendMessage( "What do you wish to rename? Select this pen for menu." );

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
	 
	public class UniversalRenamerGump : Gump
	{
		private PlayerMobile m_From;
		private UniversalRenamer m_Pen;
		
		private String name;
				
		public UniversalRenamerGump( PlayerMobile from, UniversalRenamer pen ): base( 50, 50 )
		{
			m_From = from;
			m_Pen = pen;
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(50, 50, 250, 215, 2620);
			this.AddLabel(114, 67, 1160, "Item Name Selection");
			//this.AddLabel(114, 96, 1160, "Choose your Name");
			this.AddLabel(84, 156, 1152, "NAME = ");
			this.AddTextEntry(134, 156, 150, 20, 1359, 0, m_Pen.penName );
			this.AddButton(200, 221, 238, 240, 4, GumpButtonType.Reply, 0);
			
			this.AddLabel(60, 200, 1152, "* Universal Renamer By - Doobs. *");

		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Pen.Deleted )
				return;
						
            TextRelay n = info.GetTextEntry( 0 );
			try
            {
           		 name = Convert.ToString(n.Text);
            }
            catch
            {
                 m_From.SendMessage("Bad name entry.");
            }
			
			m_Pen.penName = name;
			
			m_From.Target = new UnivPenTarget( m_Pen );
			m_From.SendMessage( "What do you wish to rename?" );
			
		}				
	}
}