using Microsoft.Xna.Framework;

namespace TankProject
{
    class OBB
    {
        private Vector3 originalMinBound;
        private Vector3 originalMaxBound;
        private Vector3 minBound;
        private Vector3 maxBound;
        private Vector3 position;
        private Matrix rotationMatrix;
        private Matrix transformationMatrix;

        //--------------------Constructors--------------------//

        public OBB(Vector3 minBound, Vector3 maxBound, Vector3 position)
        {
            this.minBound = originalMinBound = minBound;
            this.maxBound = originalMaxBound = maxBound;
            this.position = position;
            this.rotationMatrix = Matrix.Identity;
            this.transformationMatrix = Matrix.Identity;
        }

        //--------------------Update&Draw--------------------//
        internal void Update(Vector3 position, Vector3 forward, Vector3 right, Vector3 up)
        {
            this.position = position;
            this.rotationMatrix.Forward = forward;
            this.rotationMatrix.Right = right;
            this.rotationMatrix.Up = up;
            this.transformationMatrix = rotationMatrix * Matrix.CreateTranslation(position);

            this.minBound = Vector3.Transform(originalMinBound, transformationMatrix);
            this.maxBound = Vector3.Transform(originalMaxBound, transformationMatrix);
        }

        //--------------------Functions--------------------//

        internal static OBB CreateFromSphere(BoundingSphere sphere, Vector3 position, float scale)
        {
            Vector3 aux = new Vector3(sphere.Radius);
            return new OBB((sphere.Center - aux) * scale, (sphere.Center + aux) * scale, position);
        }
        internal bool Intersects(OBB other)
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
