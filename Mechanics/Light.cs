using Microsoft.Xna.Framework;

namespace TankProject
{
    /// <summary>
    /// Class to instanciate lights for use during Draw Time.
    /// </summary>
    class Light
    {
        private Color diffuseColor;
        private Color specularColor;
        private Vector3 direction;

        internal Vector3 DiffuseColor { get { return diffuseColor.ToVector3(); } set { diffuseColor = new Color(value.X, value.Y, value.Z); } }
        internal Vector3 SpecularColor { get { return specularColor.ToVector3(); } set { specularColor = new Color(value.X, value.Y, value.Z); } }
        internal Vector3 Direction { get { return direction; } set { direction = value; } }

        //--------------------Constructors--------------------//

        internal Light(Vector3 direction, Color diffuse, Color specular)
        {
            this.direction = direction;
            diffuseColor = diffuse;
            specularColor = specular;
        }

        //--------------------Instances--------------------//
        
        /// <summary>
        /// Standard white light.
        /// </summary>
        internal static Light White = new Light(-Vector3.One, Color.White, Color.White);
    }
}
