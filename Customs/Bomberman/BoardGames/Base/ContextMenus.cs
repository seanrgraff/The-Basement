using System;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Dueling;

namespace Solaris.BoardGames
{
	
	public class ViewBoardGameScoresEntry : ContextMenuEntry
	{
		Mobile _From = null;
		BoardGameControlItem _ControlItem = null;

		//3006239 = "View events"
		public ViewBoardGameScoresEntry( Mobile from, BoardGameControlItem controlitem, int index ) : base( 6239, index )
		{
			_From = from;
			_ControlItem = controlitem;
		}

		public override void OnClick()
		{
			if ( _ControlItem == null || _ControlItem.Deleted )
			{
				return;
			}
			
			_From.SendGump( new BoardGameScoresGump( _From, _ControlItem ) );
		}
	}
	
	public class ResetBoardGameScoresEntry : ContextMenuEntry
	{
		Mobile _From = null;
		String _GameName = null;
		BoardGameControlItem _ControlItem = null;

		//3006162 = "Reset Game"
		public ResetBoardGameScoresEntry( Mobile from, BoardGameControlItem controlitem, int index ) : base( 6162, index )
		{
			_From = from;
			_ControlItem = controlitem;
			_GameName = null;
		}
		
		//public ResetBoardGameScoresEntry( Mobile from, String gameName, int index ) : base( 6162, index )
		//{
		//	_From = from;
		//	_GameName = gameName;
		//	_ControlItem = null;
		//}

		public override void OnClick()
		{
			if ( _ControlItem != null && !_ControlItem.Deleted ) 
			{
				_From.SendGump( new ConfirmResetGameScoreGump( _From, _ControlItem ) );
			}
			//else if ( _GameName != null )
			//{
			//	_From.SendGump( new ConfirmResetGameScoreGump( _From, _GameName ) ); 
			//}
			else
			{
				return;
			}
		}
	}
	
}