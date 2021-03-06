﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TankProject
{
    class GameStage : Stage
    {
        private const float ENEMY_MODE_SCALE = 0.002f;

        private Camera currentCameraPlayerOne;
        private Camera currentCameraPlayerTwo;
        private Viewport defaultView, upView, downView;
        internal Player playerOne, playerTwo;
        internal List<Player> playerList;
        internal List<Enemy> enemyList;
        internal List<ParticleSystem> particleSystemList;
        internal ParticleSystem playerOneRain;
        internal ParticleSystem playerTwoRain;

        private SoundEffectInstance engineSoundFX;
        private SoundEffectInstance rainSoundFX;
        private SoundEffect explosionSoundFX;


        internal static Light currentLight;

        /*//DEBUG
        DebugLine debugLine1;
        DebugLine debugLine2;
        DebugLine debugLine3;
        DebugLine debugLine4;
        DebugLine debugLine5;
        DebugLine debugLine6;

        List<DebugBox> boxes;*/

        //--------------------Constructors--------------------//
        internal GameStage(Game1 game1) : base(game1)
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
            Enemy.Load(game1.Content, Material.White, currentLight);

            //Load Players
            playerList = new List<Player>();
            playerOne = new Player(new Vector3(64, 10, 64), Vector3.Zero, Vector3.Zero, 0.0005f, PlayerIndex.One);
            playerOne.LoadModelBones(game1.Content, Material.White, currentLight);
            playerList.Add(playerOne);
            playerTwo = new Player(new Vector3(65, 10, 65), Vector3.Zero, Vector3.Zero, 0.0005f, PlayerIndex.Two);
            playerTwo.LoadModelBones(game1.Content, Material.White, currentLight);
            playerList.Add(playerTwo);

            //Load Cameras
            currentCameraPlayerOne = new CameraThirdPersonFixed(game1.GraphicsDevice, new Vector3(64, 5, 65), playerOne.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), upView.AspectRatio);
            currentCameraPlayerTwo = new CameraThirdPersonFixed(game1.GraphicsDevice, new Vector3(64, 5, 65), playerTwo.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), downView.AspectRatio);

            //Load Enemies
            Random r = new Random();
            enemyList = new List<Enemy>();
            for (int i = 0; i < 60; i++)
            {
                enemyList.Add(new Enemy(new Vector3((float)r.NextDouble() * 128.0f, 0, (float)r.NextDouble() * 128.0f), Vector3.Zero, ENEMY_MODE_SCALE, this));
            }

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


            //TODO: END: CLEAN THIS BEFORE END
            //DEBUG
            /*
            debugLine1 = new DebugLine(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Forward, Color.Blue);
            debugLine2 = new DebugLine(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Right, Color.Red);
            debugLine3 = new DebugLine(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Up, Color.Green);
            debugLine4 = new DebugLine(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Forward, Color.Cyan);
            debugLine5 = new DebugLine(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Right, Color.Magenta);
            debugLine6 = new DebugLine(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Up, Color.Yellow);

            Debug.AddLine("1", debugLine1);
            Debug.AddLine("2", debugLine2);
            Debug.AddLine("3", debugLine3);
            Debug.AddLine("4", debugLine4);
            Debug.AddLine("5", debugLine5);
            Debug.AddLine("6", debugLine6);

            boxes = new List<DebugBox>();
            boxes.Add(new DebugBox(playerOne.boundingBox));
            boxes.Add(new DebugBox(playerTwo.boundingBox));
            boxes.Add(new DebugBox(playerOne.turret.boundingBox));
            boxes.Add(new DebugBox(playerOne.cannon.boundingBox));
            boxes.Add(new DebugBox(playerTwo.turret.boundingBox));
            boxes.Add(new DebugBox(playerTwo.cannon.boundingBox));
            foreach(Enemy e in enemyList)
            {
                boxes.Add(new DebugBox(e.boundingBox));
            }
            int aux = 0;
            foreach (DebugBox b in boxes)
            {
                Debug.AddBox(aux.ToString(), b);
                aux++;
            }*/
        }

        //--------------------Functions--------------------//
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

            #region Collisions between players and enemies
            foreach (Player p in playerList)
            {
                for (int i = p.bulletList.Count - 1; i >= 0; i--)
                {
                    #region bullets and enemies
                    for (int j = enemyList.Count - 1; j >= 0; j--)
                    {
                        if ((p.bulletList[i].position - enemyList[j].position).Length() < 1 && OBB.AreColliding(p.bulletList[i].boundingBox, enemyList[j].boundingBox))
                        {
                            p.score += (int)Vector3.Distance(p.position, enemyList[j].position) * 50;
                            particleSystemList.Add(new ParticleSystem(ParticleType.Explosion, enemyList[j].position, new ParticleSpawner(0.2f, true), thisGame.Content, 200, 2000, 1));
                            SoundEffectInstance aux = explosionSoundFX.CreateInstance();
                            aux.Volume = 0.3f;
                            aux.Play();
                            p.bulletList.Remove(p.bulletList[i]);
                            enemyList.Remove(enemyList[j]);
                            i--;
                            if (i < 0)
                                break;
                        }
                    }
                    if (i < 0)
                        break;
                    #endregion
                    #region bullets and floor
                    if(p.bulletList[i].position.X < 0 || p.bulletList[i].position.X > Floor.heightMap.Width - 1|| p.bulletList[i].position.Z < 0 || p.bulletList[i].position.Z > Floor.heightMap.Height - 1)
                    {
                        if(p.bulletList[i].position.Y <= 0)
                        {
                            particleSystemList.Add(new ParticleSystem(ParticleType.Explosion, p.bulletList[i].position, new ParticleSpawner(0.2f, true), thisGame.Content, 200, 2000, 1));
                            SoundEffectInstance aux = explosionSoundFX.CreateInstance();
                            aux.Volume = 0.3f;
                            aux.Play();
                            p.bulletList.Remove(p.bulletList[i]);
                        }
                    }
                    else if (i >= 0 && p.bulletList[i].position.Y <= Floor.GetHeight(p.bulletList[i].position))
                    {
                        particleSystemList.Add(new ParticleSystem(ParticleType.Explosion, p.bulletList[i].position, new ParticleSpawner(0.2f, true), thisGame.Content, 200, 2000, 1));
                        SoundEffectInstance aux = explosionSoundFX.CreateInstance();
                        aux.Volume = 0.3f;
                        aux.Play();
                        p.bulletList.Remove(p.bulletList[i]);
                    }
                    #endregion
                }

                #region tank and enemies
                foreach (Enemy e in enemyList)
                {
                    if (OBB.AreColliding(p.boundingBox, e.boundingBox))
                    {
                        e.position = e.lastFramePosition;
                        p.position = p.lastFramePosition;
                    }
                }
                #endregion
            }
            #endregion
            #region Collision between players
            if (OBB.AreColliding(playerOne.boundingBox, playerTwo.boundingBox))
            {
                playerOne.position = playerOne.lastFramePosition;
                playerTwo.position = playerTwo.lastFramePosition;
            }
            #endregion

            foreach (Enemy e in enemyList)
            {
                if ((e.position - playerOne.position).Length() < (e.position - playerTwo.position).Length())
                {
                    e.Update(playerOne.position + playerOne.velocity * 60, gameTime);
                }
                else
                {
                    e.Update(playerTwo.position + playerOne.velocity * 60, gameTime);
                }
            }

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

            //TODO: DELETE AT END
            //DEBUG SECTION
            /*
            debugLine1.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Forward);
            debugLine2.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Right);
            debugLine3.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Up);

            debugLine4.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Forward);
            debugLine5.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Right);
            debugLine6.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Up);

            //TODO: fix cannon box
            boxes[0].Update(playerOne.boundingBox);
            boxes[1].Update(playerTwo.boundingBox);
            boxes[2].Update(playerOne.turret.boundingBox);
            boxes[3].Update(playerOne.cannon.boundingBox);
            boxes[4].Update(playerTwo.turret.boundingBox);
            boxes[5].Update(playerTwo.cannon.boundingBox);

            //for(int i = 6; i < boxes.Count-1; i++)
            //{
            //    boxes[i].Update(enemyList[i - 6].boundingBox);
            //}
            */

            //TODO: apply to all objects
            if (OBB.AreColliding(playerOne.boundingBox, playerTwo.boundingBox))
            {
                playerOne.position = playerOne.lastFramePosition;
            }
        }
        internal override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            #region Top viewport
            device.Viewport = upView;
            Skybox.Draw(device, currentCameraPlayerOne);
            Floor.Draw(currentCameraPlayerOne);
            foreach (Enemy e in enemyList)
            {
                e.Draw(device, currentCameraPlayerOne);
            }
            playerOne.Draw(device, currentCameraPlayerOne);
            playerTwo.Draw(device, currentCameraPlayerOne);
            playerOneRain.Draw(device, currentCameraPlayerOne);
            foreach (ParticleSystem p in particleSystemList)
                p.Draw(device, currentCameraPlayerOne);
            Debug.Draw(currentCameraPlayerOne);
            #endregion

            #region Bottom viewport
            device.Viewport = downView;
            Skybox.Draw(device, currentCameraPlayerTwo);
            Floor.Draw(currentCameraPlayerTwo);
            foreach (Enemy e in enemyList)
            {
                e.Draw(device, currentCameraPlayerTwo);
            }
            playerOne.Draw(device, currentCameraPlayerTwo);
            playerTwo.Draw(device, currentCameraPlayerTwo);
            playerTwoRain.Draw(device, currentCameraPlayerTwo);
            foreach (ParticleSystem p in particleSystemList)
                p.Draw(device, currentCameraPlayerTwo);
            Debug.Draw(currentCameraPlayerTwo);
            #endregion

            device.Viewport = defaultView;

            batch.Begin();
            batch.DrawString(Debug.debugFont, playerOne.score.ToString(), new Vector2(device.Viewport.Width / 3, 10.0f), Color.White);
            batch.DrawString(Debug.debugFont, playerTwo.score.ToString(), new Vector2(device.Viewport.Width / 3 * 2, 10.0f), Color.White);
            batch.End();

            device.SamplerStates[0] = SamplerState.LinearWrap;
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
        }

        internal override void Stop()
        {
            engineSoundFX.Stop();
            rainSoundFX.Stop();
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
    }
}
