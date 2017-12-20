using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TankProject
{
    class EndStage : Stage
    {
        private string playerOneResult;
        private string playerTwoResult;

        private Vector2 pos1;
        private Vector2 pos2;

        private SoundEffect selectionSoundFX;
        private SoundEffect endSoundFX;

        private Texture2D background;
        private Color color1;
        private Color color2;
        private Rectangle rect1;
        private Rectangle rect2;


        internal EndStage(bool won1, bool won2, Game1 game1) : base(game1)
        {
            if (won1)
            {
                playerOneResult = "VICTORY";
                color1 = Color.Green;

            }
            else
            {
                playerOneResult = "DEFEAT";
                color1 = Color.Red;
            }

            if (won2)
            {
                playerTwoResult = "VICTORY";
                color2 = Color.Green;
            }
            else
            {
                playerTwoResult = "DEFEAT";
                color2 = Color.Red;
            }

            background = game1.Content.Load<Texture2D>("WhitePixel");
            rect1 = new Rectangle(Point.Zero, new Point(800, 400));
            rect2 = new Rectangle(new Point(0, 400), new Point(800, 400));
            pos1 = new Vector2(400 - Debug.debugFont.MeasureString(playerOneResult).X / 2, 200);
            pos2 = new Vector2(400 - Debug.debugFont.MeasureString(playerTwoResult).X / 2, 600);
            selectionSoundFX = game1.Content.Load<SoundEffect>("selection");
            endSoundFX = game1.Content.Load<SoundEffect>("end");
            endSoundFX.Play();

        }

        internal override void Update(GameTime gameTime)
        {
            if (Input.WasPressed(Keys.Escape) || Input.WasPressed(Buttons.Start, PlayerIndex.One))
            {
                thisGame.ChangeCurrentStage(this, new MenuStage(thisGame));
                selectionSoundFX.Play();
            }
        }

        internal override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            batch.Begin();
            batch.Draw(background, rect1, color1);
            batch.Draw(background, rect2, color2);
            batch.DrawString(Debug.debugFont, playerOneResult, pos1, Color.White);
            batch.DrawString(Debug.debugFont, playerTwoResult, pos2, Color.White);
            batch.End();
        }

        internal override void Resume()
        {
           
        }

        internal override void Stop()
        {
            
        }


    }
}
