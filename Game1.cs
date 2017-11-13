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

        internal static Camera currentCamera;
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

            currentCamera = new CameraThirdPerson(GraphicsDevice, new Vector3(64, 10, 65), playerOne, 2.0f);

            Floor.Start(this, currentCamera, Material.White, currentLight);
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
            if (Input.IsPressedDown(Keys.F1) && !(currentCamera is CameraThirdPerson))
            {
                currentCamera = new CameraThirdPerson(currentCamera, playerOne, 2.0f, gameTime);
            }
            else if (Input.IsPressedDown(Keys.F2) && !(currentCamera is CameraFreeSurfaceFolow) && currentCamera.Position.X > 0  && currentCamera.Position.X < Floor.heightMap.Width && currentCamera.Position.Z > 0 && currentCamera.Position.Z < Floor.heightMap.Height)
            {
                currentCamera = new CameraFreeSurfaceFolow(currentCamera);
            }
            else if (Input.IsPressedDown(Keys.F3) && !(currentCamera is CameraFree))
            {
                currentCamera = new CameraFree(currentCamera);
            }
            else if (Input.IsPressedDown(Keys.F4))
            {
                currentCamera = new CameraThirdPersonFixed(currentCamera, playerOne, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(0.2f, 0.3f, 0.2f));
            }
            else if (Input.IsPressedDown(Keys.F5))
            {
                currentCamera = new CameraThirdPersonFixed(currentCamera, playerOne, 2.0f, new Vector3(0.0f, 0.1f, 1.0f), new Vector3(-0.2f, 0.3f, 0.2f));
            }


            currentCamera.Update(gameTime);
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

            Floor.Draw(currentCamera);
            playerOne.Draw(currentCamera);
            playerTwo.Draw(currentCamera);

            base.Draw(gameTime);
        }
    }
}
