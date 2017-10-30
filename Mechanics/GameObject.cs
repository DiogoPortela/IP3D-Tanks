using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    class GameObject
    {
        internal Vector3 position;
        internal Vector3 rotation;

        private Matrix rotationMatrix;
        private Matrix transformMatrix;
        private BasicEffect effect;
        private Model model;

        internal GameObject(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
            rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
            transformMatrix = Matrix.CreateTranslation(position);
        }

        internal void LoadModel(string modelName, ContentManager content)
        {
            model = content.Load<Model>(modelName);
        }
    }
}
