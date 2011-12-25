using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LudumDareGame2 {

    public class Player : Microsoft.Xna.Framework.GameComponent {

        public Vector2 Position { get; protected set; }
        public Vector2 MovementSpeed { get; set; }
        public Rectangle BoundingBox { get; protected set; }
        public AnimatedSprite Sprite { get; protected set; }
        public List<Collectible> Inventory { get; protected set; }

        private const float MAX_SPEED = 3f;
        private const float ACCELERATION = 1.8f;
        private const float DECCELERATION = 0.6f;
        public float playerRotation = 0f;
        private float playerRotationAcceleration = 0f;
        private const float MAX_ROTATION_ACCELERATION = 0.02f;
        private SoundEffect move;
        private SoundEffectInstance moveEffect;

        public Player(Game game, Vector2 position)
            : base(game) {

            Position = position;
            MovementSpeed = Vector2.Zero;
            BoundingBox = new Rectangle((int)Position.X - 22,
                (int)Position.Y -22,
                32,
                45);
            Inventory = new List<Collectible>();
        }

        public void LoadContent(SpriteBatch sb, ContentManager content) {

            Texture2D animSpr = content.Load<Texture2D>("Sprites/Player/astronaut");
            Sprite = new AnimatedSprite(animSpr, new Point(32, 45), Point.Zero, new Point(1, 1), 20);
            move = content.Load<SoundEffect>("Sounds/move");
            moveEffect = move.CreateInstance();
            moveEffect.IsLooped = true;
            moveEffect.Volume = 0.3f;
        }


        public void Update(GameTime gt, KeyboardState kb, KeyboardState kbOld, Camera2D camera) {

            Vector2 oldMovementSpeed = MovementSpeed;

            float decelerationX = MovementSpeed.X * (float)gt.ElapsedGameTime.TotalSeconds * 0.3f;
            float decelerationY = MovementSpeed.Y * (float)gt.ElapsedGameTime.TotalSeconds * 0.3f;
            float acceleration = ACCELERATION * (float)gt.ElapsedGameTime.TotalSeconds;

            //Slow down the space man
            if (MovementSpeed.X > 0) {
                MovementSpeed += new Vector2(-decelerationX, 0);
            }
            else if (MovementSpeed.X < 0) {
                MovementSpeed += new Vector2(-decelerationX, 0);
            }

            if (MovementSpeed.Y > 0) {
                MovementSpeed += new Vector2(0, -decelerationY);
            }
            else if (MovementSpeed.Y < 0) {
                MovementSpeed += new Vector2(0, -decelerationY);
            }

            BoundingBox = new Rectangle((int)Position.X - 22,
                (int)Position.Y - 22,
                45,
                45);

            //Add speed based on kb input
            if (kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.A)) {
                MovementSpeed += Utility.CalculateDiagonalMovement(acceleration, camera.Rotation, "left");
                moveEffect.Play();
            }

            if (kb.IsKeyDown(Keys.Right) || kb.IsKeyDown(Keys.D)) {
                MovementSpeed += Utility.CalculateDiagonalMovement(acceleration, camera.Rotation, "right");
                moveEffect.Play();
            }

            if (kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.W)) {
                MovementSpeed += Utility.CalculateDiagonalMovement(-acceleration, camera.Rotation, "up");
                moveEffect.Play();
            }

            if (kb.IsKeyDown(Keys.Down) || kb.IsKeyDown(Keys.S)) {
                MovementSpeed += Utility.CalculateDiagonalMovement(-acceleration, camera.Rotation, "down");
                moveEffect.Play();
            }
            //Kill sound effect if all keys are up
            if (kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.S) && kb.IsKeyUp(Keys.D))
                moveEffect.Stop();

            //Adding gametime was something I forgot to do, so *85 is just a number that felt similar
            //to running the game at 60fps with no gametime multiplication
            Position += Vector2.Multiply(MovementSpeed, (float)gt.ElapsedGameTime.TotalSeconds * 85);
            //Position += MovementSpeed;

            //Determine player rotation to try to orient with the camera
            if (playerRotation < camera.Rotation && !kb.IsKeyDown(Keys.Q)) {
                playerRotationAcceleration += 0.0002f;
                playerRotation += MathHelper.Clamp(playerRotationAcceleration, playerRotationAcceleration, MAX_ROTATION_ACCELERATION);
                if (playerRotation > camera.Rotation) {
                    playerRotation = camera.Rotation;
                }
            }
            else if (playerRotation > camera.Rotation && !kb.IsKeyDown(Keys.E)) {
                playerRotationAcceleration += 0.0002f;
                playerRotation -= MathHelper.Clamp(playerRotationAcceleration, playerRotationAcceleration, MAX_ROTATION_ACCELERATION);
                if (playerRotation < camera.Rotation) {
                    playerRotation = camera.Rotation;
                }
            }
            //When there's no rotation, reset the speed so we get that acceleration again.
            else {
                playerRotationAcceleration = 0f;
            }


            base.Update(gt);
        }

        public void Draw(GameTime gt, SpriteBatch sb, Camera2D camera) {

            //Negate the rotation, the camera takes in the float backwards
            Sprite.Draw(gt, sb, Position, -playerRotation);

        }
    }
}
