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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {

        public int gameState;   //0 is main menu
                                //1 is in game
                                //2 is paused
                                //3 is game ending stuffs

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Song music;

        Level level;
        Camera2D camera;
        HUD hud;

        KeyboardState kbOld;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameState = 1;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
        }


        protected override void Initialize() {

            hud = new HUD();
            level = new Level(this, hud);
            hud.CurrentLevel = level;
            camera = new Camera2D(level.Player1.Position);

            base.Initialize();
        }


        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            music = Content.Load<Song>("Sounds/SpaceMusic");
            MediaPlayer.Volume = 50;
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

            level.LoadContent(spriteBatch, Content);
        }


        protected override void UnloadContent() {
            
        }

        
        protected override void Update(GameTime gameTime) {

            KeyboardState kb = Keyboard.GetState();

            level.Update(gameTime, kb, kbOld, gameState, camera);
            camera.Position = level.Player1.Position;

            //Reduce rotation if it's greater than zero
            if (camera.RotationSpeed > 0f) {
                camera.RotationSpeed -= 0.001f;
                if (camera.RotationSpeed < 0)
                    camera.RotationSpeed = 0f;
            }
            if (camera.RotationSpeed < 0f) {
                camera.RotationSpeed += 0.001f;
                if (camera.RotationSpeed > 0)
                    camera.RotationSpeed = 0f;
            }

            //Rotate camera if Q or E is pressed
            if (kb.IsKeyDown(Keys.E)) {
                camera.RotationSpeed += 0.003f;
                if (camera.RotationSpeed > Camera2D.MAX_ROTATION_SPEED) {
                    camera.RotationSpeed = Camera2D.MAX_ROTATION_SPEED;
                }
            }
            if (kb.IsKeyDown(Keys.Q)) {
                camera.RotationSpeed -= 0.003f;
                if (camera.RotationSpeed < -Camera2D.MAX_ROTATION_SPEED) {
                    camera.RotationSpeed = -Camera2D.MAX_ROTATION_SPEED;
                }
            }

            camera.Rotation += camera.RotationSpeed;

            hud.Update(gameTime, kb, gameState);

            kbOld = kb;
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                null, null, null, null,
                                camera.GetTransformation(graphics.GraphicsDevice));


            level.Draw(gameTime, spriteBatch, gameState, camera);

            hud.Draw(gameTime, spriteBatch, camera);


            spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
