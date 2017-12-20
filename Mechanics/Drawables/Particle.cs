using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TankProject
{
    public enum ParticleType
    {
        Rain, Explosion, Smoke, Fire,
    }
    public enum SpawnType
    {
        Square, Disc, Cube, Sphere
    }
    public delegate void UpdateFunction(GameTime gameTime);

    class Particle
    {
        internal Vector3 currentPosition;
        protected Vector3 previousPosition;
        protected Vector3 velocity;
        internal DateTime dateTime;
        protected Billboard billboard;

        //--------------------Constructors--------------------//
        internal Particle(Vector3 position, Vector3 velocity, float size)
        {
            this.currentPosition = this.previousPosition = position;
            this.velocity = velocity;
            this.dateTime = DateTime.Now;
            billboard = new Billboard(size);
        }

        internal void Update(Vector3 accelaration, GameTime gameTime)
        {
            if (velocity.Length() < 54.17f)
                velocity += accelaration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.previousPosition = currentPosition;
            this.currentPosition += velocity;
        }

        internal void Draw(GraphicsDevice device, ref BasicEffect effect, ref Camera camera)
        {
            effect.World = Matrix.CreateBillboard(currentPosition, camera.Position, Vector3.Up, Vector3.Forward);
            effect.CurrentTechnique.Passes[0].Apply();
            billboard.Draw(device);
        }
    }
    class ParticleSpawner
    {
        private SpawnType spawnType;
        private Vector3 dimensions;
        private float radius;
        private Random random;

        //--------------------Constructors--------------------//
        internal ParticleSpawner(float radius, bool hasVolume)
        {
            if (hasVolume)
                spawnType = SpawnType.Sphere;
            else
                spawnType = SpawnType.Disc;
            this.radius = radius;
            this.random = new Random();
        }
        internal ParticleSpawner(Vector3 size)
        {
            if (size.Y != 0)
                spawnType = SpawnType.Cube;
            else
                spawnType = SpawnType.Square;
            this.dimensions = size;
            this.random = new Random();
        }

        //--------------------Functions--------------------//
        internal Vector3[] GetPositions(int count, Vector3 position)
        {
            if (count <= 0)
                return null;

            Vector3[] positionArray = new Vector3[count];

            switch (spawnType)
            {
                case SpawnType.Cube:
                    for (int i = 0; i < count; i++)
                    {
                        float x = (float)random.NextDouble() * dimensions.X + (position.X - dimensions.X / 2.0f);
                        float y = (float)random.NextDouble() * dimensions.Y + (position.Y - dimensions.Y / 2.0f);
                        float z = (float)random.NextDouble() * dimensions.Z + (position.Z - dimensions.Z / 2.0f);
                        positionArray[i] = new Vector3(x, y, z);
                    }
                    break;
                case SpawnType.Square:
                    for (int i = 0; i < count; i++)
                    {
                        float x = (float)random.NextDouble() * dimensions.X + (position.X - dimensions.X / 2.0f);
                        float z = (float)random.NextDouble() * dimensions.Z + (position.Z - dimensions.Z / 2.0f);
                        positionArray[i] = new Vector3(x, 0, z);
                    }
                    break;
                case SpawnType.Sphere:
                    for (int i = 0; i < count; i++)
                    {
                        float distanceToRadius = (float)random.NextDouble() * radius;
                        float yaw = MathHelper.ToRadians((float)random.NextDouble() * 360.0f);
                        float pitch = MathHelper.ToRadians((float)random.NextDouble() * 180.0f - 90.0f);

                        Vector3 positionInsideRadius = position + Vector3.Normalize(Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0.0f))) * distanceToRadius;

                        positionArray[i] = positionInsideRadius;
                    }
                    break;
                case SpawnType.Disc:
                    for (int i = 0; i < count; i++)
                    {
                        float distanceToRadius = (float)random.NextDouble() * radius;
                        float randomAngle = MathHelper.ToRadians((float)random.NextDouble() * 360.0f);

                        positionArray[i] = position + distanceToRadius * new Vector3((float)Math.Cos(randomAngle), 0.0f, (float)Math.Sin(randomAngle));
                    }
                    break;
            }

            return positionArray;
        }
    }
    class ParticleSystem
    {
        private const float MAX_ROT = 0.05f;
        private const float EXPLOSION_FORCE = 0.04f;

        private ParticleType particleType;
        private BasicEffect effect;
        private Texture2D particleTexture;
        private ParticleSpawner spawner;
        private List<Particle> currentParticles;
        internal int particleCount;
        private int particleMax;
        private int particleMaxTime;
        private float particleSpawnRate;
        private GameTime lastParticleSpawn;
        internal Vector3 systemPosition;
        private Vector3 accelaration;
        private Random random;

        private UpdateFunction typeUpdate;

        private bool shouldSpawn;

        internal void SetShouldSpawn(bool shouldSpawn)
        {
            this.shouldSpawn = shouldSpawn;
        }
        internal bool GetShouldSpawn()
        {
            return shouldSpawn;
        }

        //--------------------Constructors--------------------//
        internal ParticleSystem(ParticleType type, Vector3 position, ParticleSpawner spawner, ContentManager content, int particleMax, int particleMaxTime, float particleSpawnRate, bool shouldStartSpawing = true)
        {
            particleType = type;
            currentParticles = new List<Particle>();
            particleCount = 0;
            this.particleMax = particleMax;
            this.particleMaxTime = particleMaxTime;
            this.particleSpawnRate = particleSpawnRate;
            lastParticleSpawn = new GameTime();

            effect = new BasicEffect(Game1.graphics.GraphicsDevice);
            effect.TextureEnabled = true;
            systemPosition = position;
            this.spawner = spawner;
            random = new Random();
            shouldSpawn = shouldStartSpawing;

            switch (type)
            {
                case ParticleType.Rain:
                    particleTexture = content.Load<Texture2D>("waterTest");
                    typeUpdate = new UpdateFunction(RainUpdate);
                    accelaration = new Vector3(0.0f, -0.98f, 0.0f);
                    break;
                case ParticleType.Explosion:
                    particleTexture = content.Load<Texture2D>("Smoke");
                    typeUpdate = new UpdateFunction(ExplosionUpdate);
                    accelaration = new Vector3(0.0f, -0.05f, 0.0f);
                    #region Add Particles
                    for (int i = 0; i < particleMax; i++)
                    {
                        Vector3 auxPosition = spawner.GetPositions(1, systemPosition)[0];
                        Vector3 direction = Vector3.Normalize(auxPosition - systemPosition) * EXPLOSION_FORCE;
                        currentParticles.Add(new Particle(auxPosition, direction, 0.1f));
                        particleCount++;
                    }
                    #endregion
                    break;
                case ParticleType.Smoke:
                    particleTexture = content.Load<Texture2D>("Smoke");
                    typeUpdate = new UpdateFunction(SmokeUpdate);
                    accelaration = new Vector3(0.0f, 0.01f, 0.0f);
                    break;
            }
        }
        
        //--------------------Functions--------------------//
        internal void Update(Vector3 position, GameTime gameTime)
        {          
            this.systemPosition = position;
          
            typeUpdate(gameTime);
        }
        private void RainUpdate(GameTime gameTime)
        {
            #region Add Particles
            while (shouldSpawn && gameTime.TotalGameTime.TotalMilliseconds - lastParticleSpawn.TotalGameTime.TotalMilliseconds - particleSpawnRate > 0)
            {
                lastParticleSpawn.TotalGameTime = lastParticleSpawn.TotalGameTime.Add(TimeSpan.FromMilliseconds(particleSpawnRate));
                if (particleCount < particleMax)
                {
                    currentParticles.Add(new Particle(spawner.GetPositions(1, systemPosition)[0], Vector3.Zero, 0.1f));
                    particleCount++;
                }
            }
            #endregion
            for (int i = particleCount - 1; i >= 0; i--)
            {
                #region Update Particles              
                currentParticles[i].Update(accelaration, gameTime);
                #endregion

                #region Kill Particles
                if ((currentParticles.Count > 0 && (DateTime.Now.TimeOfDay - currentParticles[i].dateTime.TimeOfDay).TotalMilliseconds > particleMaxTime) || currentParticles[i].currentPosition.Y <= 0)
                {
                    currentParticles.Remove(currentParticles[i]);
                    particleCount--;
                }
                #endregion

            }
        }
        private void SmokeUpdate(GameTime gameTime)
        {
            #region Add Particles
            while (shouldSpawn && gameTime.TotalGameTime.TotalMilliseconds - lastParticleSpawn.TotalGameTime.TotalMilliseconds - particleSpawnRate > 0)
            {
                lastParticleSpawn.TotalGameTime = lastParticleSpawn.TotalGameTime.Add(TimeSpan.FromMilliseconds(particleSpawnRate));
                if (particleCount < particleMax)
                {
                    currentParticles.Add(new Particle(spawner.GetPositions(1, systemPosition)[0], Vector3.Zero, 0.01f));
                    particleCount++;
                }
            }
            #endregion
            for (int i = particleCount - 1; i >= 0; i--)
            {
                #region Update Particles              
                currentParticles[i].Update(accelaration + new Vector3((float)random.NextDouble() * MAX_ROT * 2.0f - MAX_ROT, (float)random.NextDouble() * MAX_ROT * 2.0f - MAX_ROT, (float)random.NextDouble() * MAX_ROT * 2.0f - MAX_ROT), gameTime);
                #endregion

                #region Kill Particles
                if ((currentParticles.Count > 0 && (DateTime.Now.TimeOfDay - currentParticles[i].dateTime.TimeOfDay).TotalMilliseconds > particleMaxTime))
                {
                    currentParticles.Remove(currentParticles[i]);
                    particleCount--;
                }
                #endregion

            }
        }
        private void ExplosionUpdate(GameTime gameTime)
        {
          
            for (int i = particleCount - 1; i >= 0; i--)
            {
                #region Update Particles              
                currentParticles[i].Update(accelaration, gameTime);
                #endregion

                #region Kill Particles
                if ((currentParticles.Count > 0 && (DateTime.Now.TimeOfDay - currentParticles[i].dateTime.TimeOfDay).TotalMilliseconds > particleMaxTime))
                {
                    currentParticles.Remove(currentParticles[i]);
                    particleCount--;
                }
                #endregion

            }
        }
        internal void Draw(GraphicsDevice device, Camera camera)
        {
            device.BlendState = BlendState.AlphaBlend;
            device.DepthStencilState = DepthStencilState.DepthRead;

            effect.Texture = particleTexture;
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;
            foreach (Particle p in currentParticles)
                p.Draw(device, ref effect, ref camera);
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
        }
    }
}
