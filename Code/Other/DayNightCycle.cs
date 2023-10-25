
using System;
using FontStashSharp;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

class DayNightCycle
{

    public const int DayDuration_sec = 60;
    public const int NightDuration_sec = 60;

    public const int DayDuration_tick = (int)Level.tickPerSec * DayDuration_sec;
    public const int NightDuration_tick = (int)Level.tickPerSec * NightDuration_sec;

    private static SpriteFontBase font;
    
    public bool IsDay {get; private set;} = true;
    public bool IsNight {get{return !IsDay;} private set {this.IsDay = !value;}}

    private int timeLeft;
    private int tick;

    private string text;
    private string countdownText;
    private Color textColor;
    private Point windowSize;
    private Vector2 drawVec_text;
    private Vector2 drawVec_countdownText;


    



    public DayNightCycle(Point windowSize)
    {
        this.tick = 0;
        this.timeLeft = DayDuration_sec;
        this.text = "Day";
        this.countdownText = "0";
        this.textColor = Color.Yellow;

        font ??= ResourcesUi.FontSystem.GetFont(56);
        this.SetWindowSize(windowSize);

    }

    public void Tick(int currentTick)
    {
        this.tick = currentTick;

        int counter = this.tick;
        int dayNumber = 1;
        int nightNumber = 1;

        while (true)
        {
            if (counter < DayDuration_tick)
            {
                IsDay = true;
                this.timeLeft = DayDuration_tick - counter;
                break;
            }
            dayNumber++;
            counter -= DayDuration_tick;
            if (counter < NightDuration_tick)
            {
                IsNight = true;
                this.timeLeft = NightDuration_tick - counter;
                break;
            }
            nightNumber++;
            counter -= NightDuration_tick;

        }

        if (this.IsDay)
        {
            Sunlight.Mask = new Color(0.85f,0.85f,0.85f,1.0f);
            this.text = $"Day : {dayNumber}";
            this.countdownText = $"{DayDuration_sec - timeLeft / (int)Level.tickPerSec}";
            this.textColor = Color.Yellow;
            this.RecaulcuateRendering();
        }
        else
        {
            Sunlight.Mask = new Color(0.55f,0.55f,0.55f,1.0f);
            this.text = $"Night : {nightNumber}";
            this.countdownText = $"{DayDuration_sec - timeLeft / (int)Level.tickPerSec}";
            this.textColor = Color.DarkCyan;
            this.RecaulcuateRendering();
        }

    }

    public void SetWindowSize(Point size)
    {
        this.windowSize = size;
        this.RecaulcuateRendering();
    }

    private void RecaulcuateRendering()
    {
        Vector2 textSizeVector = font.MeasureString(text);
        this.drawVec_text = new Vector2(this.windowSize.X / 2 - textSizeVector.X / 2, 50f);
        Vector2 countdownTextSizeVector = font.MeasureString(countdownText);  
        this.drawVec_countdownText = new Vector2(this.windowSize.X / 2 - countdownTextSizeVector.X / 2, 100f);      
    }

    public void Draw()
    {
        GameWindow.spriteBatch.DrawString(font, this.text, drawVec_text, this.textColor);
        GameWindow.spriteBatch.DrawString(font, this.countdownText, drawVec_countdownText, this.textColor);
    }


}