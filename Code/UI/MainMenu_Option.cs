using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class MainMenu_Option
{

    private const double StartYPercentage = 0.25;
    private const double BufferPercantage = 0.025;
    private const double WidthPercentage = 0.155;
    private const double HeightPercentage = WidthPercentage;
    private const double InnerBufferPercentage = WidthPercentage / 4;

    private static Texture2D frameTexture;

    public bool highlight = false;
    public readonly bool valid = true;

    //public bool IsPicked {get; private set;} = false;
    public Rectangle Bounds {get {return this.drawArea;}}
    public readonly int index;
    public readonly string path = null;

    Rectangle drawArea;
    Rectangle textureDrawArea;
    private int numberOfOptions;

    private Texture2D optionTexture;

    

    private MainMenu_Option(Point windowSize, int numberOfOptions, int index)
    {
        this.numberOfOptions = numberOfOptions;
        this.index = index;
        frameTexture ??= Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/frame2.png");
        this.AdjustDrawArea(windowSize);
    }
    public MainMenu_Option(Point windowSize, int numberOfMaps, int index, string mapImagePath,string texturePath)
        : this(windowSize, numberOfMaps, index)
    {
        this.path = mapImagePath;
        if (File.Exists(texturePath))
            this.optionTexture = Texture2D.FromFile(GameWindow.graphicsDevice, texturePath);
        else
            this.optionTexture = Texture2D.FromFile(GameWindow.graphicsDevice, MainMenu.PATH_MAPDATA_PREVIEW + "Placeholder.jpg");
        
    }
    public MainMenu_Option(Point windowSize, GameArguments.Avatar avatar)
        : this(windowSize, (int)GameArguments.Avatar.Length, (int)avatar)
    {
        if (avatar == GameArguments.Avatar.Wizard)
            this.optionTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Player.Path_BaseTexture);
        if (avatar == GameArguments.Avatar.Orb)
            this.optionTexture = Texture2D.FromFile(GameWindow.graphicsDevice, MainMenu.PATH_MAPDATA_PREVIEW + "Placeholder.jpg");            
    }
    public MainMenu_Option(Point windowSize, GameArguments.CollectionBonus collectionBonus)
        : this(windowSize, (int)GameArguments.CollectionBonus.Length, (int)collectionBonus)
    {
        this.optionTexture = Texture2D.FromFile(GameWindow.graphicsDevice, MainMenu.PATH_MAPDATA_PREVIEW + "Placeholder.jpg");    

    }

    public void AdjustDrawArea(Point windowSize)
    {
        int bufferPxSize = (int)(windowSize.X * BufferPercantage);
        int widthPxSize = (int)(windowSize.X * WidthPercentage);
        int heightPxSize = (int)(windowSize.Y * HeightPercentage);
        int innerWidthBufferPxSize = (int)(windowSize.X * InnerBufferPercentage);
        int innerHeightBufferPxSize = (int)(windowSize.Y * InnerBufferPercentage);

        heightPxSize = widthPxSize;
        innerHeightBufferPxSize = innerWidthBufferPxSize;

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

        this.textureDrawArea = new Rectangle
        (
            drawArea.X + innerWidthBufferPxSize,
            drawArea.Y + innerHeightBufferPxSize,
            drawArea.Width - innerWidthBufferPxSize * 2,
            drawArea.Height - innerHeightBufferPxSize * 2
        );

    }

    public void Draw()
    {
        GameWindow.spriteBatch.Draw(optionTexture, this.textureDrawArea, Color.White);
        GameWindow.spriteBatch.Draw(frameTexture, this.drawArea, Color.White);
    }

    // public T GetValue()
    // {
    //     if (IsPicked)
    //         return this.value;
        
    //     throw new Exception("Tried to retrive value from Main menu option without it being picked!");
    // }


}