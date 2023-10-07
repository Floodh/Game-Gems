using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Animation
{
    private static List<Animation> allAnimations = new List<Animation>();

    public static void DrawAll()
    {
        foreach (Animation animation in allAnimations)
        {
            animation.Draw();
        }
    }
    public static void TickAll()
    {
        for (int i = 0; i < allAnimations.Count; i++)
        {
            Animation animation = allAnimations[i];
            if (animation.IsPlaying == false)
            {
                i--;
                allAnimations.Remove(animation);
                animation.IsPlaying = false;
            }
        }        

    }


    public static void PlayAnimation(Animation animation)
    {
        allAnimations.Add(animation);
        animation.IsPlaying = true;
        animation.currentFrame = 0;
        //Console.WriteLine($"     - {animation.IsPlaying}");
    }

    int currentFrame = 0;
    readonly Texture2D[] frames;
    Rectangle drawArea;
    readonly int frameDuration;

    int Duration {get {return this.frames.Length;}}

    public bool IsPlaying{get; private set;}

    public Animation(Tuple<Texture2D[], Rectangle> data, int frameDuration)
        :   this(data.Item1, data.Item2, frameDuration)
    {}

    public Animation(Texture2D[] frames, Rectangle drawArea, int frameDuration)
    {
        this.frames = frames;
        this.drawArea = drawArea;
        this.frameDuration = frameDuration;

        for (int i = 0; i < Duration; i++)
            if (frames[i] == null)
                throw new Exception($"frame {i} is null!");
        

    }

    public void Play()
    {
        PlayAnimation(this);
    }

    private void Draw()
    {
        //Console.WriteLine("Is drawing...");

        if (currentFrame/frameDuration < Duration)
        {
            GameWindow.spriteBatch.Draw(this.frames[currentFrame++ / frameDuration], Camera.ModifiedDrawArea(drawArea, Camera.zoomLevel), Color.White);
        }
        else
        {
            this.IsPlaying = false;
        }
        
    }


}