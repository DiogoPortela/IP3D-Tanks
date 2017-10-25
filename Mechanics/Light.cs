﻿using Microsoft.Xna.Framework;

namespace TankProject
{
    class Light
    {
        private Color diffuseColor;
        private Color specularColor;
        private Vector3 direction;

        public Vector3 DiffuseColor { get { return diffuseColor.ToVector3(); } set { diffuseColor = new Color(value.X, value.Y, value.Z); } }
        public Vector3 SpecularColor { get { return specularColor.ToVector3(); } set { specularColor = new Color(value.X, value.Y, value.Z); } }
        public Vector3 Direction { get { return direction; } set { direction = value; } }

        public Light(Vector3 direction, Color diffuse, Color specular)
        {
            this.direction = direction;
            diffuseColor = diffuse;
            specularColor = specular;
        }

        public static Light White = new Light(-Vector3.One, Color.White, Color.White);
    }
}
