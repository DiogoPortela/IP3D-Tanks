using Microsoft.Xna.Framework;

namespace TankProject
{
    class OBB
    {
        private Vector3 position;
        private Vector3[] originalCorners;
        private Vector3[] corners;
        private Matrix rotationMatrix;
        private Matrix transformationMatrix;

        //--------------------Constructors--------------------//

        public OBB(Vector3 minBound, Vector3 maxBound, Vector3 position, Matrix rotationMatrix)
        {
            this.position = position;
            GenerateInitialCube(minBound, maxBound);
            this.rotationMatrix = rotationMatrix;
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

            for (int i = 0; i < 8; i++)
            {
                corners[i] = Vector3.Transform(originalCorners[i], transformationMatrix);
            }
        }

        //--------------------Functions--------------------//

        internal static OBB CreateFromSphere(BoundingSphere sphere, Vector3 position, float scale, Matrix rotationMatrix)
        {
            Vector3 aux = new Vector3(sphere.Radius);
            return new OBB((sphere.Center - aux) * scale, (sphere.Center + aux) * scale, position, rotationMatrix);
        }

        private void GenerateInitialCube(Vector3 minBound, Vector3 maxBound)
        {
            this.originalCorners = new Vector3[8] {
                                                                    new Vector3(minBound.X, maxBound.Y, maxBound.Z),
                                                                    new Vector3(maxBound.X, maxBound.Y, maxBound.Z),
                                                                    new Vector3(maxBound.X, minBound.Y, maxBound.Z),
                                                                    new Vector3(minBound.X, minBound.Y, maxBound.Z),
                                                                    new Vector3(minBound.X, maxBound.Y, minBound.Z),
                                                                    new Vector3(maxBound.X, maxBound.Y, minBound.Z),
                                                                    new Vector3(maxBound.X, minBound.Y, minBound.Z),
                                                                    new Vector3(minBound.X, minBound.Y, minBound.Z)
                                                                 };
            this.corners = new Vector3[8] {
                                                                    new Vector3(minBound.X, maxBound.Y, maxBound.Z),
                                                                    new Vector3(maxBound.X, maxBound.Y, maxBound.Z),
                                                                    new Vector3(maxBound.X, minBound.Y, maxBound.Z),
                                                                    new Vector3(minBound.X, minBound.Y, maxBound.Z),
                                                                    new Vector3(minBound.X, maxBound.Y, minBound.Z),
                                                                    new Vector3(maxBound.X, maxBound.Y, minBound.Z),
                                                                    new Vector3(maxBound.X, minBound.Y, minBound.Z),
                                                                    new Vector3(minBound.X, minBound.Y, minBound.Z)
                                                                 };

        }

        //internal bool Intersects(OBB other)
        //{
        //    return other.minBound.X >= this.minBound.X
        //        && other.maxBound.X <= this.maxBound.X
        //        && other.minBound.Y >= this.minBound.Y
        //        && other.maxBound.Y <= this.maxBound.Y
        //        && other.minBound.Z >= this.minBound.Z
        //        && other.maxBound.Z <= this.maxBound.Z;
        //}
        internal Vector3[] GetCorners()
        {
            return corners;
        }
    }
}
