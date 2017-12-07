using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    abstract class Stage
    {
        internal abstract void Update(GameTime gameTime);
        internal abstract void Draw(GraphicsDevice device);
    }
}
