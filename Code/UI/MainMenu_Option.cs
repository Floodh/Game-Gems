using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class MainMenu_Option
{

    private const double StartYPercentage = 0.25;
    private const double BufferPercantage = 0.025;
    private const double WidthPercentage = 0.10;
    private const double HeightPercentage = WidthPercentage;

    private static Texture2D frameTexture;

    public bool highlight = false;
    public readonly bool valid = true;

    //public bool IsPicked {get; private set;} = false;
    public Rectangle Bounds {get {return this.drawArea;}}
    public readonly int index;

    Rectangle drawArea;
    private int numberOfOptions;

    

    private MainMenu_Option(Point windowSize, int numberOfOptions, int index)
    {
        this.numberOfOptions = numberOfOptions;
        this.index = index;
        frameTexture ??= Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/frame2.png");
        this.AdjustDrawArea(windowSize);
    }
    public MainMenu_Option(Point windowSize, GameArguments.Map map)
        : this(windowSize, (int)GameArguments.Map.Length, (int)map)
    {}
    public MainMenu_Option(Point windowSize, GameArguments.Avatar avatar)
        : this(windowSize, (int)GameArguments.Avatar.Length, (int)avatar)
    {}
    public MainMenu_Option(Point windowSize, GameArguments.CollectionBonus collectionBonus)
        : this(windowSize, (int)GameArguments.CollectionBonus.Length, (int)collectionBonus)
    {}

    public void AdjustDrawArea(Point windowSize)
    {
        int bufferPxSize = (int)(windowSize.X * BufferPercantage);
        int widthPxSize = (int)(windowSize.X * WidthPercentage);
        int heightPxSize = (int)(windowSize.Y * HeightPercentage);

        int combinedWidth = bufferPxSize * (numberOfOptions - 1) + widthPxSize * numberOfOptions;
        int startX = (windowSize.X - combinedWidth) / 2;
        int startY = (int)(windowSize.Y * StartYPercentage);

        this.drawArea = new Rectangle
        (
            startX + bufferPxSize * (index - 1) + widthPxSize * index,
            startY,
            widthPxSize,
            heightPxSize
        );
    }

    public void Draw()
    {
        GameWindow.spriteBatch.Draw(frameTexture, this.drawArea, Color.White);
    }

    // public T GetValue()
    // {
    //     if (IsPicked)
    //         return this.value;
        
    //     throw new Exception("Tried to retrive value from Main menu option without it being picked!");
    // }


}