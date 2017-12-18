using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class MenuStage : Stage
    {
        private Button StartCoop;
        private Button Options;
        private Button Quit;

        internal MenuStage(Game1 game1) : base (game1)
        {
            StartCoop = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3), new Point(150, 50), Color.White, Color.Black, "Start Coop");
            Options = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3 + 100), new Point(150, 50), Color.White, Color.Black, "Options");
            Quit = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3 + 200), new Point(150, 50), Color.White, Color.Black, "Quit");

        }

        internal override void Update(GameTime gameTime)
        {
            if (Input.LeftMouseClick())
            {
                if (StartCoop.IsPointInside(Input.MouseState.Position))
                {
                    thisGame.ChangeCurrentStage(new GameStage(thisGame));
                }
                else if (Quit.IsPointInside(Input.MouseState.Position))
                {
                    thisGame.Quit();
                }
            }            
        }

        internal override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            batch.Begin();
            StartCoop.Draw(batch);
            Options.Draw(batch);
            Quit.Draw(batch);
            batch.End();
        }
    }
}