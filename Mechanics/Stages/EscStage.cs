using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace TankProject
{
    class EscStage : Stage
    {
        private Stage previousStage;
        private Button Resume;
        private Button Controls;
        private Button ReturnToMenu;
        private Button Quit;

        private Button[] buttons;

        private int selected;

        private SoundEffect menuScrollSoundFX;
        private SoundEffect selectionSoundFX;

        internal EscStage(Stage currentStage, Game1 game1) : base (game1)
        {
            Resume = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3), new Point(150, 50), Color.Yellow, Color.Black, "Resume");
            Controls = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3 + 100), new Point(150, 50), Color.White, Color.Black, "Controls");
            ReturnToMenu = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 80, Game1.graphics.PreferredBackBufferHeight / 3 + 200), new Point(160, 50), Color.White, Color.Black, "Return to Menu");
            Quit = new Button(new Point(Game1.graphics.PreferredBackBufferWidth / 2 - 75, Game1.graphics.PreferredBackBufferHeight / 3 + 300), new Point(150, 50), Color.White, Color.Black, "Quit");

            selected = 0;
            buttons = new Button[4];
            buttons[0] = Resume;
            buttons[1] = Controls;
            buttons[2] = ReturnToMenu;
            buttons[3] = Quit;

            previousStage = currentStage;

            menuScrollSoundFX = game1.Content.Load<SoundEffect>("menuScroll");
            selectionSoundFX = game1.Content.Load<SoundEffect>("selection");
            menuScrollSoundFX.Play();
        }

        internal override void Update(GameTime gameTime)
        {
            if (Input.LeftMouseClick())
            {
                if (Resume.IsPointInside(Input.MouseState.Position))
                {
                    thisGame.ChangeCurrentStage(this, previousStage);
                    (previousStage as GameStage).Resume();
                }
                else if (ReturnToMenu.IsPointInside(Input.MouseState.Position))
                    thisGame.ChangeCurrentStage(this, new MenuStage(thisGame));
                else if (Controls.IsPointInside(Input.MouseState.Position))
                    thisGame.ChangeCurrentStage(this, new ControlsStage(thisGame, this));
                else if (Quit.IsPointInside(Input.MouseState.Position))
                    thisGame.Quit();
                selectionSoundFX.Play();
            }

            if (Input.WasPressed(Buttons.A, PlayerIndex.One) || Input.WasPressed(Keys.Enter))
            {
                if (selected == 0)
                {
                    thisGame.ChangeCurrentStage(this, previousStage);
                    (previousStage as GameStage).Resume();
                }
                else if (selected == 1)
                    thisGame.ChangeCurrentStage(this, new ControlsStage(thisGame, this));
                else if (selected == 2)
                    thisGame.ChangeCurrentStage(this, new MenuStage(thisGame));
                else if (selected == 3)
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
            else if (Input.WasPressed(Buttons.DPadDown, PlayerIndex.One) || Input.WasPressed(Keys.Down) && selected < 3)
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
            Resume.Draw(batch);
            Controls.Draw(batch);
            ReturnToMenu.Draw(batch);
            Quit.Draw(batch);
            batch.End();
        }
    }
}
