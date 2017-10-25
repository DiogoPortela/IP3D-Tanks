using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankProject
{
    class CameraFreeSurfaceFolow : CameraFree
    {
        private float[] nearVertices;

        // --- --- --- --- CONSTRUCTORS --- --- --- --- \\
        internal CameraFreeSurfaceFolow(GraphicsDevice device, Vector3 position, float cameraSpeed = 5.0f, float fieldOfView = 45.0f)
            : base (device, position, cameraSpeed, fieldOfView)
        {
            nearVertices = new float[4];
        }
        internal CameraFreeSurfaceFolow(Camera camera) : base (camera)
        {
            nearVertices = new float[4];
        }

        // --- --- --- --- FUNCTIONS --- --- --- --- \\
        internal override void Move(GameTime gameTime)
        {
            if (Input.IsPressedDown(Keys.NumPad8) && !Input.IsPressedDown(Keys.NumPad5))
                Position += relativeForward * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Input.IsPressedDown(Keys.NumPad5) && !Input.IsPressedDown(Keys.NumPad8))
                Position -= relativeForward * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Input.IsPressedDown(Keys.NumPad4) && !Input.IsPressedDown(Keys.NumPad6))
                Position -= relativeRight * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Input.IsPressedDown(Keys.NumPad6) && !Input.IsPressedDown(Keys.NumPad4))
                Position += relativeRight * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            try
            {
                nearVertices[0] = Floor.VerticesHeight[(int)(this.Position.X), (int)(this.Position.Z)];
                nearVertices[1] = Floor.VerticesHeight[(int)(this.Position.X + 1), (int)(this.Position.Z)];
                nearVertices[2] = Floor.VerticesHeight[(int)(this.Position.X), (int)(this.Position.Z + 1)];
                nearVertices[3] = Floor.VerticesHeight[(int)(this.Position.X + 1), (int)(this.Position.Z + 1)];
            }
            catch
            {
                Game1.currentCamera = new CameraFree(this);
            }

            float yab, ycd, y;
            float da, db, dcd, dab;

            da = (this.Position.X - (int)(this.Position.X));
            db = ((int)(this.Position.X + 1) - this.Position.X);

            yab = db * nearVertices[0] + da * nearVertices[1];
            ycd = db * nearVertices[2] + da * nearVertices[3];

            dcd = ((int)(this.Position.Z + 1) - this.Position.Z);
            dab = (this.Position.Z - (int)(this.Position.Z));

            y = dcd * yab + dab * ycd + 1.0f;
            Position.Y = y;
        }
    }
}
