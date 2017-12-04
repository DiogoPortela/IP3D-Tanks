using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TankProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        internal static GraphicsDeviceManager graphics;
        internal static SpriteBatch spriteBatch;

        internal static Camera currentCameraPlayerOne;
        internal static Camera currentCameraPlayerTwo;
        static private Viewport defaultView, upView, downView;
        internal static Player playerOne, playerTwo;
        internal static List<Bullet> bulletList;

        internal static Light currentLight;

        //DEBUG
        DebugLine debugLine1;
        DebugLine debugLine2;
        DebugLine debugLine3;
        DebugLine debugLine4;
        DebugLine debugLine5;
        DebugLine debugLine6;
        List<DebugBox> boxes;
        ParticleSystem teste;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Input.Start();
            Debug.Start(Color.Green, Content.Load<SpriteFont>("Arial"));

            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            #region Camera. Split screen
            //Viewports           
            defaultView = Game1.graphics.GraphicsDevice.Viewport;
            upView = downView = defaultView;
            //Dividing it in half, and adjusting the positioning.
            upView.Height /= 2;
            downView.Height /= 2;
            downView.Y = upView.Height;
            #endregion

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentLight = new Light(-Vector3.One, new Color(new Vector3(0.5f, 0.5f, 0.5f)), new Color(new Vector3(0.1f, 0.1f, 0.1f)));

            playerOne = new Player(new Vector3(64, 10, 64), Vector3.Zero, Vector3.Zero, 0.0005f, Player.PlayerNumber.PlayerOne);
            playerOne.LoadModelBones(Content, Material.White, currentLight);

            playerTwo = new Player(new Vector3(65, 10, 65), Vector3.Zero, Vector3.Zero, 0.0005f, Player.PlayerNumber.PlayerTwo);
            playerTwo.LoadModelBones(Content, Material.White, currentLight);

            Bullet.LoadModel(this.Content);
            bulletList = new List<Bullet>();

            currentCameraPlayerOne = new CameraThirdPersonFixed(GraphicsDevice, new Vector3(64, 5, 65), playerOne.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), upView.AspectRatio);
            currentCameraPlayerTwo = new CameraThirdPersonFixed(GraphicsDevice, new Vector3(64, 5, 65), playerTwo.turret, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), downView.AspectRatio);

            Floor.Start(this, currentCameraPlayerOne, Material.White, currentLight);

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
            teste = new ParticleSystem(ParticleType.Rain, new Vector3(64, 64, 64), new ParticleSpawner(30, false),Content, 1000, 1000, 10);

            boxes = new List<DebugBox>();
            int aux = 0;
            //foreach (BoundingBox b in playerOne.boundingBoxes)
            //{
            //    boxes.Add(new DebugBox(b));
            //}
            boxes.Add(new DebugBox(playerOne.boundingBox));
            boxes.Add(new DebugBox(playerTwo.boundingBox));
            foreach (DebugBox b in boxes)
            {
                Debug.AddBox(aux.ToString(), b);
                aux++;
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update();
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
            playerOne.Update(gameTime);
            playerTwo.Update(gameTime);

            foreach (Bullet b in bulletList)
            {
                b.Update(gameTime);
            }

            //TODO: DELETE AT END
            //DEBUG SECTION
            teste.Update(gameTime);
            debugLine1.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Forward);
            debugLine2.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Right);
            debugLine3.Update(playerOne.cannon.position, playerOne.cannon.position + playerOne.cannon.Up);

            debugLine4.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Forward);
            debugLine5.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Right);
            debugLine6.Update(playerOne.turret.position, playerOne.turret.position + playerOne.turret.Up);

            boxes[0].Update(playerOne.boundingBox);
            boxes[1].Update(playerTwo.boundingBox);

            if (playerOne.boundingBox.Intersects(playerTwo.boundingBox))
            {
                Console.WriteLine("COLLISION DETECTED");
            }

            Debug.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.Viewport = upView;
            Floor.Draw(currentCameraPlayerOne);
            playerOne.Draw(currentCameraPlayerOne);
            playerTwo.Draw(currentCameraPlayerOne);
            foreach (Bullet b in bulletList)
            {
                b.Draw(currentCameraPlayerOne);
            }
            teste.Draw(GraphicsDevice, currentCameraPlayerOne); //DELETE
            Debug.Draw(spriteBatch, currentCameraPlayerOne);

            GraphicsDevice.Viewport = downView;
            Floor.Draw(currentCameraPlayerTwo);
            playerOne.Draw(currentCameraPlayerTwo);
            playerTwo.Draw(currentCameraPlayerTwo);
            foreach (Bullet b in bulletList)
            {
                b.Draw(currentCameraPlayerTwo);
            }
            Debug.Draw(spriteBatch, currentCameraPlayerTwo);

            GraphicsDevice.Viewport = defaultView;

            base.Draw(gameTime);
        }
    }
}
