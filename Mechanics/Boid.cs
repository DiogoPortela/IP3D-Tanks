using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankProject
{
    class Boid : GameObject
    {
        private const float MAX_VELOCITY = 0.02f;
        private Vector3 seekVelocity;
        private Vector3 acceleration;

        List<Boid> neighboardhood;

        internal Boid(Vector3 position, Vector3 rotation) : base(position, rotation, Vector3.Zero)
        {
            this.position = position;
            acceleration = velocity = seekVelocity = Vector3.Zero;
        }

        internal virtual void Update(Vector3 targetPosition, GameTime deltaTime)
        {
            if ((targetPosition - position).Length() > 1.0f)
            {
                if (velocity == Vector3.Zero)
                    velocity = relativeForward * 0.01f;
                seekVelocity = Vector3.Normalize(targetPosition - position) * MAX_VELOCITY;
                acceleration = seekVelocity - velocity;

                velocity += acceleration * (float)deltaTime.ElapsedGameTime.TotalSeconds;
                if (velocity.Length() > MAX_VELOCITY)
                    velocity = Vector3.Normalize(velocity) * MAX_VELOCITY;

                relativeForward = Vector3.Normalize(velocity);
                relativeRight = Vector3.Cross(relativeForward, Vector3.Up);
            }
            else
            {
                acceleration = Vector3.Zero;
                velocity = Vector3.Zero;
            }
            this.position += velocity;
        }
    }
}
