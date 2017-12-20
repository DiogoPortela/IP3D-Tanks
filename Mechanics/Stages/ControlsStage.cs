using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace TankProject
{
    class ControlsStage : Stage
    {
        private Texture2D controls1;
        private Texture2D controls2;
        private Texture2D currentTexture;
        private Rectangle rect;
        private Stage previousStage;

        private Button back;
        private Button playerOneBtn;
        private Button playerTwoBtn;

        private Button[] buttons;

        private int selected;

        private SoundEffect menuScrollSoundFX;
        private SoundEffect selectionSoundFX;

        internal ControlsStage(Game1 game1, Stage currentStage) : base(game1)
        {
            currentTexture = controls1 = game1.Content.Load<Texture2D>("playerOneHelp");
            controls2 = game1.Content.Load<Texture2D>("playerTwoHelp");
            rect = new Rectangle(new Point(10, 10), new Point(500, 500));
            previousStage = currentStage;
            back = new Button(new Point(Game1.graphics.PreferredBackBufferWidth - 200, Game1.graphics.PreferredBackBufferHeight  - 200), new Point(150, 50), Color.White, Color.Black, "Back");
            playerOneBtn = new Button(new Point(Game1.graphics.PreferredBackBufferWidth - 200, Game1.graphics.PreferredBackBufferHeight - 400), new Point(150, 50), Color.Yellow, Color.Black, "P1");
            playerTwoBtn = new Button(new Point(Game1.graphics.PreferredBackBufferWidth - 200, Game1.graphics.PreferredBackBufferHeight - 300), new Point(150, 50), Color.White, Color.Black, "P2");

            selected = 0;
            buttons = new Button[3];
            buttons[0] = playerOneBtn;
            buttons[1] = playerTwoBtn;
            buttons[2] = back;

            menuScrollSoundFX = game1.Content.Load<SoundEffect>("menuScroll");
            selectionSoundFX = game1.Content.Load<SoundEffect>("selection");
        }

        internal override void Update(GameTime gameTime)
        {
            if(Input.LeftMouseClick())
            {
                if(back.IsPointInside(Input.MouseState.Position))
                    thisGame.ChangeCurrentStage(this, previousStage);
                else if (playerOneBtn.IsPointInside(Input.MouseState.Position))
                    currentTexture = controls1;
                else if (playerTwoBtn.IsPointInside(Input.MouseState.Position))
                    currentTexture = controls2;
                selectionSoundFX.Play();

            }

            if (Input.WasPressed(Buttons.A, PlayerIndex.One) || Input.WasPressed(Keys.Enter))
            {
                if (selected == 0)
                    currentTexture = controls1;
                else if (selected == 1)
                    currentTexture = controls2;
                else if (selected == 2)
                    thisGame.ChangeCurrentStage(this, previousStage);
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
            back.Draw(batch);
            playerOneBtn.Draw(batch);
            playerTwoBtn.Draw(batch);
            batch.Draw(currentTexture, rect, Color.White);
            batch.End();
        }
    }
}
