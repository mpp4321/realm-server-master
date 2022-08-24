using RotMG.Common;
using RotMG.Game.Entities;
using RotMG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RotMG.Game.Logic.Behaviors
{
    class BuzzBehavior : Behavior
    {

		private float speed_;
		private float acceleration_;
		private float turnRate_;
		private float acquireRange_;
		private float cooldown_;


		private Vector2 target_ = new Vector2(0, 0);

		public BuzzBehavior(float speed = 1.50f, float acceleration = .50f, float turnRate = (float) Math.PI / 2, float acquireRange = 20.0f, float cooldown = 2.0f)
		{
			speed_ = speed;
			acceleration_ = acceleration;
			turnRate_ = turnRate;
			acquireRange_ = acquireRange;
			cooldown_ = cooldown;
		}

		class Momentum
		{
			public float speed_;
			//Direction vector
			public float facing_;
            public bool hasTarget_ = false;
            public bool locked_;
            public bool buzzed_;
            public float tilFindTarget_ = 0.0f;
		}

		public override void Enter(Entity host)
		{
			host.StateObject[Id] = new Momentum()
			{
				speed_ = this.speed_
			};
		}

		public override bool Tick(Entity host)
		{
			float dt = Settings.MillisecondsPerTick;
			Momentum m = host.StateObject[Id] as Momentum;

			if (!m.hasTarget_)
			{
				m.tilFindTarget_ -= dt;
				if (m.tilFindTarget_ < 0)
				{
					// Find a target
					Player o = GameUtils.GetNearestPlayer(host, acquireRange_) as Player;
					if (o == null)
					{
						m.tilFindTarget_ = cooldown_;
						return false;
					}

					// Yay found a target
					target_ = o.Position;
					m.hasTarget_ = true;
					m.locked_ = false;
					m.buzzed_ = false;
				}
				else return false;
			}

			if (!m.hasTarget_) return false;

			if (!m.locked_)
			{
				m.speed_ -= acceleration_ * Settings.SecondsPerTick;  // Slow down until we get locked
				if (m.speed_ < 0) m.speed_ = 0;
			}
			else
			{
				// Accelerate to ramming speed
				if (m.speed_ < speed_)
				{
					m.speed_ += acceleration_ * Settings.SecondsPerTick;
					if (m.speed_ > speed_) m.speed_ = speed_;
				}
				else if (m.speed_ > speed_)
				{    // BUGBUG THIS IS A BUG < or > ?
					m.speed_ -= acceleration_ * Settings.SecondsPerTick;
					if (m.speed_ < speed_) m.speed_ = speed_;
				}
			}

			// Find arc to target

			double angleBetween = host.Position.Angle(target_);
			double arc = MathUtils.AngleDifference(m.facing_, angleBetween);

			if (m.locked_ && Math.Abs(angleBetween) > Math.PI / 2) m.buzzed_ = true; // target behind us

			if (!m.locked_)
			{
				// Adjust facing
				double maxTurn = turnRate_ * Settings.SecondsPerTick;
				double deltaArc = arc;
				if (Math.Abs(deltaArc) > maxTurn)
				{
					deltaArc = (deltaArc < 0) ? -maxTurn : maxTurn;
				}
				var absArc = Math.Abs(arc);
				if (absArc < Math.PI / 50 ||
					host.Position.DistanceSquared(target_) < 1)
                        m.locked_ = true; // good enough; lock
				else m.facing_ = m.facing_ + (float) deltaArc;                     // keep steering
			}
			Vector2 movementVec = new Vector2(m.facing_);
			movementVec.Normalize();
            float dist = host.GetSpeed(m.speed_) * Settings.SecondsPerTick;

			host.ValidateAndMove(movementVec * dist + host.Position);

			double BUZZRANGE = acquireRange_ / 3;

			if (host.Position.DistanceSquared(target_) > BUZZRANGE * BUZZRANGE)
			{
				// Buzz run is done
				m.hasTarget_ = false;
				m.tilFindTarget_ = cooldown_;
			}

			host.StateObject[Id] = m;

			return true;
		}

	}
}
