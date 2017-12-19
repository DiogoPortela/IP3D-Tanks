using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class ControlsStage : Stage
    {
        private Texture2D controls;
        private Stage previousStage;
        private Button back;

        internal ControlsStage(Game1 game1, Stage currentStage) : base(game1)
        {
            //controls = game1.Content.Load<Texture2D>("Gamepad");
            previousStage = currentStage;
            back = new Button(new Point(Game1.graphics.PreferredBackBufferWidth - 200, Game1.graphics.PreferredBackBufferHeight  - 200), new Point(150, 50), Color.White, Color.Black, "Back");

        }

        internal override void Update(GameTime gameTime)
        {
            if(Input.LeftMouseClick())
            {
                if(back.IsPointInside(Input.MouseState.Position))
                {
                    thisGame.ChangeCurrentStage(this, previousStage);
                }
            }
        }

        internal override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            batch.Begin();
            back.Draw(batch);
            batch.End();
        }
    }
}
