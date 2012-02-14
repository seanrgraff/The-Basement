/*
 Army System v1.0 Beta
 By: Shadow-Sigma
 
 If you have any questions or concerns, please leave me a private message on the RunUO forums (Username: Shadow-Sigma), or send me an e-mail at intranetworkster@gmail.com
 
 Enjoy!
 
 Please do not remove this comment from these scripts, thank you! :)
 */
using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
	public class ArmyBase : BaseCreature
	{
		public Timer	m_Timer;

		[Constructable]
		public ArmyBase(int Team, AIType iAI, FightMode iFightMode, int iRangePerception, int iRangeFight, double dActiveSpeed, double dPassiveSpeed) 
            : base(iAI, iFightMode, iRangePerception, iRangeFight, dActiveSpeed, dPassiveSpeed)
		{
			this.Body = 400 + Utility.Random(2);
			this.Hue = Utility.RandomSkinHue();
            this.Team = Team;

			this.Skills[SkillName.DetectHidden].Base = 100;
            this.Skills[SkillName.MagicResist].Base = 120;

			int iHue = 20 + Team * 40;
			int jHue = 25 + Team * 40;

			Item hair = new Item( Utility.RandomList( 0x203C, 0x203B, 0x203C, 0x203D ) );
			hair.Hue = iHue;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem( hair );

			LeatherGloves glv = new LeatherGloves();
			glv.Hue = iHue;
			glv.LootType = LootType.Newbied;
			AddItem(glv);

			Container pack = new Backpack();

			pack.Movable = false;

			AddItem( pack );

			m_Timer = new AutokillTimer(this);
			m_Timer.Start();
		}

		public ArmyBase( Serial serial ) : base( serial )
		{
			m_Timer = new AutokillTimer(this);
			m_Timer.Start();
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

		public override bool HandlesOnSpeech( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
				return true;

			return base.HandlesOnSpeech( from );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			base.OnSpeech( e );

			if (e.Mobile.AccessLevel >= AccessLevel.GameMaster)
			{
				if (e.Speech == "Die.")
				{
					m_Timer.Stop();
					m_Timer.Delay = TimeSpan.FromSeconds( 1 /*Utility.Random(1, 5)*/ );
					m_Timer.Start();
				}
			}
		}

		public override void OnTeamChange()
		{
			int iHue = 20 + Team * 40;
			int jHue = 25 + Team * 40;

			Item item = FindItemOnLayer( Layer.OuterTorso );

			if ( item != null )
				item.Hue = jHue;

			item = FindItemOnLayer( Layer.Helm );

			if ( item != null )
				item.Hue = iHue;

			item = FindItemOnLayer( Layer.Gloves );

			if ( item != null )
				item.Hue = iHue;

			item = FindItemOnLayer( Layer.Shoes );

			if ( item != null )
				item.Hue = iHue;

			item = FindItemOnLayer( Layer.Hair );

			if ( item != null )
				item.Hue = iHue;

			item = FindItemOnLayer( Layer.MiddleTorso );

			if ( item != null )
				item.Hue = iHue;

			item = FindItemOnLayer( Layer.OuterLegs );

			if ( item != null )
				item.Hue = iHue;
		}

		private class AutokillTimer : Timer
		{
			private ArmyBase m_Owner;

			public AutokillTimer( ArmyBase owner ) : base( TimeSpan.FromMinutes(25.0) )
			{
				m_Owner = owner;
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				m_Owner.Kill();
				Stop();
			}
		}
	}
}
