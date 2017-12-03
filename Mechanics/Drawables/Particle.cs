using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TankProject
{
    public enum ParticleType
    {
        rain, explosion, smoke, fire,
    }
    public delegate void drawFunction();

    class Particle
    {
        Vector3 currentPosition;
        Vector3 previousPosition;
        Vector3 velocity;
        DateTime dateTime;
        Square drawingSquare;

        internal Particle(Vector3 position, Vector3 velocity, Texture2D texture)
        {
            this.currentPosition = this.previousPosition = position;
            this.velocity = velocity;
            this.dateTime = DateTime.Now;
            drawingSquare = new Square(position, texture, 10.0f);
        }

        internal void Update(Vector3 accelaration, GameTime gameTime)
        {
            if (velocity.Length() < 54.17f)
                velocity += accelaration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.previousPosition = currentPosition;
            this.currentPosition += velocity;
            drawingSquare.Update(currentPosition);
        }

        internal void Draw(GraphicsDevice device)
        {
            drawingSquare.Draw(device);
        }
    }

    class ParticleSystem
    {
        ParticleType particleType;
        BasicEffect effect;
        List<Particle> currentParticles;
        Texture2D particleTexture;
        Vector3 systemPosition;

        internal ParticleSystem(ParticleType type, Vector3 position, ContentManager content)
        {
            particleType = type;
            currentParticles = new List<Particle>();
            effect = new BasicEffect(Game1.graphics.GraphicsDevice);
            effect.TextureEnabled = true;
            systemPosition = position;

            switch (type)
            {
                case ParticleType.rain:
                    particleTexture = content.Load<Texture2D>("waterTest");
                    currentParticles.Add(new Particle(systemPosition, Vector3.Zero, particleTexture));
                    break;
                case ParticleType.explosion:
                    break;
                case ParticleType.smoke:
                    break;
            }
        }

        internal void Update(GameTime gameTime)
        {
            Vector3 acc = new Vector3();
            foreach (Particle p in currentParticles)
                p.Update(acc, gameTime);
        }

        internal void Draw(GraphicsDevice device, Camera camera)
        {
            effect.Texture = particleTexture;
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;
            effect.World = Matrix.CreateBillboard(Vector3.Zero, camera.Position, Vector3.Up, camera.Forward);
            effect.CurrentTechnique.Passes[0].Apply();
            foreach (Particle p in currentParticles)
                p.Draw(device);
        }
    }
}
