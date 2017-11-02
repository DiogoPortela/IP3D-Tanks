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
        internal static Player player;

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
            Debug.Start();

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
            currentCamera = new CameraFree(GraphicsDevice, new Vector3(64, 10, 65), 3.0f);
            player = new Player(new Vector3(64, 10, 64), Vector3.Zero, Vector3.Zero, 0.0005f);
            player.LoadModelBones(Content);

            

            Floor.Start(this, currentCamera, Material.White, new Light(-Vector3.One, new Color(new Vector3(0.5f, 0.5f, 0.5f)), new Color(new Vector3(0.1f, 0.1f, 0.1f))));
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
                currentCamera = new CameraThirdPerson(currentCamera, player, 5.0f, gameTime);
            }
            if (Input.IsPressedDown(Keys.F2) && !(currentCamera is CameraFreeSurfaceFolow) && currentCamera.Position.X > 0 && currentCamera.Position.Z > 0/* TODO: Clean camera position check code.*/)
            {
                currentCamera = new CameraFreeSurfaceFolow(currentCamera);
            }
            if (Input.IsPressedDown(Keys.F3) && !(currentCamera is CameraFree))
            {
                currentCamera = new CameraFree(currentCamera);
            }

            currentCamera.Update(gameTime);
            player.Move(gameTime);
            player.Update(gameTime);

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
            player.Draw(currentCamera);

            base.Draw(gameTime);
        }
    }
}
