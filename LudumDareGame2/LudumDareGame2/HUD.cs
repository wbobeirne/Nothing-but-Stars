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

    //Messages made with help from
    //http://www.hackforums.net/archive/index.php/thread-1370804.html
    //Google XNA timed message, pull up the cache for this one
    struct DisplayMessage{
        public string Message;
        public TimeSpan DisplayTime;
        public int CurrentIndex;
        public Vector2 Position;
        public Color DrawColor;
        public string CurrentDrawnMessage;
        public DisplayMessage(string message, TimeSpan displayTime,
            Vector2 pos, Color color){

            Message = message;
            DisplayTime = displayTime;
            CurrentIndex = 0;
            Position = pos;
            DrawColor = color;
            CurrentDrawnMessage = string.Empty;
        }
    }

    public class HUD {

        public int HudState { get; set; }   //0 is main menu
                                            //1 is pre AI
                                            //2 is post AI
                                            //3 is paused
                                            //4 is end game
        public Level CurrentLevel { get; set; }

        #region Constants

        private const string PLAY = "Play";
        private const string EXIT = "Exit";
        private const string RETRIEVE_AI_MSG = "Retrieve the AI chip";
        private const string RETRIEVE_WELDER_MSG = "Navigation online. Retrieve the Welder";
        private const string RETRIEVE_ENGINE_MSG = "Hull breach repaired. Retrieve the Engine Couplings";
        private const string RETURN_AI_MSG = "Return the AI chip to the shuttle";
        private const string RETURN_WELDER_MSG = "Return the Welder to the shuttle";
        private const string RETURN_ENGINE_MSG = "Return the Engine Couplings to the shuttle";
        private const string OBJ_COMPLETE = "Objective complete";
        private DisplayMessage controlsMsg;

        private Vector2 PLAY_POSITION = new Vector2(600, 100);
        private Vector2 EXIT_POSITION = new Vector2(600, 300);
        private Vector2 MSG_POSITION = new Vector2(0, -100);
        private Vector2 CONTROLS_POSITION = new Vector2(0, 100);

        private TimeSpan DISPLAY_TIME_EXTRA_LONG = new TimeSpan(0, 0, 15);
        private TimeSpan DISPLAY_TIME_LONG = new TimeSpan(0, 0, 5);
        private TimeSpan DISPLAY_TIME_SHORT = new TimeSpan(0, 0, 3);

        private SpriteFont HUDFont;

        #endregion

        private List<DisplayMessage> messages = new List<DisplayMessage>();

        public HUD() {
            HudState = 1;

            controlsMsg = new DisplayMessage("Take some time to get used to the EVA controls",
            DISPLAY_TIME_EXTRA_LONG, MSG_POSITION, Color.White);

            messages.Add(controlsMsg);
        }

        public void LoadContent(SpriteBatch sb, ContentManager content) {
            HUDFont = CurrentLevel.Resources.HUDFont;
        }

        public void Update(GameTime gt, KeyboardState kb, int gs) {

            UpdateMessages(gt);

            //messages.Add(new DisplayMessage(RETRIEVE_AI_MSG, DISPLAY_TIME_LONG, Vector2.Zero, Color.White));

            if (gs == 0) {
                HudState = 0;
            }
            else if (gs == 1) {
                //If we haven't collected the AI, don't show the fancy HUD
                if(CurrentLevel.ObjectiveStatus == 0 || CurrentLevel.ObjectiveStatus == 1){
                    HudState = 1;
                    if (CurrentLevel.ObjectiveStatus == 0 && messages.Count == 0) {
                        AddMessage(0);
                    }
                }
                //Now that we have the AI chip, show waypoints etc.
                else{
                    HudState = 2;
                }
            }
            else if (gs == 2) {
                HudState = 3;
            }
            else {
                HudState = 4;
            }
        }

        public void Draw(GameTime gt, SpriteBatch sb, Camera2D camera) {

            DrawMessages(sb, camera);

            if (CurrentLevel.ObjectiveStatus < 5) {
                Collectible col = CurrentLevel.Objectives.Peek();

                if (HudState == 2) {
                    if (CurrentLevel.ObjectiveStatus == 0 || CurrentLevel.ObjectiveStatus == 2 || CurrentLevel.ObjectiveStatus == 4) {
                        sb.Draw(CurrentLevel.Resources.ObjectiveArrow, CurrentLevel.Player1.Position, null,
                            Color.White, GetWaypointRotationAngle(CurrentLevel.Player1.Position, col.Position),
                            new Vector2(CurrentLevel.Resources.ObjectiveArrow.Width / 2, CurrentLevel.Resources.ObjectiveArrow.Height / 2),
                            1f, SpriteEffects.None, 0.5f);
                    }
                }
            }

            if (HudState == 2) {
                if (CurrentLevel.ObjectiveStatus == 1 || CurrentLevel.ObjectiveStatus == 3 || CurrentLevel.ObjectiveStatus == 5) {
                    //This draws arrow to ship
                    sb.Draw(CurrentLevel.Resources.ObjectiveArrow, CurrentLevel.Player1.Position, null,
                        Color.White, GetWaypointRotationAngle(CurrentLevel.Player1.Position, CurrentLevel.Ship.Position),
                        new Vector2(CurrentLevel.Resources.ObjectiveArrow.Width / 2, CurrentLevel.Resources.ObjectiveArrow.Height / 2),
                        1f, SpriteEffects.None, 0.5f);
                }
            }

            //If the controls message is up, show the controls image
            if (CurrentLevel.ObjectiveStatus == 0) {
                sb.Draw(CurrentLevel.Resources.Controls, CONTROLS_POSITION + camera.Position, null, Color.White,
                    -CurrentLevel.Player1.playerRotation,
                    new Vector2(CurrentLevel.Resources.Controls.Width/2, CurrentLevel.Resources.Controls.Height/2),
                    1f, SpriteEffects.None, 0f);
            }
        }


        private void DrawMessages(SpriteBatch sb, Camera2D camera){
            if (messages.Count > 0) {
                for (int i = 0; i < messages.Count; i++) {
                    DisplayMessage msg = messages[i];

                    sb.DrawString(HUDFont, msg.CurrentDrawnMessage, msg.Position + camera.Position, msg.DrawColor,
                        -CurrentLevel.Player1.playerRotation, new Vector2(msg.CurrentDrawnMessage.Length*14 /2, 10),
                        1f, SpriteEffects.None, 0f);
                    if (msg.CurrentIndex == msg.Message.Length) {
                        continue;
                    }

                    msg.CurrentDrawnMessage += msg.Message[msg.CurrentIndex].ToString();
                    msg.CurrentIndex++;
                    messages[i] = msg;
                }
            }
        }

        private void UpdateMessages(GameTime gt) {
            if (messages.Count > 0) {
                for (int i = 0; i < messages.Count; i++) {
                    DisplayMessage msg = messages[i];
                    msg.DisplayTime -= gt.ElapsedGameTime;
                    if (msg.DisplayTime <= TimeSpan.Zero) {
                        messages.RemoveAt(i);
                    }
                    else {
                        messages[i] = msg;
                    }
                }
            }
        }

        private float GetWaypointRotationAngle(Vector2 playerPos, Vector2 objPos){
            Vector2 thing = playerPos - objPos;

            //If we get close to the target, remove the arrow
            if (Math.Abs(thing.X) + Math.Abs(thing.Y) < 400) {
                thing = Vector2.Zero;
            }

            thing = new Vector2(-thing.X, thing.Y);
            Vector2 up = new Vector2(0, 1);

            float rotationAngle = (float)Math.Acos((Vector2.Dot(thing, up)) / (thing.Length() * up.Length()));

            if (playerPos.X > objPos.X) {
                rotationAngle = -rotationAngle;
            }

            return rotationAngle;
        }

        public void AddMessage(int objState) {
            switch (objState) {

                case 0:
                    messages.Add(new DisplayMessage(RETRIEVE_AI_MSG,
                        DISPLAY_TIME_LONG, MSG_POSITION, Color.White));
                    break;
                case 1:
                    messages.Clear();
                    messages.Add(new DisplayMessage(RETURN_AI_MSG,
                        DISPLAY_TIME_LONG, MSG_POSITION, Color.White));
                    break;
                case 2:
                    messages.Clear();
                    messages.Add(new DisplayMessage(RETRIEVE_WELDER_MSG,
                        DISPLAY_TIME_LONG, MSG_POSITION, Color.White));
                    break;
                case 3:
                    messages.Add(new DisplayMessage(RETURN_WELDER_MSG,
                        DISPLAY_TIME_LONG, MSG_POSITION, Color.White));
                    break;
                case 4:
                    messages.Add(new DisplayMessage(RETRIEVE_ENGINE_MSG,
                        DISPLAY_TIME_LONG, MSG_POSITION, Color.White));
                    break;
                case 5:
                    messages.Add(new DisplayMessage(RETURN_ENGINE_MSG,
                        DISPLAY_TIME_LONG, MSG_POSITION, Color.White));
                    break;
                case 6:
                    messages.Add(new DisplayMessage("Congratulations and thanks for playing",
                        DISPLAY_TIME_LONG, MSG_POSITION, Color.White));
                    messages.Add(new DisplayMessage("Sorry I didnt finish the game",
                        DISPLAY_TIME_EXTRA_LONG, CONTROLS_POSITION, Color.White));
                    break;

            }
        }

    }
}
