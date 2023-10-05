using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Animation
{
    private static List<Animation> allAnimations = new List<Animation>();

    int currentFrame = 0;
    readonly Texture2D[] frames;
    Rectangle drawArea;
    readonly int frameDuration;

    int Duration {get {return this.frames.Length;}}

    public Animation(Tuple<Texture2D[], Rectangle> data, int frameDuration)
        :   this(data.Item1, data.Item2, frameDuration)
    {}

    public Animation(Texture2D[] frames, Rectangle drawArea, int frameDuration)
    {
        this.frames = frames;
        this.drawArea = drawArea;
        this.frameDuration = frameDuration;
        allAnimations.Add(this);
    }

    public void Draw()
    {

        if (currentFrame/frameDuration < Duration)
        {
            GameWindow.spriteBatch.Draw(this.frames[currentFrame++ / frameDuration], Camera.ModifiedDrawArea(drawArea, Camera.zoomLevel), Color.White);
        }
        else
        {
            allAnimations.Remove(this); //  consider not doing this in the draw phase
        }
        
    }




}