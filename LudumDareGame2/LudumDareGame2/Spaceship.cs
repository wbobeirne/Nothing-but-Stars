using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LudumDareGame2 {

    public class Spaceship : Microsoft.Xna.Framework.GameComponent {

        public int ShipStatus { get; set; } //0 = intial, 1 = AI returned, 2 = wing welded, 3 = engine fixed
        public Vector2 Position { get; protected set; }
        public Rectangle BoundingBox { get; protected set; }
        public AnimatedSprite Sprite { get; protected set; }

        private Texture2D brokenShip1; //Wing
        private Texture2D brokenShip2; //Engine
        private Rectangle brokenShipRect1;
        private Rectangle brokenShipRect2;

        public Spaceship(Game game, Vector2 position)
            : base(game) {

            ShipStatus = 0;
            Position = position;

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, 460, 214);
            brokenShipRect1 = new Rectangle((int)Position.X + 270, (int)Position.Y + 130, 23, 29);
            brokenShipRect2 = new Rectangle((int)Position.X + 315, (int)Position.Y + 60, 47, 27);
        }

        public void LoadContent(SpriteBatch sb, ContentManager content) {

            Texture2D animSpr = content.Load<Texture2D>("Sprites/Spaceship/spaceship");
            Sprite = new AnimatedSprite(animSpr, new Point(460, 214), Point.Zero, new Point(1, 1), 20);

            brokenShip1 = content.Load<Texture2D>("Sprites/Spaceship/broken_ship1");
            brokenShip2 = content.Load<Texture2D>("Sprites/Spaceship/broken_ship2");

        }


        public void Update(GameTime gt, KeyboardState kb, KeyboardState kbOld, Camera2D camera) {


            base.Update(gt);
        }

        public void Draw(GameTime gt, SpriteBatch sb, Camera2D camera) {

            Sprite.Draw(gt, sb, Position, 0f);

            //Draw both
            if (ShipStatus == 0 || ShipStatus == 1) {
                sb.Draw(brokenShip1, brokenShipRect1, Color.White);
                sb.Draw(brokenShip2, brokenShipRect2, Color.White);
            }
            //Just the broken engine
            else if (ShipStatus == 2) {
                sb.Draw(brokenShip2, brokenShipRect2, Color.White);
            }

        }
    }
}
