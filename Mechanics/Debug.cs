using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankProject
{
    /// <summary>
    /// Class to print out Debug information on screen.
    /// Wanted to create a base class for a few different debugging screens.
    /// </summary>
    static class Debug
    {
        static internal SpriteFont debugFont;
        static internal Color debugColor;
        static internal Dictionary<String, DebugText> textDictionary;
        static internal Dictionary<String, DebugLine> lineDictionary;
        static internal Dictionary<String, DebugBox> boxDictionary;

        internal static void Start(Color color, SpriteFont spriteFont)
        {
            textDictionary = new Dictionary<string, DebugText>();
            lineDictionary = new Dictionary<string, DebugLine>();
            boxDictionary = new Dictionary<string, DebugBox>();
            debugFont = spriteFont;
            debugColor = color;
        }
        internal static void AddText(String id, String text, Vector2 screenPosition)
        {
            if (!textDictionary.ContainsKey(id))
                textDictionary.Add(id, new DebugText(text, screenPosition));
        }
        internal static void AddText(String id, DebugText text)
        {
            if (!textDictionary.ContainsKey(id))
                textDictionary.Add(id, text);
        }
        internal static void RemoveText(String id)
        {
            if (textDictionary.ContainsKey(id))
            {
                textDictionary.Remove(id);
            }
        }
        internal static void AddLine(String id, Vector3 firstPosition, Vector3 secondPosition)
        {
            if (!lineDictionary.ContainsKey(id))
                lineDictionary.Add(id, new DebugLine(firstPosition, secondPosition, debugColor));
        }
        internal static void AddLine(String id, DebugLine line)
        {
            if (!lineDictionary.ContainsKey(id))
                lineDictionary.Add(id, line);
        }
        internal static void RemoveLine(String id)
        {
            if (lineDictionary.ContainsKey(id))
            {
                lineDictionary.Remove(id);
            }
        }
        internal static void AddBox(String id, BoundingBox box)
        {
            if (!boxDictionary.ContainsKey(id))
                boxDictionary.Add(id, new DebugBox(box));
        }
        internal static void AddBox(String id, DebugBox box)
        {
            if (!boxDictionary.ContainsKey(id))
                boxDictionary.Add(id, box);
        }
        internal static void RemoveBox(String id)
        {
            if (boxDictionary.ContainsKey(id))
            {
                boxDictionary.Remove(id);
            }
        }

        //--------------------Update&Draw--------------------//

        internal static void Update()
        {

        }
        internal static void Draw(SpriteBatch batch, Camera cam)
        {
            foreach (KeyValuePair<String, DebugText> text in textDictionary)
            {
                text.Value.Draw(batch);
            }
            foreach (KeyValuePair<String, DebugLine> line in lineDictionary)
            {
                line.Value.Draw(cam);
            }
            foreach(KeyValuePair<String, DebugBox> box in boxDictionary)
            {
                box.Value.Draw(cam);
            }
        }
    }
    internal class DebugText
    {
        private String text;
        private Vector2 position;

        public String Text { get { return text; } set { text = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }

        internal DebugText(String text, Vector2 position)
        {
            this.text = text;
            this.position = position;
        }

        internal void Draw(SpriteBatch batch)
        {
            batch.DrawString(Debug.debugFont, text, position, Debug.debugColor);
        }
    }
    internal class DebugLine
    {
        private VertexPositionColor[] vertexes;
        private BasicEffect effect;

        internal DebugLine(Vector3 firstPosition, Vector3 secondPosition, Color color)
        {
            vertexes = new VertexPositionColor[2];
            vertexes[0] = new VertexPositionColor(firstPosition, color);
            vertexes[1] = new VertexPositionColor(secondPosition, color);
            effect = new BasicEffect(Game1.graphics.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.VertexColorEnabled = true;
            effect.LightingEnabled = false;

        }

        internal void Update(Vector3 firstPosition, Vector3 secondPosition)
        {
            vertexes[0].Position = firstPosition;
            vertexes[1].Position = secondPosition;
        }

        internal void Draw(Camera cam)
        {
            effect.View = cam.ViewMatrix;
            effect.Projection = cam.ProjectionMatrix;

            effect.CurrentTechnique.Passes[0].Apply();
            Game1.graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertexes, 0, 1);
        }
    }
    internal class DebugBox
    {
        private VertexPositionColor[] vertexes;
        private short[] indexes;
        private BasicEffect effect;

        internal DebugBox(BoundingBox box)
        {
            vertexes = new VertexPositionColor[8];
            indexes = new short[24];
            effect = new BasicEffect(Game1.graphics.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.VertexColorEnabled = true;
            effect.LightingEnabled = false;
            Vector3[] aux = box.GetCorners(); //TODO: CLEAN THIS
            vertexes[0] = new VertexPositionColor(aux[0], Color.Green);
            vertexes[1] = new VertexPositionColor(aux[1], Color.Green);
            vertexes[2] = new VertexPositionColor(aux[2], Color.Green);
            vertexes[3] = new VertexPositionColor(aux[3], Color.Green);
            vertexes[4] = new VertexPositionColor(aux[4], Color.Green);
            vertexes[5] = new VertexPositionColor(aux[5], Color.Green);
            vertexes[6] = new VertexPositionColor(aux[6], Color.Green);
            vertexes[7] = new VertexPositionColor(aux[7], Color.Green);

            indexes[0] = 0;
            indexes[1] = 1;
            indexes[2] = 1;
            indexes[3] = 2;
            indexes[4] = 2;
            indexes[5] = 3;
            indexes[6] = 3;
            indexes[7] = 0;
            indexes[8] = 0;
            indexes[9] = 5;
            indexes[10] = 5;
            indexes[11] = 6;
            indexes[12] = 6;
            indexes[13] = 7;
            indexes[14] = 7;
            indexes[15] = 4;
            indexes[16] = 4;
            indexes[17] = 5;
            indexes[18] = 3;
            indexes[19] = 4;
            indexes[20] = 1;
            indexes[21] = 6;
            indexes[22] = 2;
            indexes[23] = 7;
        }

        internal void Update(BoundingBox box)
        {
            Vector3[] aux = box.GetCorners(); //TODO: CLEAN THIS TOO
            vertexes[0].Position = aux[0];
            vertexes[1].Position = aux[1];
            vertexes[2].Position = aux[2];
            vertexes[3].Position = aux[3];
            vertexes[4].Position = aux[4];
            vertexes[5].Position = aux[5];
            vertexes[6].Position = aux[6];
            vertexes[7].Position = aux[7];

        }

        internal void Draw(Camera cam)
        {
            effect.View = cam.ViewMatrix;
            effect.Projection = cam.ProjectionMatrix;

            effect.CurrentTechnique.Passes[0].Apply();
            Game1.graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertexes, 0, 1);
            Game1.graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertexes, 0, 8, indexes, 0, 12);
        }
    }
}
