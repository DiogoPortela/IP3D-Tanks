using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankProject
{
    class BoundingBox
    {
        private Vector3 minBound;
        private Vector3 maxBound;

        public BoundingBox(Vector3 minBound, Vector3 maxBound)
        {
            this.minBound = minBound;
            this.maxBound = maxBound;
        }

        public static BoundingBox CreateFromSphere(BoundingSphere sphere)
        {
            Vector3 vector1 = new Vector3(sphere.Radius);
            return new BoundingBox(sphere.Center - vector1, sphere.Center + vector1);
        }
    }
}
