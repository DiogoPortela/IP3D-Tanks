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
        static SpriteFont debugFont;
        static Color debugColor;
        static Dictionary<String, DebugText> textDictionary;

        class DebugText
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
                batch.DrawString(debugFont, text, position, debugColor);
            }
        }


        internal static void Start()
        {
            textDictionary = new Dictionary<string, DebugText>();
        }
        internal static void AddText(String id, String text, Vector2 screenPosition)
        {
            if (!textDictionary.ContainsKey(id))
                textDictionary.Add(id, new DebugText(text, screenPosition));
        }
        internal static void RemoveText(String id)
        {
            if (textDictionary.ContainsKey(id))
            {
                textDictionary.Remove(id);
            }
        }

        //--------------------Update&Draw--------------------//

        internal static void Update()
        {

        }
        internal static void Draw(SpriteBatch batch)
        {
            foreach (DebugText text in textDictionary)
            {
                text.Draw(batch);
            }
        }
    }
}
