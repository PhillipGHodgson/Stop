#region File Description
//-----------------------------------------------------------------------------
// Game1.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

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
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Storage;


namespace Stop
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Pointer pointer;
        SpriteFont Font1;

        //   Video video1, video2, video3, video4;
        //VideoPlayer player;
        // Texture2D videoTexture;
        MouseState lastMouseState;
        KeyboardState lastKeyboardState;

        // SceneData data;

        //ScreenRegion currentRegion = null;

        // ScreenRegion[] regions;

        Scene currentScene;
        Scene nextScene;
        Rectangle screen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            pointer = new Pointer();
          //  graphics.PreferredBackBufferHeight = 720;
           // graphics.PreferredBackBufferWidth = 1280;
              graphics.PreferredBackBufferHeight = 480;
              graphics.PreferredBackBufferWidth = 640;

              Scene.WINDOW_HEIGHT = graphics.PreferredBackBufferHeight;
              Scene.WINDOW_WIDTH = graphics.PreferredBackBufferWidth;
             
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
           // this.IsMouseVisible = true;

            // Drawing to the rectangle will stretch the 
            // video to fill the screen
            
        }



        protected override void LoadContent()
        {
            screen = new Rectangle(GraphicsDevice.Viewport.X,
                GraphicsDevice.Viewport.Y,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            /*
            video1 = Content.Load<Video>("Movie_0001");
            video2 = Content.Load<Video>("Movie_0002");
            video3 = Content.Load<Video>("Movie_0003");
            video4 = Content.Load<Video>("Movie_0004");

            player = new VideoPlayer();
            player.Play(video1);

            */
         //   string[] videos = new string[] {"Movie_0010", "Movie_0006", "Movie_0009", "Movie_0008"};

            Font1 = Content.Load<SpriteFont>("SpriteFont1");

            pointer.loadContent(Content);
            nextScene = Scene.getScene("testStop",screen, Content);

           
            lastMouseState = Mouse.GetState();
            lastKeyboardState = Keyboard.GetState();
        }

        protected override void Update(GameTime gameTime)
        {
            //switch scene check
            if (currentScene == null && nextScene.IsLoaded)
            {
                currentScene = nextScene;
                nextScene = null;
                currentScene.startPlay(gameTime);
            }



            MouseState currMouseState = Mouse.GetState();
            KeyboardState currKeyboardState = Keyboard.GetState();

            pointer.update(currMouseState.X, currMouseState.Y);

            if (currentScene != null)
            {
                currentScene.Update(pointer, gameTime);
            }


            // Allows the game to exit
            if (currentScene != null && 
                currKeyboardState.IsKeyDown(Keys.Escape) && !lastKeyboardState.IsKeyDown(Keys.Escape))
            {
                if (currentScene.State == MediaState.Paused)
                    currentScene.Resume();
                else if (currentScene.State == MediaState.Playing)
                    currentScene.Pause();
            }

            if (currentScene != null && currentScene.State == MediaState.Stopped)
            {
                if (currMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    currentScene.clickEvent(gameTime);
                }
            }
            lastMouseState = currMouseState;
            lastKeyboardState = currKeyboardState;
            base.Update(gameTime);
        }

        

        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            spriteBatch.Begin();

            if (currentScene == null)
                spriteBatch.DrawString(Font1, "Loading...", Vector2.Zero, Color.White);
            else
                currentScene.draw(spriteBatch);
      
            pointer.draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            if(currentScene != null)
                currentScene.Dispose();
            if (nextScene != null)
                nextScene.Dispose();
            base.UnloadContent();
        }
    }

}

