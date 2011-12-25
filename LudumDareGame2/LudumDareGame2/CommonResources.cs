/*************
 * This class is used for more generic resources
 * that may need calling on from multiple classes
 * *************/

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
    public class CommonResources {

        //HUD Stuffs
        public SpriteFont HUDFont { get; protected set; }
        public Texture2D Arrow { get; protected set; }
        public Texture2D ObjectiveArrow { get; protected set; }
        public Texture2D BigBlank { get; protected set; }
        public Texture2D Controls { get; protected set; }

        //Background stuffs
        public Texture2D Background1 { get; protected set; }
        public Texture2D Background2 { get; protected set; }
        public Texture2D Background3 { get; protected set; }
        public Texture2D Background4 { get; protected set; }

        private List<Texture2D> bgList = new List<Texture2D>();

        //Items
        public Texture2D Ai { get; protected set; }
        public Texture2D Engine { get; protected set; }
        public Texture2D Welder { get; protected set; }
        public SoundEffect Pickup { get; protected set; }
        public SoundEffect Repair { get; protected set; }

        //Meteor
        public AnimatedSprite Meteor { get; protected set; }
        public Texture2D MeteorMask { get; protected set; }

        public CommonResources() {
        }

        public void LoadContent(SpriteBatch sb, ContentManager content) {

            //HUD stuffs
            HUDFont = content.Load<SpriteFont>("Fonts/HUDFont");
            Arrow = content.Load<Texture2D>("Sprites/HUD/arrow");
            ObjectiveArrow = content.Load<Texture2D>("Sprites/HUD/objective_arrow");
            BigBlank = content.Load<Texture2D>("Sprites/HUD/big_blank");
            Controls = content.Load<Texture2D>("Sprites/HUD/controls");

            //Background stuffs
            bgList.Add(Background1 = content.Load<Texture2D>("Sprites/Backgrounds/background1"));
            bgList.Add(Background2 = content.Load<Texture2D>("Sprites/Backgrounds/background2"));
            bgList.Add(Background3 = content.Load<Texture2D>("Sprites/Backgrounds/background3"));
            bgList.Add(Background4 = content.Load<Texture2D>("Sprites/Backgrounds/background4"));

            //Items
            Ai = content.Load<Texture2D>("Sprites/Items/ai");
            Engine = content.Load<Texture2D>("Sprites/Items/engine");
            Welder = content.Load<Texture2D>("Sprites/Items/welder");
            Pickup = content.Load<SoundEffect>("Sounds/pick_up");
            Repair = content.Load<SoundEffect>("Sounds/repair");

            //Meteor
            Texture2D meteorSprite = content.Load<Texture2D>("Sprites/Meteorite/meteorite");
            Meteor = new AnimatedSprite(meteorSprite, new Point(32, 64), Point.Zero, new Point(1, 1), 20);

            MeteorMask = content.Load<Texture2D>("Sprites/Meteorite/meteorite_mask");

        }

        public Texture2D ReturnRandomBackground() {
            Random r = new Random();
            int i = r.Next(1, bgList.Count);

            return bgList[i];
        }

    }
}
