using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        internal static Light currentLight;

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

            currentCameraPlayerOne = new CameraThirdPersonFixed(GraphicsDevice, new Vector3(64, 5, 65), playerOne, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), upView.AspectRatio);
            currentCameraPlayerTwo = new CameraThirdPersonFixed(GraphicsDevice, new Vector3(64, 5, 65), playerTwo, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f), downView.AspectRatio);

            Floor.Start(this, currentCameraPlayerOne, Material.White, currentLight);
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
            else if (Input.IsPressedDown(Keys.F3) && !(currentCameraPlayerOne is CameraFree))
            {
                currentCameraPlayerOne = new CameraFree(currentCameraPlayerOne);
            }
            else if (Input.IsPressedDown(Keys.F4))
            {
                currentCameraPlayerOne = new CameraThirdPersonFixed(currentCameraPlayerOne, playerOne, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(0.2f, 0.3f, 0.2f));
            }
            else if (Input.IsPressedDown(Keys.F5))
            {
                currentCameraPlayerOne = new CameraThirdPersonFixed(currentCameraPlayerOne, playerOne, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f));
            }
            #endregion
            #region Player Two Camera
            if (Input.IsPressedDown(Keys.F7) && !(currentCameraPlayerTwo is CameraThirdPerson))
            {
                currentCameraPlayerTwo = new CameraThirdPerson(currentCameraPlayerTwo, playerOne, 2.0f, gameTime);
            }
            else if (Input.IsPressedDown(Keys.F8) && !(currentCameraPlayerTwo is CameraFreeSurfaceFolow) && currentCameraPlayerTwo.Position.X > 0 && currentCameraPlayerTwo.Position.X < Floor.heightMap.Width && currentCameraPlayerTwo.Position.Z > 0 && currentCameraPlayerTwo.Position.Z < Floor.heightMap.Height)
            {
                currentCameraPlayerTwo = new CameraFreeSurfaceFolow(currentCameraPlayerTwo);
            }
            else if (Input.IsPressedDown(Keys.F9) && !(currentCameraPlayerTwo is CameraFree))
            {
                currentCameraPlayerTwo = new CameraFree(currentCameraPlayerTwo);
            }
            else if (Input.IsPressedDown(Keys.F10))
            {
                currentCameraPlayerTwo = new CameraThirdPersonFixed(currentCameraPlayerTwo, playerTwo, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(0.2f, 0.3f, 0.2f));
            }
            else if (Input.IsPressedDown(Keys.F11))
            {
                currentCameraPlayerTwo = new CameraThirdPersonFixed(currentCameraPlayerTwo, playerTwo, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f));
            }
            #endregion

            currentCameraPlayerOne.Update(gameTime);
            currentCameraPlayerTwo.Update(gameTime);
            playerOne.Update(gameTime);
            playerTwo.Update(gameTime);

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

            GraphicsDevice.Viewport = downView;
            Floor.Draw(currentCameraPlayerTwo);
            playerOne.Draw(currentCameraPlayerTwo);
            playerTwo.Draw(currentCameraPlayerTwo);

            GraphicsDevice.Viewport = defaultView;


            base.Draw(gameTime);
        }
    }
}
