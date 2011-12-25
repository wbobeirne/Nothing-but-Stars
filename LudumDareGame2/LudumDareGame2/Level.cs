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

    public class Level : Microsoft.Xna.Framework.GameComponent {

        public Player Player1 { get; protected set; }
        public Spaceship Ship { get; protected set; }
        public HUD Hud { get; set; }
        public CommonResources Resources { get; protected set; }
        public int ObjectiveStatus { get; protected set; } //0 is finding chip, 1 is returning chip, 2 is finding
                                                           //welder, 3 is returning to repair panel, 4 is finding engine,
                                                           //5 is returning engine, 6 is game end
        private Texture2D[,] bgList = new Texture2D[30, 30];
        public Stack<Collectible> Objectives { get; protected set; }

        private List<Meteorite> meteoriteList = new List<Meteorite>();

        public Level(Game game, HUD h)
            : base(game) {

            Player1 = new Player(game, new Vector2(0, 0));
            Ship = new Spaceship(game, new Vector2(100, 100));
            Hud = h;
        }


        public override void Initialize() {

            base.Initialize();
        }

        public void LoadContent(SpriteBatch sb, ContentManager content) {

            Resources = new CommonResources();
            Resources.LoadContent(sb, content);

            Player1.LoadContent(sb, content);
            Ship.LoadContent(sb, content);
            Hud.LoadContent(sb, content);

            //Initialize bg array
            for (int i = 0; i < bgList.GetLength(0); i++) {
                for (int j = 0; j < bgList.GetLength(1); j++) {
                    bgList[i, j] = Resources.ReturnRandomBackground();
                }
            }

            //Instantiate obj stack
            Objectives = new Stack<Collectible>();
            //Push to objective stack
            Objectives.Push(new Collectible("engine", new Vector2(-3000, -2000), Resources.Engine));
            Objectives.Push(new Collectible("welder", new Vector2(3000, 4000), Resources.Welder));
            Objectives.Push(new Collectible("ai", new Vector2(-200, 200), Resources.Ai));

        }


        public void Update(GameTime gt, KeyboardState kb, KeyboardState kbOld, int gs, Camera2D camera) {

            Player1.Update(gt, kb, kbOld, camera);
            Ship.Update(gt, kb, kbOld, camera);

            /*if (Math.Abs(Player1.MovementSpeed.X) + Math.Abs(Player1.MovementSpeed.Y) > 2) {
                if ((int)gt.TotalGameTime.TotalSeconds % 5 == 0) {
                    Meteorite meteorite = new Meteorite(Resources.Meteor, Resources.MeteorMask,
                        Player1.Position, Player1.MovementSpeed);
                    meteoriteList.Add(meteorite);
                }
            }*/

            //Check through game states for collision logic
            if(ObjectiveStatus == 1 || ObjectiveStatus == 3 || ObjectiveStatus == 5){
                if(Player1.BoundingBox.Intersects(Ship.BoundingBox)){
                    ObjectiveStatus++;
                    Ship.ShipStatus++;
                    Player1.Inventory.RemoveAt(Player1.Inventory.Count - 1);
                    Resources.Repair.Play();
                    Hud.AddMessage(ObjectiveStatus);
                }
            }
            else if(ObjectiveStatus == 0 || ObjectiveStatus == 2 || ObjectiveStatus == 4){
                Collectible col = Objectives.Peek();
                if(Player1.BoundingBox.Intersects(col.BoundingBox)){
                    Player1.Inventory.Add(Objectives.Pop());
                    ObjectiveStatus++;
                    Resources.Pickup.Play();
                    Hud.AddMessage(ObjectiveStatus);
                }
            }

            /*foreach (Meteorite meteorite in meteoriteList) {
                meteorite.Update(gt);
            }*/

            base.Update(gt);
        }

        public void Draw(GameTime gt, SpriteBatch sb, int gs, Camera2D camera) {

            //Draw background
            for (int i = 0; i < 30; i++) {
                for (int j = 0; j < 30; j++) {
                    sb.Draw(bgList[i,j],
                        new Vector2(i * 512 - (512*15), j * 512 - (512*15)),
                        Color.White);
                }
            }

            foreach (Meteorite meteorite in meteoriteList) {
                meteorite.Draw(gt, sb, camera);
            }

            //Draw newest objective, if it's the right obj status
            if (ObjectiveStatus == 0 || ObjectiveStatus == 2 || ObjectiveStatus == 4) {
                Collectible c = Objectives.Peek();
                c.Draw(sb);
            }

            Ship.Draw(gt, sb, camera);

            Player1.Draw(gt, sb, camera);

        }
    }
}
