using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LudumDareGame2 {
    public class Meteorite {

        public Texture2D SpriteMask { get; protected set; }
        public AnimatedSprite Sprite { get; protected set; }
        public Vector2 MovementSpeed { get; protected set; }
        public Vector2 Position { get; protected set; }
        public Rectangle BoundingBox { get; protected set; }

        private SoundEffect soundEffect;

        public Meteorite(AnimatedSprite sprite, Texture2D spriteMask, Vector2 playerPosition, Vector2 playerSpeed) {

            Sprite = sprite;
            SpriteMask = spriteMask;
            SetRandomPosition(playerPosition);
            CalculateVelocity(playerPosition, playerSpeed);

        }

        public void Update(GameTime gt) {

            Position += Vector2.Multiply(MovementSpeed, (float)gt.ElapsedGameTime.TotalSeconds * 80);

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);

            Sprite.Update(gt);

        }

        public void Draw(GameTime gt, SpriteBatch sb, Camera2D camera) {

            Sprite.Draw(gt, sb, Position, camera.Rotation);

        }

        private void SetRandomPosition(Vector2 playerPosition) {
            Random r = new Random();
            int x = 0;
            int y = 0;
            while((x > -1000 && x < 1000) || (x > -1000 && x < 1000)){
                x = r.Next(-3000, 3000);
                y = r.Next(-3000, 3000);
            }
            

            Position = new Vector2(playerPosition.X + x, playerPosition.Y + y);
        }

        private void CalculateVelocity(Vector2 pPos, Vector2 pSpeed) {

            System.Diagnostics.Debug.WriteLine(pSpeed);

            Random r = new Random();
            int randDistance = r.Next(200, 400);
            float randVariance = (float)r.Next(90, 115) / 100f;
            Vector2 targetPos = new Vector2(pPos.X + (pSpeed.X * randDistance), pPos.Y + (pSpeed.Y * randDistance));
            MovementSpeed = new Vector2((targetPos.X - Position.X) / randDistance * randVariance, (targetPos.Y - Position.Y) / randDistance * randVariance);
        }

    }
}
