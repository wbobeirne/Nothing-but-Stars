using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Class provided by O'Reilly's learning XNA 3.0 book, with
//some of my own tweaks
namespace LudumDareGame2 {
    public class AnimatedSprite {

        Texture2D textureImage;
        protected Point frameSize;
        Point currentFrame;
        Point sheetSize;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16;

        public AnimatedSprite(Texture2D textureImage, Point frameSize,
        Point currentFrame, Point sheetSize,
        int millisecondsPerFrame) {
            this.textureImage = textureImage;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public Texture2D GetTexture2D() {
            return textureImage;
        }

        public virtual void Update(GameTime gameTime) {
            //Update animation frame
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame) {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X) {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 pos, float rotation) {
            SpriteEffects se = SpriteEffects.None;
            //Draw the sprite
            spriteBatch.Draw(textureImage, pos,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                Color.White, rotation, new Vector2(15, 24),
                1f, se, 0);
        }



    }
}
