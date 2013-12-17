using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Stop
{
    class Pointer
    {
        private Texture2D left, right, action, no_action;
        private Vector2 pos = new Vector2();
        private Vector2 clickPos = new Vector2();
        private Texture2D currTexture;
        static Vector2 offsetArrow = new Vector2(32, 32);
        static Vector2 offsetPoint = new Vector2(16, 16);
        public void loadContent(ContentManager content)
        {
            left = content.Load<Texture2D>("left_pointer");
            right = content.Load<Texture2D>("right_pointer");
            action = content.Load<Texture2D>("action_pointer");
            no_action = content.Load<Texture2D>("default_pointer");
            currTexture = no_action;
        }

        public int X { get { return (int)clickPos.X; } }
        public int Y { get { return (int)clickPos.Y; } }

        public void update(int x, int y)
        {
            clickPos.X = x;
            clickPos.Y = y;
            pos.X = x;
            pos.Y = y;

          
        }

        public void setLook(ScreenRegion region)
        {
            MouseLook look = MouseLook.DEFAULT_POINTER;
            if (region != null)
                look = region.MouseLook;
            if (look == MouseLook.DEFAULT_POINTER || look == MouseLook.ACTION)
            {
                pos -= offsetPoint;
            }
            else
            {
                pos -= offsetArrow;
            }

            switch (look)
            {
                case MouseLook.DEFAULT_POINTER:
                    currTexture = no_action;
                    break;
                case MouseLook.LEFT_ARROW:
                    currTexture = left;
                    break;
                case MouseLook.RIGHT_ARROW:
                    currTexture = right;
                    break;
                case MouseLook.ACTION:
                    currTexture = action;
                    break;
                default:
                    currTexture = no_action;
                    break;
            }
        }
        public void draw(SpriteBatch batch)
        {
            batch.Draw(currTexture, pos, Color.White);
        }


    }
}
