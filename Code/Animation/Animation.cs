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
    private bool useModifiedDrawArea = true;

    int Duration {get {return this.frames.Length;}}

    public bool IsPlaying{get; private set;}

    public Animation(Tuple<Texture2D[], Rectangle> data, int frameDuration, bool useModifiedDrawArea = true)
        :   this(data.Item1, data.Item2, frameDuration, useModifiedDrawArea)
    {}

    public Animation(Texture2D[] frames, Rectangle drawArea, int frameDuration, bool useModifiedDrawArea = true)
    {
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

    private void Draw()
    {
        //Console.WriteLine("Is drawing...");

        if (currentFrame/frameDuration < Duration)
        {
            Rectangle rect = useModifiedDrawArea ? Camera.ModifiedDrawArea(drawArea, Camera.zoomLevel) : drawArea;
            GameWindow.spriteBatch.Draw(this.frames[currentFrame++ / frameDuration], rect, Color.White);
        }
        else
        {
            this.IsPlaying = false;
        }
        
    }


}