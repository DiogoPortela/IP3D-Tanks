using Microsoft.Xna.Framework;

namespace TankProject
{

    static class Interpolation
    {
        public static float BiLinear(Vector2 currentPosition, Vector2 gridPosition, float gridSize, float valueOne, float valueTwo, float valueThree, float valueFour)
        {
            float yAB, yCD, y;
            float da, db, dcd, dab;

            da = (currentPosition.X - gridPosition.X);
            db = ((gridPosition.X + gridSize) - currentPosition.X);

            yAB = (db * valueOne + da * valueTwo);
            yCD = db * valueThree + da * valueFour;

            dcd = ((gridPosition.Y + gridSize) - currentPosition.Y);
            dab = (currentPosition.Y - gridPosition.Y);

            y = dcd * yAB + dab * yCD;

            return y;
        }
        public static Vector3 BiLinear(Vector2 currentPosition, Vector2 gridPosition, float gridSize, Vector3 valueOne, Vector3 valueTwo, Vector3 valueThree, Vector3 valueFour)
        {
            Vector3 yAB, yCD, y;
            float da, db, dcd, dab;

            da = (currentPosition.X - gridPosition.X);
            db = ((gridPosition.X + gridSize) - currentPosition.X);

            yAB = (db * valueOne + da * valueTwo);
            yCD = db * valueThree + da * valueFour;

            dcd = ((gridPosition.Y + gridSize) - currentPosition.Y);
            dab = (currentPosition.Y - gridPosition.Y);

            y = dcd * yAB + dab * yCD;

            return y;
        }
    }
}

