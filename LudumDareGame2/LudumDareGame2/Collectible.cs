using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LudumDareGame2 {
    public class Collectible {

        public Rectangle BoundingBox { get; protected set; }
        public string Name { get; protected set; }  //"ai"
                                                    //"welder"
                                                    //"engine"
        public Vector2 Position { get; protected set; }
        public SoundEffect PickupSound { get; protected set; }
        public Texture2D Sprite { get; protected set; }
        private float rotation = 0f;

        public Collectible(string name, Vector2 pos, Texture2D sprt) {

            Name = name;
            Position = pos;
            Sprite = sprt;
            BoundingBox = new Rectangle(
                (int)Position.X - 16,
                (int)Position.Y - 16,
                32,
                32);
        }

        public void Draw(SpriteBatch sb) {
            rotation += 0.003f;

            sb.Draw(Sprite, Position, null, Color.White, rotation, new Vector2(16, 16), 1f, SpriteEffects.None, 0);
        }

        public void PlayPickupSound() {

        }

    }
}
