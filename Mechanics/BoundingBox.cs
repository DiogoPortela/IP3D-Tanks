using Microsoft.Xna.Framework;

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

        internal static BoundingBox CreateFromSphere(BoundingSphere sphere, float scale)
        {
            Vector3 aux = new Vector3(sphere.Radius) * scale;
            return new BoundingBox(sphere.Center - aux, sphere.Center + aux);
        }

        internal bool Intersects(BoundingBox other)
        {
            return other.minBound.X >= this.minBound.X
                && other.maxBound.X <= this.maxBound.X
                && other.minBound.Y >= this.minBound.Y
                && other.maxBound.Y <= this.maxBound.Y
                && other.minBound.Z >= this.minBound.Z
                && other.maxBound.Z <= this.maxBound.Z;
        }

        internal Vector3[] GetCorners()
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
