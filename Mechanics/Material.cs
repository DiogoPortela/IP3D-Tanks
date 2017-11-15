using Microsoft.Xna.Framework;

namespace TankProject
{
    /// <summary>
    /// Class to instanciate materials for use during Draw Time.
    /// </summary>
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

        //--------------------Constructors--------------------//

        internal Material(Color diffuse, Color specular, Color ambient, float specularPower)
        {
            diffuseColor = diffuse;
            specularColor = specular;
            ambientColor = ambient;
            this.specularPower = specularPower;
        }

        //--------------------Instances--------------------//
        /// <summary>
        /// Full White material.(normal material)
        /// </summary>
        internal static Material White = new Material(Color.White, Color.White, new Color(new Vector3(0.5f, 0.5f, 0.5f)), 75.0f);
        /// <summary>
        /// Full Black material. (for testing purposes)
        /// </summary>
        internal static Material Black = new Material(Color.Black, Color.White, Color.Black, 75.0f);
    }   
}
