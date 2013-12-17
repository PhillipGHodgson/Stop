using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Stop
{
    
    enum MouseLook
    {
        DEFAULT_POINTER,
        LEFT_ARROW,
        RIGHT_ARROW,
        UP_ARROW,
        DOWN_ARROW,
        UPLEFT_ARROW,
        UPRIGHT_ARROW,
        DOWNLEFT_ARROW,
        DOWNRIGHT_ARROW,
        ACTION
    }
    class ScreenRegion
    {
        Rectangle rect;
        RegionData data;
        public ScreenRegion(RegionData data, int screenWidth, int screenHeight)
        {
            rect = new Rectangle((int)(screenWidth * data.xPercent), (int)(screenHeight * data.yPercent),
                                 (int)(screenWidth * data.widthPercent), (int)(screenHeight * data.heightPercent));
            this.data = data;

        }

        public bool contains(int x, int y)
        {
            return rect.Contains(x, y);
        }

        public MouseLook MouseLook
        {
            get{ return (Stop.MouseLook) data.look; }
        }

        public string EventName
        {
            get { return data.eventName; }
        }


        public static ScreenRegion[] getRegions(SceneData data, int screenWidth, int screenHeight)
        {
            ScreenRegion[] regions = new ScreenRegion[data.regions.Length];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = new ScreenRegion(data.regions[i], screenWidth, screenHeight);

            return regions;
        }

    }
}
