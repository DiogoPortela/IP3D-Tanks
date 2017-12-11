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
        private Camera currentCameraPlayerOne;
        private Camera currentCameraPlayerTwo;
        private Viewport defaultView, upView, downView;
        internal Player playerOne, playerTwo;

        internal static Light currentLight;

        //DEBUG
        DebugLine debugLine1;
        DebugLine debugLine2;
        DebugLine debugLine3;
        DebugLine debugLine4;
        DebugLine debugLine5;
        DebugLine debugLine6;

        DebugLine debugLineEnemyVelocity;
        List<DebugBox> boxes;
        ParticleSystem teste;

        Bullet testbullet;
        Enemy testEnemy;

        //--------------------Constructors--------------------//
        internal GameStage(ContentManager content, GraphicsDevice device)
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

            //Load Players
            playerOne = new Player(new Vector3(64, 10, 64), Vector3.Zero, Vector3.Zero, 0.0005f, Player.PlayerNumber.PlayerOne, this);
            playerOne.LoadModelBones(content, Material.White, currentLight);
            playerTwo = new Player(new Vector3(65, 10, 65), Vector3.Zero, Vector3.Zero, 0.0005f, Player.PlayerNumber.PlayerTwo, this);
            playerTwo.LoadModelBones(content, Material.White, currentLight);

            Bullet.LoadModel(content, Material.White, currentLight);
            Skybox.Load(content);
            Enemy.Load(content);
            currentCameraPlayerOne = new CameraThirdPersonFixed(device, new Vector3(64, 5, 65), playerOne.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), upView.AspectRatio);
            currentCameraPlayerTwo = new CameraThirdPersonFixed(device, new Vector3(64, 5, 65), playerTwo.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), downView.AspectRatio);

            Floor.Start(content, currentCameraPlayerOne, Material.White, currentLight);

            //TODO: END: CLEAN THIS BEFORE END
            //DEBUG
            testEnemy = new Enemy(new Vector3(80, 0, 80), Vector3.Zero);

            debugLine1 = new DebugLine(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Forward, Color.Blue);
            debugLine2 = new DebugLine(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Right, Color.Red);
            debugLine3 = new DebugLine(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Up, Color.Green);
            debugLine4 = new DebugLine(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Forward, Color.Cyan);
            debugLine5 = new DebugLine(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Right, Color.Magenta);
            debugLine6 = new DebugLine(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Up, Color.Yellow);
            debugLineEnemyVelocity = new DebugLine(testEnemy.position, testEnemy.position + testEnemy.velocity, Color.Pink);

            Debug.AddLine("1", debugLine1);
            Debug.AddLine("2", debugLine2);
            Debug.AddLine("3", debugLine3);
            Debug.AddLine("4", debugLine4);
            Debug.AddLine("5", debugLine5);
            Debug.AddLine("6", debugLine6);
            Debug.AddLine("7", debugLineEnemyVelocity);
            teste = new ParticleSystem(ParticleType.Rain, new Vector3(64, 64, 64), new ParticleSpawner(30, false), content, 1000, 1000, 10);

            boxes = new List<DebugBox>();
            boxes.Add(new DebugBox(playerOne.boundingBox));
            boxes.Add(new DebugBox(playerTwo.boundingBox));
            boxes.Add(new DebugBox(playerOne.turret.boundingBox));
            boxes.Add(new DebugBox(playerOne.cannon.boundingBox));
            boxes.Add(new DebugBox(playerTwo.turret.boundingBox));
            boxes.Add(new DebugBox(playerTwo.cannon.boundingBox));
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

            playerOne.Update(gameTime);
            playerTwo.Update(gameTime);


            //TODO: DELETE AT END
            //DEBUG SECTION
            teste.Update(new Vector3(playerOne.position.X, 10, playerOne.position.Z), gameTime);
            testEnemy.Update(playerOne.position, gameTime);
            debugLine1.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Forward);
            debugLine2.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Right);
            debugLine3.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Up);

            debugLine4.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Forward);
            debugLine5.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Right);
            debugLine6.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Up);
            debugLineEnemyVelocity.Update(testEnemy.position, testEnemy.position + testEnemy.velocity);

            if (OBB.AreColliding(playerOne.boundingBox, playerTwo.boundingBox))
            {
                Console.WriteLine("Collision Detected");
            }
            //TODO: Apply OBB's to all bones

            boxes[0].Update(playerOne.boundingBox);
            boxes[1].Update(playerTwo.boundingBox);
            boxes[2].Update(playerOne.turret.boundingBox);
            boxes[3].Update(playerOne.cannon.boundingBox);
            boxes[4].Update(playerTwo.turret.boundingBox);
            boxes[5].Update(playerTwo.cannon.boundingBox);
        }
        internal override void Draw(GraphicsDevice device)
        {
            device.Viewport = upView;
            Skybox.Draw(device, currentCameraPlayerOne);
            Floor.Draw(currentCameraPlayerOne);
            playerOne.Draw(currentCameraPlayerOne);
            playerTwo.Draw(currentCameraPlayerOne);
            teste.Draw(device, currentCameraPlayerOne); //DELETE
            testEnemy.Draw(device, currentCameraPlayerOne);
            Debug.Draw(currentCameraPlayerOne);

            device.Viewport = downView;
            Skybox.Draw(device, currentCameraPlayerTwo);
            Floor.Draw(currentCameraPlayerTwo);
            playerOne.Draw(currentCameraPlayerTwo);
            playerTwo.Draw(currentCameraPlayerTwo);
            Debug.Draw(currentCameraPlayerTwo);

            device.Viewport = defaultView;
        }
    }
}
