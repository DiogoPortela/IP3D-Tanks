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
            Vector3 aux = new Vector3(sphere.Radius);
            return new BoundingBox(sphere.Center - aux, sphere.Center + aux);
        }

        public bool Intersects(BoundingBox other)
        {
            return other.minBound.X >= this.minBound.X
                && other.maxBound.X <= this.maxBound.X
                && other.minBound.Y >= this.minBound.Y
                && other.maxBound.Y <= this.maxBound.Y
                && other.minBound.Z >= this.minBound.Z
                && other.maxBound.Z <= this.maxBound.Z;
        }

        public Vector3[] GetCorners()
        {
            return new Vector3[] {
                new Vector3(this.minBound.X, this.maxBound.Y, this.maxBound.Z),
                new Vector3(this.maxBound.X, this.maxBound.Y, this.maxBound.Z),
                new Vector3(this.maxBound.X, this.minBound.Y, this.maxBound.Z),
                new Vector3(this.minBound.X, this.minBound.Y, this.maxBound.Z),
                new Vector3(this.minBound.X, this.maxBound.Y, this.minBound.Z),
                new Vector3(this.maxBound.X, this.maxBound.Y, this.minBound.Z),
                new Vector3(this.maxBound.X, this.minBound.Y, this.minBound.Z),
                new Vector3(this.minBound.X, this.minBound.Y, this.minBound.Z)
            };
        }
    }
}
