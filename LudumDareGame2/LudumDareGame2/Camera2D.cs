/****
 * This class made with help from
 * http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
 * ****/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDareGame2 {
    public class Camera2D {

        public float Zoom { get; set; }
        public float Rotation { get; set; }
        public Matrix Transform { get; set; }
        public Vector2 Position { get; set; }
        //The following 2 are for making the camera more floaty
        public float RotationSpeed { get; set; }
        public float FollowSpeed { get; set; }

        public const float MAX_ROTATION_SPEED = 0.02f;

        public Camera2D() {
            Zoom = 1.0f;
            Rotation = 0.0f;
            Position = Vector2.Zero;
        }

        public Camera2D(Vector2 pos) {
            Position = pos;
            Zoom = 1.0f;
            Rotation = 0.0f;
        }

        public void Move(Vector2 amount) {
            Position += amount;
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice) {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                        Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));

            return Transform;
        }

    }
}
