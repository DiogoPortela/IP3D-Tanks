using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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


        internal static Light currentLight;

        //DEBUG
        DebugLine debugLine1;
        DebugLine debugLine2;
        DebugLine debugLine3;
        DebugLine debugLine4;
        DebugLine debugLine5;
        DebugLine debugLine6;

        List<DebugBox> boxes;

        //--------------------Constructors--------------------//
        internal GameStage(Game1 game1) : base (game1)
        {
            #region Camera. Split screen
            //Viewports           
            defaultView = Game1.graphics.GraphicsDevice.Viewport;
            upView = downView = defaultView;
            //Dividing it in half, and adjusting the positioning.
            upView.Height /= 2;
            downView.Height /= 2;
            downView.Y = upView.Height;
            #endregion

            currentLight = new Light(-Vector3.One, new Color(new Vector3(0.5f, 0.5f, 0.5f)), new Color(new Vector3(0.1f, 0.1f, 0.1f)));
            Bullet.LoadModel(game1.Content, Material.White, currentLight);
            Skybox.Load(game1.Content);
            Enemy.Load(game1.Content, Material.White, currentLight);

            //Load Players
            playerList = new List<Player>();
            playerOne = new Player(new Vector3(64, 10, 64), Vector3.Zero, Vector3.Zero, 0.0005f, PlayerIndex.One, this);
            playerOne.LoadModelBones(game1.Content, Material.White, currentLight);
            playerList.Add(playerOne);
            playerTwo = new Player(new Vector3(65, 10, 65), Vector3.Zero, Vector3.Zero, 0.0005f, PlayerIndex.Two, this);
            playerTwo.LoadModelBones(game1.Content, Material.White, currentLight);
            playerList.Add(playerTwo);

            currentCameraPlayerOne = new CameraThirdPersonFixed(game1.GraphicsDevice, new Vector3(64, 5, 65), playerOne.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), upView.AspectRatio);
            currentCameraPlayerTwo = new CameraThirdPersonFixed(game1.GraphicsDevice, new Vector3(64, 5, 65), playerTwo.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), downView.AspectRatio);

            Random r = new Random();
            enemyList = new List<Enemy>();
            for(int i = 0; i < 60; i++)
            {
                enemyList.Add(new Enemy(new Vector3((float)r.NextDouble() * 128.0f, 0, (float)r.NextDouble() * 128.0f), Vector3.Zero, ENEMY_MODE_SCALE, this));
            }

            particleSystemList = new List<ParticleSystem>();
            playerOneRain = new ParticleSystem(ParticleType.Rain, new Vector3(64, 64, 64), new ParticleSpawner(30, false), game1.Content, 1000, 1000, 1);
            playerTwoRain = new ParticleSystem(ParticleType.Rain, new Vector3(64, 64, 64), new ParticleSpawner(30, false), game1.Content, 1000, 1000, 1);


            Floor.Start(game1.Content, currentCameraPlayerOne, Material.White, currentLight);

            //TODO: END: CLEAN THIS BEFORE END
            //DEBUG

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
            }
        }

        //--------------------Functions--------------------//
        internal override void Update(GameTime gameTime)
        {
            if (Input.WasPressed(Keys.Escape))
                thisGame.ChangeCurrentStage(new EscStage(this, thisGame));

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

            try
            {
                currentCameraPlayerOne.Update(gameTime);
            }
            catch (Exception exception)
            {
                if (exception.Message == "Camera out of bounds.")
                    currentCameraPlayerOne = new CameraFree(currentCameraPlayerOne);
            }
            try
            {
                currentCameraPlayerTwo.Update(gameTime);
            }
            catch (Exception exception)
            {
                if (exception.Message == "Camera out of bounds.")
                    currentCameraPlayerTwo = new CameraFree(currentCameraPlayerTwo);
            }

            foreach(Player p in playerList)
            {
                if(p.hp > 0)
                    p.Update(gameTime);
                else
                {
                    //TODO: explosion on tank ?and gameover screen?
                }
            }

            #region Collisions between players and enemies
            foreach(Player p in playerList)
            {
                #region bullets and enemies
                for (int i = p.bulletList.Count - 1; i >= 0; i--)
                {
                    for (int j = enemyList.Count - 1; j >= 0; j--)
                    {
                        try
                        {
                            if (OBB.AreColliding(p.bulletList[i].boundingBox, enemyList[j].boundingBox))
                            {
                                p.bulletList.Remove(p.bulletList[i]);
                                enemyList.Remove(enemyList[j]);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("DEU MERDA");
                        }
                    }
                }
                #endregion

                #region tank and enemies
                foreach(Enemy e in enemyList)
                {
                    if(OBB.AreColliding(p.boundingBox, e.boundingBox))
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
                if((e.position - playerOne.position).Length() < (e.position - playerTwo.position).Length())
                {
                    e.Update(playerOne.position + playerOne.velocity * 60, gameTime);
                }
                else
                {
                    e.Update(playerTwo.position + playerOne.velocity * 60, gameTime);
                }
            }

            playerOneRain.Update(new Vector3(playerOne.position.X, 10, playerOne.position.Z), gameTime);
            playerTwoRain.Update(new Vector3(playerTwo.position.X, 10, playerTwo.position.Z), gameTime);


            //TODO: DELETE AT END
            //DEBUG SECTION
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

            for(int i = 6; i < boxes.Count-1; i++)
            {
                boxes[i].Update(enemyList[i - 6].boundingBox);
            }
        }
        internal override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
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
            Debug.Draw(currentCameraPlayerOne);

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
            Debug.Draw(currentCameraPlayerTwo);

            device.Viewport = defaultView;
        }
    }
}
