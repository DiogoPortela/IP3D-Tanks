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
        private const float MAX_ROTATION = 2.0f;
        private Vector3 seekVelocity;
        private Vector3 acceleration;

        private List<Boid> neighboardhood;
        private List<float> neighboardhoodDistances;
        private List<Vector3> neighboardhoodDistanceNormalized;

        private GameStage game;

        internal Boid(Vector3 position, Vector3 rotation, GameStage game) : base(position, rotation, Vector3.Zero, 100)
        {
            this.position = position;
            acceleration = velocity = seekVelocity = Vector3.Zero;
            this.game = game;
        }

        internal virtual void Update(Vector3 targetPosition, GameTime deltaTime)
        {
            neighboardhood = new List<Boid>();
            neighboardhoodDistances = new List<float>();
            neighboardhoodDistanceNormalized = new List<Vector3>();
            foreach (Boid b in game.enemyList)
            {
                float d = (this.position - b.position).Length();
                if (d < 3.0f)
                {
                    neighboardhood.Add(b);
                    neighboardhoodDistances.Add(d);
                    neighboardhoodDistanceNormalized.Add(Vector3.Normalize(this.position - b.position));
                }
            }

            if ((targetPosition - position).Length() > 0.5f)
            {
                if (velocity == Vector3.Zero)
                    velocity = relativeForward * 0.01f;
                seekVelocity = Vector3.Normalize(targetPosition - position) * MAX_VELOCITY;
                acceleration = seekVelocity - velocity;

                double cosSteeringAngle;
                double angle;
                if (Vector3.Dot(Right, acceleration) < 0)
                {
                    cosSteeringAngle = Vector3.Dot(Vector3.Normalize(velocity), Vector3.Normalize(acceleration));
                    angle = Math.Acos(cosSteeringAngle);
                }
                else
                {
                    cosSteeringAngle = Vector3.Dot(Vector3.Normalize(velocity), Vector3.Normalize(acceleration));
                    angle = -Math.Acos(cosSteeringAngle);
                }


                if (angle > MAX_ROTATION)
                {
                    Matrix angleRotation = Matrix.CreateFromAxisAngle(Up, MAX_ROTATION - (float)angle);
                    acceleration = Vector3.Transform(acceleration, angleRotation);
                }
                else if (angle < -MAX_ROTATION)
                {
                    Matrix angleRotation = Matrix.CreateFromAxisAngle(Up, -(float)angle - MAX_ROTATION);
                    acceleration = Vector3.Transform(acceleration, angleRotation);
                }

                ///
                Vector3 repulsionAcceleration = Vector3.Zero;
                for(int i = 0; i < neighboardhood.Count; i++)
                {
                    if(!((1.0f / neighboardhoodDistances[i]) > float.MaxValue))
                        repulsionAcceleration *= neighboardhoodDistanceNormalized[i] * (1.0f / neighboardhoodDistances[i]);

                }
                ///

                acceleration = (acceleration + repulsionAcceleration) / 2;

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
