using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TankProject
{
    class MenuStage : Stage
    {
        private Button StartCoop;
        private Button Controls;
        private Button Quit;

        private int selected;
        private Button[] buttons;

        private SoundEffect menuScrollSoundFX;
        private SoundEffect selectionSoundFX;

        internal MenuStage(Game1 game1) : base (game1)
        {
            StartCoop = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3), new Point(150, 50), Color.Yellow, Color.Black, "Start Coop");
            Controls = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3 + 100), new Point(150, 50), Color.White, Color.Black, "Controls");
            Quit = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3 + 200), new Point(150, 50), Color.White, Color.Black, "Quit");
            buttons = new Button[3];
            buttons[0] = StartCoop;
            buttons[1] = Controls;
            buttons[2] = Quit;

            selected = 0;

            menuScrollSoundFX = game1.Content.Load<SoundEffect>("menuScroll");
            selectionSoundFX = game1.Content.Load<SoundEffect>("selection");
        }

        internal override void Update(GameTime gameTime)
        {
            if (Input.LeftMouseClick())
            {
                if (StartCoop.IsPointInside(Input.MouseState.Position))
                    thisGame.ChangeCurrentStage(this, new GameStage(thisGame));
                else if(Controls.IsPointInside(Input.MouseState.Position))
                    thisGame.ChangeCurrentStage(this, new ControlsStage(thisGame, this));
                else if (Quit.IsPointInside(Input.MouseState.Position))
                    thisGame.Quit();
                selectionSoundFX.Play();
            }
            if (Input.WasPressed(Buttons.A, PlayerIndex.One) || Input.WasPressed(Keys.Enter))
            {
                if (selected == 0)
                    thisGame.ChangeCurrentStage(this, new GameStage(thisGame));
                else if (selected == 1)
                    thisGame.ChangeCurrentStage(this, new ControlsStage(thisGame, this));
                else if (selected == 2)
                    thisGame.Quit();
                selectionSoundFX.Play();
            }
               
                
            if (Input.WasPressed(Buttons.DPadUp, PlayerIndex.One) || Input.WasPressed(Keys.Up) && selected > 0)
            {
                buttons[selected].backgroundColor = Color.White;
                selected--;
                buttons[selected].backgroundColor = Color.Yellow;
                menuScrollSoundFX.Play();
            }
            else if (Input.WasPressed(Buttons.DPadDown, PlayerIndex.One) || Input.WasPressed(Keys.Down) && selected < 2)
            {
                buttons[selected].backgroundColor = Color.White;
                selected++;
                buttons[selected].backgroundColor = Color.Yellow;
                menuScrollSoundFX.Play();
            }

        }

        internal override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            batch.Begin();
            StartCoop.Draw(batch);
            Controls.Draw(batch);
            Quit.Draw(batch);
            batch.End();
        }
    }
}