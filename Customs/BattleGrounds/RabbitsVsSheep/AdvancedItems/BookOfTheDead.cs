using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.RabbitsVsSheep
{
	public class BookOfTheDead : Item
	{
		[Constructable]
		public BookOfTheDead() : base( 0x2253 )
		{
			Name = "Book of the Dead";
			Weight = 1.0;
		}
		
		public override void OnDoubleClick( Mobile from )
		{	
		
			if ( IsChildOf( from.Backpack ) )
			{
				Map map = from.Map;
				IPoint3D p = from.Location;

				if ( map != null )
				{
					IPooledEnumerable eable = map.GetObjectsInRange( new Point3D( p ), 5 );
					int j = 0;
					foreach ( Object m in eable )
					{
						if ( m is Corpse )
						{
							Corpse n = (Corpse)m;
							if ( from != n.Owner && j <= 7 && n.Owner != null && n.Channeled == false )
							{
								AnimateCorpse( n, from );
								//from.SendMessage("RESSING: {0}", n.Name );
								j++;
							}
						}
					}
				//from.SendMessage("DONE");
				eable.Free();
				}	
				from.SendMessage("The book's brittle pages crumble in your grasp!");
				this.Delete();
			}
			else
			{	
				from.SendMessage("The book must be in your backpack!");
			}
		}
		
		public void AnimateCorpse( Corpse corp, Mobile caster )
		{
			if ( corp != null )
			{
				Corpse c = corp;
				
				Point3D p = c.GetWorldLocation();
				Map map = c.Map;

				if ( map != null )
				{
					Effects.PlaySound( p, map, 0x1FB );
					Effects.SendLocationParticles( EffectItem.Create( p, map, EffectItem.DefaultDuration ), 0x3789, 1, 40, 0x3F, 3, 9907, 0 );

					Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( SummonDelay_Callback ), new object[]{ caster, c, p, map } );
				}
			}
		}
		
		private static void SummonDelay_Callback( object state )
		{
			object[] states = (object[])state;

			Mobile caster = (Mobile)states[0];
			Corpse corpse = (Corpse)states[1];
			Point3D loc = (Point3D)states[2];
			Map map = (Map)states[3];

			if ( corpse.ItemID != 0x2006 )
			{
				corpse.Channeled = true;
				return;
			}

			Mobile owner = corpse.Owner;

			if ( owner == null )
			{
				corpse.Channeled = true;
				return;
			}
			
			//Make toSummon some random undead monster!
			Type toSummon = null;
			
			int ChanceWheel = Utility.Random( 100 );
			
			if ( ChanceWheel > 80 )
			{
				toSummon = typeof( Lich );
			}
			else if ( ChanceWheel > 70 )
			{
				toSummon = typeof( SkeletalKnight );
			}
			else if ( ChanceWheel > 60 )
			{
				toSummon = typeof( SkeletalMage );
			}
			else if ( ChanceWheel > 20 )
			{
				toSummon = typeof( Skeleton );
			}
			else
			{
				toSummon = typeof( Zombie );
			}

			if ( toSummon == null )
				return;

			Mobile summoned = null;

			try{ summoned = Activator.CreateInstance( toSummon ) as Mobile; }
			catch{}

			if ( summoned == null )
				return;

			if ( summoned is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)summoned;

				// to be sure
				bc.Tamable = false;

				if ( bc is BaseMount )
					bc.ControlSlots = 1;
				else
					bc.ControlSlots = 0;

				Effects.PlaySound( loc, map, bc.GetAngerSound() );

				BaseCreature.Summon( (BaseCreature)summoned, false, caster, loc, 0x28, TimeSpan.FromDays( 1.0 ) );
			}

			summoned.Fame = 0;
			summoned.Karma = -1500;

			summoned.MoveToWorld( loc, map );
			
			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ), new TimerStateCallback( Summoned_Damage ), summoned );

			corpse.ProcessDelta();
			corpse.SendRemovePacket();
			corpse.ItemID = Utility.Random( 0xECA, 9 ); // bone graphic
			corpse.Hue = 132;
			corpse.Channeled = true;
			corpse.ProcessDelta();
		}
		
		private static void Summoned_Damage( object state )
		{
			Mobile mob = (Mobile)state;

			if ( mob.Hits > 0 )
				--mob.Hits;
			else
				mob.Kill();
		}

		public BookOfTheDead( Serial serial ) : base( serial )
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