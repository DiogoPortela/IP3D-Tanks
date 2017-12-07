using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankProject
{
    class Physics
    {
        internal static Vector3 g = new Vector3(0, -9.8f, 0);
        internal static Dictionary<string, float> FRICTION_COEFICIENTS = new Dictionary<string, float>()
        {
            { "metal on dry sand", 0.3f},
            { "metal on wet sand", 0.4f}
        };

        /// <summary>
        /// Multiplies given mass by g
        /// </summary>
        /// <param name="mass"></param>
        /// <returns></returns>
        internal static Vector3 WeightOf(float mass)
        {
            return mass * g;
        }

        /// <summary>
        /// Decomposes mg into two vectors with the x and y component.
        /// Used for friction physics.
        /// </summary>
        /// <param name="gForce">mg</param>
        /// <param name="theta">angle between a ramp and the horizontal axis</param>
        /// <param name="gForceX">result: component x</param>
        /// <param name="gForceY">result: component y</param>
        internal static void DecomposeGravitationalForce(Vector3 gForce, float theta, out Vector3 gForceX, out Vector3 gForceY)
        {
            gForceX = gForce * (float)Math.Sin(theta);
            gForceY = gForce * (float)Math.Cos(theta);
        }

        

    }
}
