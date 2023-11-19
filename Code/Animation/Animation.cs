using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Animation
{
    private class PlayInstance
    {
        public Animation animation;
        public int frameCounter;
        public PlayInstance(Animation animation)
        {
            this.animation = animation;
            this.frameCounter = 0;
        }
    }


    private static List<PlayInstance> allAnimations = new List<PlayInstance>();

    public static void DrawAll()
    {
        foreach (PlayInstance animationInstance in allAnimations)
        {
            //Console.WriteLine($"         frame counter {animationInstance.frameCounter}");
            animationInstance.animation.Draw(animationInstance.frameCounter);
        }
    }
    public static void TickAll()
    {
        //Console.WriteLine($"tick : ");
        for (int i = 0; i < allAnimations.Count; i++)
        {
            
            PlayInstance animationInstance = allAnimations[i];
            animationInstance.frameCounter++;
            //Console.WriteLine($"    {i}, {animationInstance.frameCounter}, {animationInstance.animation.Duration}");
            if (animationInstance.frameCounter >= animationInstance.animation.Duration * animationInstance.animation.frameDuration)
            {
                allAnimations.RemoveAt(i--);
                animationInstance.animation.numberOfActiveAnimations--;
            }
        }        

    }

    public static void PlayAnimation(Animation animation)
    {
        allAnimations.Add(new PlayInstance(animation));

        animation.numberOfActiveAnimations++;
        //animation.currentFrame = 0;
        //Console.WriteLine($"     - {animation.IsPlaying}");
    }

    //int currentFrame = 0;
    readonly Texture2D[] frames;
    public Rectangle drawArea;
    readonly int frameDuration;
    private bool useModifiedDrawArea = true;

    int Duration {get {return this.frames.Length;}}

    public bool IsPlaying{get {return numberOfActiveAnimations > 0;}}
    private int numberOfActiveAnimations = 0;

    public Animation(Tuple<Texture2D[], Rectangle> data, int frameDuration, bool useModifiedDrawArea = true)
        :   this(data.Item1, data.Item2, frameDuration, useModifiedDrawArea)
    {}

    public Animation(Texture2D[] frames, Rectangle drawArea, int frameDuration, bool useModifiedDrawArea = true)
    {
        if (drawArea == Rectangle.Empty)
            throw new ArgumentException("Draw area should not be empty!");

        this.frames = frames;
        this.drawArea = drawArea;
        this.frameDuration = frameDuration;
        this.useModifiedDrawArea = useModifiedDrawArea;

        for (int i = 0; i < Duration; i++)
            if (frames[i] == null)
                throw new Exception($"frame {i} is null!");
        

    }

    public void Play()
    {
        PlayAnimation(this);
    }

    private void Draw(int currentFrame)
    {
        //Console.WriteLine("Is drawing...");

        if (currentFrame/frameDuration < Duration)
        {
            Rectangle rect = useModifiedDrawArea ? Camera.ModifiedDrawArea(drawArea, Camera.zoomLevel) : drawArea;
            GameWindow.spriteBatch.Draw(this.frames[currentFrame / frameDuration], rect, Color.White);
        } 
    }


}