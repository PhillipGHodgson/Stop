using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Stop
{
    class Scene
    {
        public static int WINDOW_WIDTH, WINDOW_HEIGHT;

        private SceneData data;
        private ScreenRegion[] regions;
        private ScreenRegion currentRegion;
        private VideoManager videoManager;
        //private bool loaded = false;
        private Rectangle screenRect;

        public static Scene getScene(string sceneName,Rectangle rect, ContentManager content)
        {
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream(sceneName + ".xml");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data

                XmlSerializer s = new XmlSerializer(typeof(SceneData));
                SceneData data = (SceneData)s.Deserialize(sreader);

                stream.Close();

                return new Scene(data, content, rect);

                
            }
            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
                return null;
            }
        }

        private Scene(SceneData data, ContentManager content, Rectangle rect)
        {
            screenRect = rect;
            this.data = data;
            regions = ScreenRegion.getRegions(data, WINDOW_WIDTH, WINDOW_HEIGHT);
            videoManager = new VideoManager(data.clips, content);
        }


        public bool IsLoaded { get { return videoManager.IsLoaded; } }

        public void startPlay(GameTime time)
        {
            fireEvent(ConditionType.INTRO, new string[0], time);
        }

        public void Update(Pointer pointer, GameTime time)
        {
            videoManager.update(time);
            currentRegion = null;

            if (videoManager.IsLoaded && videoManager.State != MediaState.Playing)
            {
                foreach (ScreenRegion r in regions)
                {

                    if (r.contains(pointer.X, pointer.Y))
                    {
                        currentRegion = r;
                        break;
                    }
                }
            }
            pointer.setLook(currentRegion);
        }

        public void draw(SpriteBatch spritebatch)
        {
            videoManager.DrawFrame(spritebatch, screenRect);
        }

        private void fireEvent(ConditionType type, string[] args, GameTime gameTime)
        {
            foreach(TriggerData t in data.triggers)
            {
                if (isMatch(t, type, args))
                {
                    activateTrigger(t, gameTime);
                }
            }
        }

        private void activateTrigger(TriggerData t, GameTime gameTime)
        {
            foreach (Effect e in t.effects)
            {
                switch (e.type)
                {
                    case EffectType.PLAY_CLIP:
                        videoManager.startVideo(e.effect_args[0], gameTime);
                        break;
                }
            }
        }

        private bool isMatch(TriggerData t, ConditionType type, string[] args)
        {
            if (t.conditions[0].type == type && args.Length >= t.conditions[0].condition_args.Length)
            {
                for (int i = 0; i < t.conditions[0].condition_args.Length; i++)
                {
                    if (t.conditions[0].condition_args[i] != args[i])
                        return false;
                }
            }
            else
                return false;
            return true;
        }

        public void clickEvent(GameTime time)
        {
            if (currentRegion != null)
            {
                fireEvent(ConditionType.CLICK, new string[] { currentRegion.RegionName }, time);
            }
        }

        public MediaState State
        {
            get { return videoManager.State; }
        }

        public void Resume()
        {
            videoManager.Resume();
        }
        public void Pause()
        {
            videoManager.Pause();
        }

        public void Dispose()
        {
            videoManager.Dispose();
        }
        
    }
}
