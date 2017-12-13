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
        internal static bool isRaining = false;
        internal static Vector3 g = new Vector3(0, -9.8f, 0);
        internal static Dictionary<string, float> FRICTION_COEFICIENTS = new Dictionary<string, float>()
        {
            { "kinetic- metal on dry sand", 0.3f},
            { "kinetic- metal on wet sand", 0.4f},
            { "static- metal on dry sand", 0.6f},
            { "static- metal on wet sand", 0.7f}
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
        /// Used for motion physics.
        /// </summary>
        /// <param name="gForce">mg</param>
        /// <param name="theta">angle between a ramp and the axis we are using</param>
        /// <param name="gForceX">result: component x</param>
        /// <param name="gForceY">result: component y</param>
        internal static void DecomposeVector(Vector3 v, float theta, out Vector3 vX, out Vector3 vY)
        {
            vY = v * (float)Math.Sin(theta);
            vX = v * (float)Math.Cos(theta);
        }
    }
}
