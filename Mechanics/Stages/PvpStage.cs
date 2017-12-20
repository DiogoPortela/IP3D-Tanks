using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace TankProject
{
    class PvpStage : Stage
    {
        private Camera currentCameraPlayerOne;
        private Camera currentCameraPlayerTwo;
        private Viewport defaultView, upView, downView;
        internal Player playerOne, playerTwo;
        internal List<Player> playerList;
        internal List<ParticleSystem> particleSystemList;
        internal ParticleSystem playerOneRain;
        internal ParticleSystem playerTwoRain;

        private SoundEffectInstance engineSoundFX;
        private SoundEffectInstance rainSoundFX;
        private SoundEffect explosionSoundFX;

        internal static Light currentLight;

        internal PvpStage(Game1 game1) : base(game1)
        {
            //Load Viewports
            defaultView = Game1.graphics.GraphicsDevice.Viewport;
            upView = downView = defaultView;
            upView.Height /= 2;
            downView.Height /= 2;
            downView.Y = upView.Height;

            //Load stuff
            currentLight = new Light(-Vector3.One, new Color(new Vector3(0.5f, 0.5f, 0.5f)), new Color(new Vector3(0.1f, 0.1f, 0.1f)));
            Bullet.LoadModel(game1.Content, Material.White, currentLight);
            Skybox.Load(game1.Content);

            //Load Players
            playerList = new List<Player>();
            playerOne = new Player(new Vector3(50, 1, 64), new Vector3(0, MathHelper.TwoPi - MathHelper.PiOver2, 0), Vector3.Zero, 0.0005f, PlayerIndex.One);
            playerOne.LoadModelBones(game1.Content, Material.White, currentLight);
            playerList.Add(playerOne);
            playerTwo = new Player(new Vector3(70, 1, 64), new Vector3(0, MathHelper.PiOver2, 0), Vector3.Zero, 0.0005f, PlayerIndex.Two);
            playerTwo.LoadModelBones(game1.Content, Material.White, currentLight);
            playerList.Add(playerTwo);

            //Load Cameras
            currentCameraPlayerOne = new CameraThirdPersonFixed(game1.GraphicsDevice, new Vector3(64, 5, 65), playerOne.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), upView.AspectRatio);
            currentCameraPlayerTwo = new CameraThirdPersonFixed(game1.GraphicsDevice, new Vector3(64, 5, 65), playerTwo.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), downView.AspectRatio);

            //Load Particles
            particleSystemList = new List<ParticleSystem>();
            playerOneRain = new ParticleSystem(ParticleType.Rain, new Vector3(64, 64, 64), new ParticleSpawner(30, false), game1.Content, 1000, 1000, 1);
            playerTwoRain = new ParticleSystem(ParticleType.Rain, new Vector3(64, 64, 64), new ParticleSpawner(30, false), game1.Content, 1000, 1000, 1);

            //Load Floor
            Floor.Start(game1.Content, currentCameraPlayerOne, Material.White, currentLight);

            //Load Sounds
            engineSoundFX = game1.Content.Load<SoundEffect>("engine").CreateInstance();
            engineSoundFX.IsLooped = true;
            engineSoundFX.Volume = 0.1f;
            engineSoundFX.Play();

            rainSoundFX = game1.Content.Load<SoundEffect>("rain").CreateInstance();
            rainSoundFX.IsLooped = true;
            rainSoundFX.Volume = 0.2f;
            rainSoundFX.Play();

            explosionSoundFX = game1.Content.Load<SoundEffect>("explosion");
        }

        internal override void Update(GameTime gameTime)
        {
            if (Input.WasPressed(Keys.Escape) || Input.WasPressed(Buttons.Start, PlayerIndex.One))
                thisGame.ChangeCurrentStage(this, new EscStage(this, thisGame));

            #region Player One Camera
            if (Input.IsPressedDown(Keys.F1) && !(currentCameraPlayerOne is CameraThirdPerson))
            {
                currentCameraPlayerOne = new CameraThirdPerson(currentCameraPlayerOne, playerOne, 2.0f, gameTime);
            }
            else if (Input.IsPressedDown(Keys.F2) && !(currentCameraPlayerOne is CameraFreeSurfaceFolow) && currentCameraPlayerOne.Position.X > 0 && currentCameraPlayerOne.Position.X < Floor.heightMap.Width && currentCameraPlayerOne.Position.Z > 0 && currentCameraPlayerOne.Position.Z < Floor.heightMap.Height)
            {
                currentCameraPlayerOne = new CameraFreeSurfaceFolow(currentCameraPlayerOne);
            }
            else if (Input.IsPressedDown(Keys.F3))
            {
                currentCameraPlayerOne = new CameraFree(currentCameraPlayerOne);
            }
            else if (Input.IsPressedDown(Keys.F4))
            {
                currentCameraPlayerOne = new CameraThirdPersonFixed(currentCameraPlayerOne, playerOne.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(0.2f, 0.3f, 0.2f));
            }
            else if (Input.IsPressedDown(Keys.F5))
            {
                currentCameraPlayerOne = new CameraThirdPersonFixed(currentCameraPlayerOne, playerOne.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f));
            }
            #endregion
            #region Player Two Camera
            if (Input.IsPressedDown(Keys.F7) && !(currentCameraPlayerTwo is CameraThirdPerson))
            {
                currentCameraPlayerTwo = new CameraThirdPerson(currentCameraPlayerTwo, playerTwo, 2.0f, gameTime);
            }
            else if (Input.IsPressedDown(Keys.F8) && !(currentCameraPlayerTwo is CameraFreeSurfaceFolow) && currentCameraPlayerTwo.Position.X > 0 && currentCameraPlayerTwo.Position.X < Floor.heightMap.Width && currentCameraPlayerTwo.Position.Z > 0 && currentCameraPlayerTwo.Position.Z < Floor.heightMap.Height)
            {
                currentCameraPlayerTwo = new CameraFreeSurfaceFolow(currentCameraPlayerTwo);
            }
            else if (Input.IsPressedDown(Keys.F9))
            {
                currentCameraPlayerTwo = new CameraFree(currentCameraPlayerTwo);
            }
            else if (Input.IsPressedDown(Keys.F10))
            {
                currentCameraPlayerTwo = new CameraThirdPersonFixed(currentCameraPlayerTwo, playerTwo.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(0.2f, 0.3f, 0.2f));
            }
            else if (Input.IsPressedDown(Keys.F11))
            {
                currentCameraPlayerTwo = new CameraThirdPersonFixed(currentCameraPlayerTwo, playerTwo.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f));
            }
            #endregion

            currentCameraPlayerOne.Update(gameTime);
            currentCameraPlayerTwo.Update(gameTime);

            foreach (Player p in playerList)
            {
                if (p.hp > 0)
                    p.Update(gameTime);
                else
                {
                    //TODO: explosion on tank ?and gameover screen?
                }
            }

            #region Collisions between bullets and players floor.
            foreach (Player p in playerList)
            {
                for (int i = p.bulletList.Count - 1; i >= 0; i--)
                {
                    if (i >= 0 && p.bulletList[i].position.Y <= Floor.GetHeight(p.bulletList[i].position))
                    {
                        particleSystemList.Add(new ParticleSystem(ParticleType.Explosion, p.bulletList[i].position, new ParticleSpawner(0.2f, true), thisGame.Content, 200, 2000, 1));
                        SoundEffectInstance aux = explosionSoundFX.CreateInstance();
                        aux.Volume = 0.3f;
                        aux.Play();
                        p.bulletList.Remove(p.bulletList[i]);
                    }
                }
            }
            #endregion
            #region Collision between players
            if (OBB.AreColliding(playerOne.boundingBox, playerTwo.boundingBox))
            {
                playerOne.position = playerOne.lastFramePosition;
                playerTwo.position = playerTwo.lastFramePosition;
            }
            #endregion


            //Particle Update
            playerOneRain.Update(new Vector3(playerOne.position.X, 10, playerOne.position.Z), gameTime);
            playerTwoRain.Update(new Vector3(playerTwo.position.X, 10, playerTwo.position.Z), gameTime);
            for (int i = particleSystemList.Count - 1; i >= 0; i--)
            {
                if (particleSystemList[i].particleCount == 0)
                    particleSystemList.Remove(particleSystemList[i]);
                else
                    particleSystemList[i].Update(Vector3.Zero, gameTime);
            }

            //TODO: apply to all objects
            if (OBB.AreColliding(playerOne.boundingBox, playerTwo.boundingBox))
            {
                playerOne.position = playerOne.lastFramePosition;
            }
        }

        internal override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            #region Top Viewport
            device.Viewport = upView;
            Skybox.Draw(device, currentCameraPlayerOne);
            Floor.Draw(currentCameraPlayerOne);
            playerOne.Draw(device, currentCameraPlayerOne);
            playerTwo.Draw(device, currentCameraPlayerOne);
            playerOneRain.Draw(device, currentCameraPlayerOne);
            foreach (ParticleSystem p in particleSystemList)
                p.Draw(device, currentCameraPlayerOne);
            Debug.Draw(currentCameraPlayerOne);
            #endregion

            #region Bottom Viewport
            device.Viewport = downView;
            Skybox.Draw(device, currentCameraPlayerTwo);
            Floor.Draw(currentCameraPlayerTwo);
            playerOne.Draw(device, currentCameraPlayerTwo);
            playerTwo.Draw(device, currentCameraPlayerTwo);
            playerTwoRain.Draw(device, currentCameraPlayerTwo);
            foreach (ParticleSystem p in particleSystemList)
                p.Draw(device, currentCameraPlayerTwo);
            Debug.Draw(currentCameraPlayerTwo);
            #endregion

            device.Viewport = defaultView;

            //batch.Begin();
            //batch.DrawString(Debug.debugFont, playerOne.score.ToString(), new Vector2(device.Viewport.Width / 3, 10.0f), Color.White);
            //batch.DrawString(Debug.debugFont, playerTwo.score.ToString(), new Vector2(device.Viewport.Width / 3 * 2, 10.0f), Color.White);
            //batch.End();

            //device.SamplerStates[0] = SamplerState.LinearWrap;
            //device.BlendState = BlendState.Opaque;
            //device.DepthStencilState = DepthStencilState.Default;
        }

        internal override void Resume()
        {
            if (engineSoundFX == null)
            {
                engineSoundFX = thisGame.Content.Load<SoundEffect>("engine").CreateInstance();
                engineSoundFX.IsLooped = true;
                engineSoundFX.Volume = 0.1f;
            }

            engineSoundFX.Play();
            if (rainSoundFX == null)
            {
                rainSoundFX = thisGame.Content.Load<SoundEffect>("rain").CreateInstance();
                rainSoundFX.IsLooped = true;
                rainSoundFX.Volume = 0.2f;
            }
            rainSoundFX.Play();
        }

        internal override void Stop()
        {
            engineSoundFX.Stop();
            rainSoundFX.Stop();
        }

    }
}
