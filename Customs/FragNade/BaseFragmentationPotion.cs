using System;
using System.Collections;
using Server.Mobiles;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Items
{
	public abstract class BaseFragmentationPotion : BasePotion
	{
		public abstract int MinDamage { get; }
		public abstract int MaxDamage { get; }

		public override bool RequireFreeHand{ get{ return false; } }

		private static bool LeveledExplosion = false; // Should explosion potions explode other nearby potions?
		private static bool InstantExplosion = false; // Should explosion potions explode on impact?
		private const int   ExplosionRange   = 2;     // How long is the blast radius?

		public BaseFragmentationPotion( PotionEffect effect ) : base( 0xF0D, effect )
		{
		}

		public BaseFragmentationPotion( Serial serial ) : base( serial )
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

		public virtual object FindParent( Mobile from )
		{
			Mobile m = this.HeldBy;

			if ( m != null && m.Holding == this )
				return m;

			object obj = this.RootParent;

			if ( obj != null )
				return obj;

			if ( Map == Map.Internal )
				return from;

			return this;
		}

		private Timer m_Timer;

		private ArrayList m_Users;

		public override void Drink( Mobile from )
		{

			int Delay = 5; // <- Added this line to set the delay between use
			if ( from.BeginAction( typeof( BaseFragmentationPotion ) ) ) // <- Added this line and set it to BaseFragmentationPotion
			{


			if ( Core.AOS && (from.Paralyzed || from.Frozen || (from.Spell != null && from.Spell.IsCasting)) )
			{
				from.SendLocalizedMessage( 1062725 ); // You can not use a purple potion while paralyzed.
				return;
			}

			ThrowTarget targ = from.Target as ThrowTarget;

			if ( targ != null && targ.Potion == this )
				return;

			from.RevealingAction();

			if ( m_Users == null )
				m_Users = new ArrayList();

			if ( !m_Users.Contains( from ) )
				m_Users.Add( from );

			from.Target = new ThrowTarget( this );

			Timer.DelayCall( TimeSpan.FromSeconds( Delay ), new TimerStateCallback( ReleaseExploderLock ), from ); // <- Added this line to set the timer for allowing you to throw another

			if ( m_Timer == null )
			{
				from.SendLocalizedMessage( 500236 ); // You should throw it now!
				m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 0.75 ), TimeSpan.FromSeconds( 1.0 ), 5, new TimerStateCallback( Detonate_OnTick ), new object[]{ from, Utility.RandomMinMax(3, 4) } );
			}
			}
			else
			{ 
                		from.SendMessage("You must wait a few seconds before using another explosion potion."); 
			}
					
		}

		private static void ReleaseExploderLock( object state ) // <- Added this as well! Hooray!
		{
			((Mobile)state).EndAction( typeof( BaseFragmentationPotion ) );
		}

		private void Detonate_OnTick( object state )
		{
			if ( Deleted )
				return;

			object[] states = (object[])state;
			PlayerMobile from = (PlayerMobile)states[0];
			int timer = (int)states[1];

			object parent = FindParent( from );

			if ( timer == 0 )
			{
				Point3D loc;
				Map map;

				if ( parent is Item )
				{
					Item item = (Item)parent;

					loc = item.GetWorldLocation();
					map = item.Map;
				}
				else if ( parent is Mobile )
				{
					Mobile m = (Mobile)parent;

					loc = m.Location;
					map = m.Map;
				}
				else
				{
					return;
				}
				
				Point3D startLoc = new Point3D( loc.X, loc.Y, loc.Z );

				Explode( from, true, loc, map );
				DoLaunch( parent, from, loc, startLoc, map);
			}
			else
			{
				if ( parent is Item )
					((Item)parent).PublicOverheadMessage( MessageType.Regular, 0x22, false, timer.ToString() );
				else if ( parent is Mobile )
					((Mobile)parent).PublicOverheadMessage( MessageType.Regular, 0x22, false, timer.ToString() );

				states[1] = timer - 1;
			}
		}

		private void Reposition_OnTick( object state )
		{
			if ( Deleted )
				return;

			object[] states = (object[])state;
			Mobile from = (Mobile)states[0];
			IPoint3D p = (IPoint3D)states[1];
			Map map = (Map)states[2];

			Point3D loc = new Point3D( p );

			if ( InstantExplosion )
				Explode( from, true, loc, map );
			else
				MoveToWorld( loc, map );
		}

		private class ThrowTarget : Target
		{
			private BaseFragmentationPotion m_Potion;

			public BaseFragmentationPotion Potion
			{
				get{ return m_Potion; }
			}

			public ThrowTarget( BaseFragmentationPotion potion ) : base( 12, true, TargetFlags.None )
			{
				m_Potion = potion;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Potion.Deleted || m_Potion.Map == Map.Internal )
					return;

				IPoint3D p = targeted as IPoint3D;

				if ( p == null )
					return;

				Map map = from.Map;

				if ( map == null )
					return;

				SpellHelper.GetSurfaceTop( ref p );

				from.RevealingAction();

				IEntity to;

				if ( p is Mobile )
					to = (Mobile)p;
				else
					to = new Entity( Serial.Zero, new Point3D( p ), map );

				Effects.SendMovingEffect( from, to, m_Potion.ItemID & 0x3FFF, 7, 0, false, false, m_Potion.Hue, 0 );

				if( m_Potion.Amount > 1 )
				{
					Mobile.LiftItemDupe( m_Potion, 1 );
				}

				m_Potion.Internalize();
				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( m_Potion.Reposition_OnTick ), new object[]{ from, p, map } );
			}
			protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
        	{
				from.SendMessage("You cancelled the throw.");
				Timer.DelayCall( TimeSpan.FromSeconds( 0 ), new TimerStateCallback( ReleaseExploderLock ), from );
        	}
		}

		public void Explode( Mobile from, bool direct, Point3D loc, Map map )
		{
			if ( Deleted )
				return;

			Consume();

			for ( int i = 0; m_Users != null && i < m_Users.Count; ++i )
			{
				Mobile m = (Mobile)m_Users[i];
				ThrowTarget targ = m.Target as ThrowTarget;

				if ( targ != null && targ.Potion == this )
					Target.Cancel( m );
			}

			if ( map == null )
				return;

			Effects.PlaySound( loc, map, 0x207 );
			Effects.SendLocationEffect( loc, map, 0x36BD, 20 );

			int alchemyBonus = 0;

			if ( direct )
				alchemyBonus = (int)(from.Skills.Alchemy.Value / (Core.AOS ? 5 : 10));

			IPooledEnumerable eable = LeveledExplosion ? map.GetObjectsInRange( loc, ExplosionRange ) : map.GetMobilesInRange( loc, ExplosionRange );
			ArrayList toExplode = new ArrayList();

			int toDamage = 0;

			foreach ( object o in eable )
			{
				if ( o is Mobile )
				{
					toExplode.Add( o );
					++toDamage;
				}
				else if ( o is BaseFragmentationPotion && o != this )
				{
					toExplode.Add( o );
				}
			}

			eable.Free();

			int min = Scale( from, MinDamage );
			int max = Scale( from, MaxDamage );

			for ( int i = 0; i < toExplode.Count; ++i )
			{
				object o = toExplode[i];

				if ( o is Mobile )
				{
					Mobile m = (Mobile)o;

					if ( from == null || (SpellHelper.ValidIndirectTarget( from, m ) && from.CanBeHarmful( m, false )) )
					{
						if ( from != null )
							from.DoHarmful( m );

						int damage = Utility.RandomMinMax( min, max );

						damage += alchemyBonus;

						if ( !Core.AOS && damage > 40 )
							damage = 40;
						else if ( Core.AOS && toDamage > 2 )
							damage /= toDamage - 1;

						AOS.Damage( m, from, damage, 0, 100, 0, 0, 0 );
					}
				}
				else if ( o is BaseFragmentationPotion )
				{
					BaseFragmentationPotion pot = (BaseFragmentationPotion)o;

					pot.Explode( from, false, pot.GetWorldLocation(), pot.Map );
				}
			}
		}
		
		private static void DoLaunch( object killed, PlayerMobile from, Point3D OurLoc, Point3D StartLoc, Map map )
		{

			//ONE//
			Point3D endHeadLoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -6, 6 ), StartLoc.Y + Utility.RandomMinMax( -6, 6 ), StartLoc.Z );
			Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endHeadLoc, map ),
						0xF0D, 5, 0, false, false );
			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endHeadLoc, map, killed } );
			//END ONE//

			//TWO//
			Point3D endTorsoLoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -6, 6 ), StartLoc.Y + Utility.RandomMinMax( -6, 6 ), StartLoc.Z );
			Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endTorsoLoc, map ),
						0xF0D, 5, 0, false, false );
			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endTorsoLoc, map, killed } );
			//END TWO//
		
			//THREE//
			Point3D endLLLoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -6, 6 ), StartLoc.Y + Utility.RandomMinMax( -6, 6 ), StartLoc.Z );
			Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endLLLoc, map ),
						0xF0D, 5, 0, false, false );
			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endLLLoc, map, killed } );
			//END THREE//

			//FOUR//
			Point3D endRLLoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -6, 6 ), StartLoc.Y + Utility.RandomMinMax( -6, 6 ), StartLoc.Z );
			Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endRLLoc, map ),
						0xF0D, 5, 0, false, false );
			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endRLLoc, map, killed } );
			//END FOUR//

			//FIVE//
			Point3D endLALoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -6, 6 ), StartLoc.Y + Utility.RandomMinMax( -6, 6 ), StartLoc.Z );
			Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endLALoc, map ),
						0xF0D, 5, 0, false, false );
			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endLALoc, map, killed } );
			//END FIVE//

			//SIX//
			Point3D endRALoc = new Point3D( StartLoc.X + Utility.RandomMinMax( -6, 6 ), StartLoc.Y + Utility.RandomMinMax( -6, 6 ), StartLoc.Z );
			Effects.SendMovingEffect( new Entity( Serial.Zero, StartLoc, map ), new Entity( Serial.Zero, endRALoc, map ),
						0xF0D, 5, 0, false, false );
			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerStateCallback( FinishLaunch ), new object[]{ from, endRALoc, map, killed } );
			//END SIX//

		}

		private static void FinishLaunch( object state )
		{
			object[] states = (object[])state;

			Mobile from = (Mobile)states[0];
			Point3D endHeadLoc = (Point3D)states[1];
			Map map = (Map)states[2];

			if ( map == null || map == Map.Internal )
					return;

			ExplosionPotion Epot = new ExplosionPotion();
			Epot.Movable = true;
			Epot.Name = "Fragmentation Potion";
			Epot.Hue = 1167;
			Epot.MoveToWorld( endHeadLoc, map );
			Epot.Explode(from, false, Epot.GetWorldLocation(), Epot.Map); 
		}
		
	}
}