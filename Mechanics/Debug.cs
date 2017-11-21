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



        internal static void Start(Color color, SpriteFont spriteFont)
        {
            textDictionary = new Dictionary<string, DebugText>();
            lineDictionary = new Dictionary<string, DebugLine>();
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
}
