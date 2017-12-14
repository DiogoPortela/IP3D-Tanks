using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TankProject
{
    class OBB
    {
        internal Vector3 position;
        private Vector3[] originalCorners;
        private Vector3[] corners;
        private Matrix rotationMatrix;
        private Matrix transformationMatrix;

        //--------------------Constructors--------------------//

        public OBB() { }
        
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
            aux.Y /= 3; //scale down the box in y
            aux.X /= 1.2f;
            return new OBB((sphere.Center - aux) * scale, (sphere.Center + aux) * scale, position, rotationMatrix);
        }

        internal Vector3[] GetCorners()
        {
            return corners;
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

        internal static Vector3[] GenerateAxis(OBB a, OBB b)
        {
            Vector3[] axisList = new Vector3[15];

            //a based axes
            axisList[0] = Vector3.Normalize(a.rotationMatrix.Forward);
            axisList[1] = Vector3.Normalize(a.rotationMatrix.Up);
            axisList[2] = Vector3.Normalize(a.rotationMatrix.Right);

            //b based axes
            axisList[3] = Vector3.Normalize(b.rotationMatrix.Forward);
            axisList[4] = Vector3.Normalize(b.rotationMatrix.Up);
            axisList[5] = Vector3.Normalize(b.rotationMatrix.Right);

            //axis based of cross products
            axisList[6] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Forward, b.rotationMatrix.Forward));
            axisList[7] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Forward, b.rotationMatrix.Up));
            axisList[8] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Forward, b.rotationMatrix.Right));

            axisList[6] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Up, b.rotationMatrix.Forward));
            axisList[7] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Up, b.rotationMatrix.Up));
            axisList[8] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Up, b.rotationMatrix.Right));

            axisList[6] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Right, b.rotationMatrix.Forward));
            axisList[7] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Right, b.rotationMatrix.Up));
            axisList[8] = Vector3.Normalize(Vector3.Cross(a.rotationMatrix.Right, b.rotationMatrix.Right));

            return axisList;
        }
        
        internal static bool IntersectsWhenProjected(OBB a, OBB b, Vector3 axis)
        {
            //if a cross product returns zero, then the vectors are alined
            if (axis == Vector3.Zero)
                return true;

            float aMin = float.MaxValue;
            float aMax = float.MinValue;
            float bMin = float.MaxValue;
            float bMax = float.MinValue;

            for (int i = 0; i < 8; i++)
            {
                float aDist = Vector3.Dot(a.GetCorners()[i], axis);
                aMin = (aDist < aMin) ? aDist : aMin;
                aMax = (aDist > aMax) ? aDist : aMax;
                float bDist = Vector3.Dot(b.GetCorners()[i], axis);
                bMin = (bDist < bMin) ? bDist : bMin;
                bMax = (bDist > bMax) ? bDist : bMax;
            }

            //1d test for intersection of a and b
            float longSpan = Math.Max(aMax, bMax) - Math.Min(aMin, bMin);
            float sumSpan = aMax - aMin + bMax - bMin;

            return longSpan < sumSpan;
        }

        internal static bool AreColliding(OBB a, OBB b)
        {
            Vector3[] axisList = OBB.GenerateAxis(a, b);
            bool separatingAxisChecker = false;

            foreach (Vector3 axis in axisList)
            {
                separatingAxisChecker = OBB.IntersectsWhenProjected(a, b, axis);
                if (!separatingAxisChecker)
                    break;
            }
            return separatingAxisChecker;
        }   

    }
}