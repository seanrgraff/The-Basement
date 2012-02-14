using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.RabbitsVsSheep
{
	public class AnkeOfResurrection : Item
	{
		[Constructable]
		public AnkeOfResurrection() : base( 0x20BA )
		{
			Hue = 0;
			Name = "Anke of Resurrection";
			LootType = LootType.Blessed;
		}

		public AnkeOfResurrection( Serial serial ) : base( serial )
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

        public static void Initialize()
        {
            EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_Death);
        }
        
        public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage("The Anke of Resurrection will bring you back to life in 3 seconds after you die!");
		}

		public static readonly double ResurrectionDelay = 3.0;

        private static void EventSink_Death(PlayerDeathEventArgs e)
        {
            PlayerMobile owner = e.Mobile as PlayerMobile;


            if (owner != null && !owner.Deleted)
            {
                if (owner.Alive)
                    return;

                if (owner.Backpack == null || owner.Backpack.Deleted)
                    return;

                AnkeOfResurrection stone = owner.Backpack.FindItemByType(typeof(AnkeOfResurrection)) as AnkeOfResurrection;

                if (stone != null && !stone.Deleted)
                {
                    owner.SendMessage("Your Anke of Resurrection will bring you back in 3 seconds!");
                    stone.CountDown(owner);
                }
            }
        }
        
        public void CountDown(PlayerMobile owner)
        {
            Timer.DelayCall( TimeSpan.FromSeconds( ResurrectionDelay ), new TimerStateCallback( Resurrect ), new object[]{owner} );
        }
        
        
        private void Resurrect( object state )
		{
			object[] states = (object[])state;
			PlayerMobile m = (PlayerMobile)states[0];
			
			if( !m.Alive )
			{
				m.PlaySound( 0x214 );
				m.FixedEffect( 0x376A, 10, 16 );
				m.SendMessage("Your Anke of Resurrection glows with healing power!");
				m.Resurrect();
				m.Hits = m.HitsMax;
				m.Mana = m.ManaMax;
				m.Stam = m.StamMax;
				this.Delete();
			}
		}
	}
}