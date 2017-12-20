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


        internal EndStage(bool won1, bool won2, Game1 game1) : base(game1)
        {
            if (won1)
                playerOneResult = "VICTORY";
            else
                playerOneResult = "DEFEAT";

            if (won2)
                playerTwoResult = "VICTORY";
            else
                playerTwoResult = "DEFEAT";

            pos1 = new Vector2(400 - Debug.debugFont.MeasureString(playerOneResult).X / 2, 200);
            pos2 = new Vector2(400 - Debug.debugFont.MeasureString(playerTwoResult).X / 2, 600);
            selectionSoundFX = game1.Content.Load<SoundEffect>("selection");
            endSoundFX = game1.Content.Load<SoundEffect>("end");
            endSoundFX.Play();

        }

        internal override void Update(GameTime gameTime)
        {
            if (Input.WasPressed(Keys.Escape))
            {
                thisGame.ChangeCurrentStage(this, new MenuStage(thisGame));
                selectionSoundFX.Play();
            }
        }

        internal override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            batch.Begin();
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
