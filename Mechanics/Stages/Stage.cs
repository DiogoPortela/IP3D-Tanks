using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    abstract class Stage
    {
        protected Game1 thisGame;

        internal Stage(Game1 game1) {
            thisGame = game1;
        }

        internal abstract void Update(GameTime gameTime);
        internal abstract void Draw(GraphicsDevice device, SpriteBatch batch);
    }

    class Button
    {
        internal static Texture2D texture;
        internal Color backgroundColor;
        internal Color textColor;
        internal Rectangle rectangle;
        internal string text;
        internal Vector2 textPosition;

        internal static void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("WhitePixel");
        }

        internal Button(Point position, Point size, Color backgroundColor, Color textColor, string text)
        {
            this.rectangle = new Rectangle(position, size);
            this.backgroundColor = backgroundColor;
            this.textColor = textColor;
            this.text = text;
            textPosition = position.ToVector2() + (size.ToVector2() / 2.0f) - (Debug.debugFont.MeasureString(text) / 2.0f);
        }

        internal bool IsVectorInside(Vector2 position)
        {
            return (rectangle.Contains(position.ToPoint()));
        }
        internal bool IsPointInside(Point position)
        {
            return (rectangle.Contains(position));
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, backgroundColor);
            spriteBatch.DrawString(Debug.debugFont, text, textPosition, textColor);
        }
    }
}
