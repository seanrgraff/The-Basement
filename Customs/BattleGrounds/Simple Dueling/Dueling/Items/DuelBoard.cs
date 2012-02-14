using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Solaris.BoardGames;

namespace Server.Dueling 
{

	[Flipable( 0x1E5E, 0x1E5F )]
	public class DuelBoard : BoardGameControlItem //BaseBulletinBoard
	{
		public override string GameName{ get{ return "Dueling"; } }
	
		[Constructable]
		public DuelBoard()
		{
			ItemID=0x1E5E;
			Name = "Duel Board";
		}

		public DuelBoard( Serial serial ) : base( serial )
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
	
		public override void OnDoubleClick( Mobile from )
		{
			if ( CheckRange( from ) )
			{
				from.SendGump( new BoardGameScoresGump( from, this ) );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
		}
		
		public override void UpdatePosition()
		{
			//GameZone = new Rectangle3D(new Point3D( this.X, this.Y, this.Z ), new Point3D( this.X, this.Y, this.Z ));
			
			//base.UpdatePosition();
			return;
		}
		
		public bool CheckRange( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
				return true;

			return ( from.Map == this.Map && from.InRange( GetWorldLocation(), 2 ) );
		}
		
		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			
			if( from.AccessLevel >= AccessLevel.GameMaster )
			{
				list.Add( new ResetBoardGameScoresEntry( from, this, 1 ) );
				
			}
		}

	}
}