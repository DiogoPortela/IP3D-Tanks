using Microsoft.Xna.Framework;

namespace TankProject
{
    class BoundingBox
    {
        private Vector3 minBound;
        private Vector3 maxBound;
        private Vector3 position;

        //--------------------Constructors--------------------//

        public BoundingBox(Vector3 minBound, Vector3 maxBound, Vector3 position)
        {
            this.minBound = minBound;
            this.maxBound = maxBound;
            this.position = position;
        }

        //--------------------Update&Draw--------------------//
        internal void Update(Vector3 position)
        {
            this.position = position;

        }

        //--------------------Functions--------------------//

        internal static BoundingBox CreateFromSphere(BoundingSphere sphere, Vector3 position, float scale)
        {
            Vector3 aux = new Vector3(sphere.Radius);
            return new BoundingBox((sphere.Center - aux) * scale, (sphere.Center + aux) * scale, position);
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
                new Vector3(this.minBound.X, this.maxBound.Y, this.maxBound.Z) + position,
                new Vector3(this.maxBound.X, this.maxBound.Y, this.maxBound.Z) + position,
                new Vector3(this.maxBound.X, this.minBound.Y, this.maxBound.Z) + position,
                new Vector3(this.minBound.X, this.minBound.Y, this.maxBound.Z) + position,
                new Vector3(this.minBound.X, this.maxBound.Y, this.minBound.Z) + position,
                new Vector3(this.maxBound.X, this.maxBound.Y, this.minBound.Z) + position,
                new Vector3(this.maxBound.X, this.minBound.Y, this.minBound.Z) + position,
                new Vector3(this.minBound.X, this.minBound.Y, this.minBound.Z) + position
            };
        }
    }
}
