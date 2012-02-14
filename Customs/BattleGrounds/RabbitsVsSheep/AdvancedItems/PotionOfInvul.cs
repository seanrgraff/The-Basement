using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.RabbitsVsSheep
{
	public class PotionOfInvul : Item
	{
		[Constructable]
		public PotionOfInvul() : base( 0xF06 )
		{
			Hue = 2066;
			Name = "Potion of Invulnerability";
		}

		public PotionOfInvul( Serial serial ) : base( serial )
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
			if ( from.Blessed != true )
			{
				if ( IsChildOf( from.Backpack ) )
				{
					new InvulTimer(from).Start();
	
					from.FixedParticles( 0x376A, 9, 32, 5007, EffectLayer.Waist );
					from.PlaySound( 0x1E3 );
	
					BasePotion.PlayDrinkEffect( from );
	
					this.Consume();
				}
				else
				{
					from.SendMessage("The Potion must be in your backpack.");
				}
			}
			else
			{
				from.SendMessage( "You are already invulnerable." );
			}
		}
	}
	
	public class InvulTimer : Timer
    {
        public Mobile m_From;
        public int count = 0;

        public InvulTimer(Mobile from)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
        {
            Priority = TimerPriority.FiftyMS;
            m_From = from;
            m_From.Blessed = true;
        }

        protected override void OnTick()
        {
        	count++;
            // heals the player
            if ( count >= 30 )
            {
            	m_From.FixedParticles( 0x376A, 9, 32, 5007, EffectLayer.Waist );
            	m_From.Blessed = false;
            	this.Stop();
            }
            else if ( m_From.Deleted )
            {
            	this.Stop();
            }
        }
    }
}