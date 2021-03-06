﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankProject
{
    /// <summary>
    /// Free camera that follow ground Height. Use numpad to control.
    /// </summary>
    class CameraFreeSurfaceFolow : CameraFree
    {
        private float[] nearVertices;

        //--------------------Constructors--------------------//

        internal CameraFreeSurfaceFolow(GraphicsDevice device, Vector3 position, float aspectRatio, float cameraSpeed = 5.0f, float fieldOfView = 45.0f)
            : base(device, position, aspectRatio, cameraSpeed, fieldOfView)
        {
            nearVertices = new float[4];
        }
        internal CameraFreeSurfaceFolow(Camera camera) : base(camera)
        {
            nearVertices = new float[4];
        }

        //--------------------Functions--------------------//

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

            if (this.Position.X >= 0 && this.Position.X < Floor.heightMap.Width - 1 && this.Position.Y >= 0 && this.Position.Y < Floor.heightMap.Height - 1)
            {
                nearVertices[0] = Floor.VerticesHeight[(int)(this.Position.X), (int)(this.Position.Z)];
                nearVertices[1] = Floor.VerticesHeight[(int)(this.Position.X + 1), (int)(this.Position.Z)];
                nearVertices[2] = Floor.VerticesHeight[(int)(this.Position.X), (int)(this.Position.Z + 1)];
                nearVertices[3] = Floor.VerticesHeight[(int)(this.Position.X + 1), (int)(this.Position.Z + 1)];

                Position.Y = 1.0f + Interpolation.BiLinear(new Vector2(this.Position.X, this.Position.Z),
                                                           new Vector2((int)this.Position.X, (int)this.Position.Z),
                                                           1.0f, nearVertices[0], nearVertices[1], nearVertices[2], nearVertices[3]);
            }
            else
                Position.Y = 1.0f;
        }
    }
}
