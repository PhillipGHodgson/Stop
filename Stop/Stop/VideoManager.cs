using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
namespace Stop
{
    class VideoManager
    {
        private ThreadedVideoPlayer[] players;
        ThreadedVideoPlayer activePlayer;
        MediaState _state;
        TimeSpan videoStartTime;
        private bool useOldFrame = false;
        private const int FRAME_HOLD_TIME_MILLIS = 100;
        private ThreadedVideoPlayer lastPlayer = null;

        public VideoManager(string[] videos, ContentManager content)
        {
            players = new ThreadedVideoPlayer[videos.Length];
            for (int i = 0; i < videos.Length; i++)
            {
                players[i] = new ThreadedVideoPlayer(content, videos[i]);
            }
        }

        public MediaState State { get { return _state; } }

        public void DrawFrame(SpriteBatch spritebatch, Rectangle rect)
        {
            if (activePlayer != null)
            {
                Texture2D texture;
                if(useOldFrame && lastPlayer != null)
                    texture = lastPlayer.getCurrentFrame();
                else
                    texture = activePlayer.getCurrentFrame();
                if(texture != null)
                    spritebatch.Draw(texture, rect, Color.White);
            }
        }

        public void startVideo(string name, GameTime time)
        {
            for(int i = 0; i < players.Length; i++)
            {
                if (players[i].Name == name)
                {
                    startVideo(i, time);
                    break;
                }
            }
        }

        public void startVideo(int index, GameTime time)
        {
            useOldFrame = true;
            lastPlayer = activePlayer;
            videoStartTime = time.TotalGameTime;
            if (activePlayer != null)
                activePlayer.Pause();
            activePlayer = players[index];
            activePlayer.Play();
            _state = MediaState.Playing;
        }

        public void Pause()
        {
            if (activePlayer != null)
            {
                activePlayer.Pause();
                _state = MediaState.Paused;
            }

        }

        public bool IsLoaded
        {
            get
            {
                foreach (ThreadedVideoPlayer player in players)
                {
                    if (!player.IsLoaded)
                        return false;
                }
                return true;
            }
        }

        public void Resume()
        {
            if (activePlayer != null)
                activePlayer.Play();
            _state = MediaState.Playing;
        }

        public void Dispose()
        {
            foreach (ThreadedVideoPlayer player in players)
                player.Dispose();
        }

        public void update(GameTime time)
        {
            if (useOldFrame) 
                if( time.TotalGameTime.Subtract(videoStartTime).Milliseconds > FRAME_HOLD_TIME_MILLIS)
                useOldFrame = false;

            if (activePlayer != null && activePlayer.IsFinished)
                _state = MediaState.Stopped;
        }
    }
}
