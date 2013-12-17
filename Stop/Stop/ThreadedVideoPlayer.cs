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
    class ThreadedVideoPlayer
    {
        private volatile Video _video;
        private VideoPlayer _videoPlayer;
        private volatile MediaState _videoState;
        //private volatile bool _loop;
        private volatile Texture2D _texture;
        private Thread _videoThread;
        private const int SKIP_TOLERANCE = 100;
        private volatile bool _loaded = false;
        private string _name;

        private volatile bool isFinished = false;

        public ThreadedVideoPlayer(ContentManager content, string videoName)
        {
            _name = videoName;
            _video = content.Load<Video>("videos/" + videoName);
            _videoThread = new Thread(DoVideoThread);
            _videoThread.Start();

        }

        public string Name { get { return _name; } }

        public void Dispose()
        {
            _videoState = MediaState.Stopped;
            _videoThread.Abort();
            _videoPlayer.Dispose();
        }

        public Texture2D getCurrentFrame()
        {
            return _texture;
        }

        public bool IsFinished { get { return isFinished; } }

        public bool IsLoaded { get { return _loaded; } }

        public void Pause()
        {
            _videoState = MediaState.Paused;
        }
        public void Play()
        {
            _videoState = MediaState.Playing;
            isFinished = false;
        }

        //called from other thread
        private void UpdateTexture()
        {
            try
            {
                _texture = _videoPlayer.GetTexture();
            }
            catch (ArgumentException)
            {
                _texture = null;
            }
        }

        //called from other thread
        private void loadVideo()
        {
            _loaded = false;
            _videoPlayer.IsMuted = true;
            _videoPlayer.Stop();
          //  _videoPlayer.IsLooped = _loop;
            if (_videoState != MediaState.Playing)
            {
                _videoState = MediaState.Paused;
                isFinished = true;
            }
            _videoPlayer.Play(_video);
            _videoPlayer.Pause();
            if (_videoPlayer.PlayPosition.TotalMilliseconds > SKIP_TOLERANCE)
                loadVideo();
            _videoPlayer.IsMuted = false;
            _loaded = true;
        }

        private void DoVideoThread()
        {
            if (_video == null)
                return;

            if (_videoPlayer == null)
                _videoPlayer = new VideoPlayer();

            loadVideo();



            while (true) //(_videoPlayer.State != MediaState.Stopped)
            {
                if (_videoPlayer.State == MediaState.Playing)
                {
                    UpdateTexture();
                }

                if (_videoPlayer.State == MediaState.Stopped)
                {
                    _videoState = MediaState.Paused;
                    loadVideo();
                }

                if (_videoState != _videoPlayer.State)
                {
                    switch (_videoState)
                    {
                        case MediaState.Paused:
                            _videoPlayer.Pause();
                            break;

                        case MediaState.Playing:
                            if (_videoPlayer.State == MediaState.Stopped)
                            {
                                _videoState = MediaState.Paused;
                                loadVideo();
                                //shouldn't ever get here
                            }
                            else
                            {
                                _videoPlayer.Resume();
                            }
                            break;

                        case MediaState.Stopped:
                            _videoPlayer.Stop();
                            break;
                    }
                }
            }
        } 

    }
}
