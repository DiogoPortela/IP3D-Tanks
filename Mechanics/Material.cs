using Microsoft.Xna.Framework;

namespace TankProject
{
    class Material
    {
        private Color diffuseColor;
        private Color ambientColor;
        private Color specularColor;
        private float specularPower;

        internal Vector3 DiffuseColor { get { return diffuseColor.ToVector3(); } set { diffuseColor = new Color(value.X, value.Y, value.Z); } }
        internal Vector3 AmbientColor { get { return ambientColor.ToVector3(); } set { ambientColor = new Color(value.X, value.Y, value.Z); } }
        internal Vector3 SpecularColor { get { return specularColor.ToVector3(); } set { specularColor = new Color(value.X, value.Y, value.Z); } }
        internal float SpecularPower { get { return specularPower; } set { specularPower = value; } }

        internal Material(Color diffuse, Color specular, Color ambient, float specularPower)
        {
            diffuseColor = diffuse;
            specularColor = specular;
            ambientColor = ambient;
            this.specularPower = specularPower;
        }

        internal static Material White = new Material(Color.White, Color.White, new Color(new Vector3(0.5f, 0.5f, 0.5f)), 75.0f);
        internal static Material Black = new Material(Color.Black, Color.White, Color.Black, 75.0f);
    }   
}
