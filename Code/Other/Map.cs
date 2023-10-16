using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;
using System.IO;



class Map
{

    public enum TilesRGB : uint
    {
        Dirt = 0xFF663931,
        Grass = 0xFF4b692F,
        Water = 0xFF5b6ee1,
        Stone = 0xFF595652
    }

    private const string DIRT_TEXTURE_PATH = "Data/Texture/Dirt.png";
    private const string GRASS_TEXTURE_PATH = "Data/Texture/Grass.png";
    private const string WATER_TEXTURE_PATH = "Data/Texture/Water.png";
    private const string GRID_VALIDTILE_TEXTURE_PATH = "Data/Texture/Grid_ValidTile.png";
    private const string GRID_INVALIDTILE_TEXTURE_PATH = "Data/Texture/Grid_InValidTile.png";

    public const int mapPixelToGridTile_Multiplier = 1;  //  1 pixel = 2x2 tiles
    public const int mapPixelToTexturePixel_Multiplier = 16 * 2;

    public Size SourceSize { get {return this.SourceImage.Size;} }
    public Bitmap SourceImage {get; private set;}

    public Point drawOffset = Point.Zero;

    private Texture2D drawTexture;
    private Texture2D gridDrawTexture;

    private Texture2D[] dirtTextures;
    private Texture2D[] grassTextures;
    private Texture2D[] waterTextures;
    private Texture2D[] stoneTextures;

    private Texture2D validTileTexture;
    private Texture2D inValidTileTexture;

    private Size drawTextureSize;


    public bool RenderGrid {get { return this.renderGrid;} set{this.renderGrid = value; if(value)this.RenderGridStatus();}}
    private bool renderGrid = false;

    


    public Map(string path)
    {
        Random random = new();
        //this.drawOffset = new Point(-150, -200);
        //  load the map
        this.SourceImage = new Bitmap(path);
        this.drawTextureSize = new Size(SourceImage.Width * mapPixelToTexturePixel_Multiplier, SourceImage.Height * mapPixelToTexturePixel_Multiplier);

        //  graphic libary stuff
        GraphicsDevice graphicsDevice = GameWindow.graphicsDevice;
        using RenderTarget2D renderTargetIsAOffScreenBuffer = new RenderTarget2D(graphicsDevice, drawTextureSize.Width, drawTextureSize.Height, false, SurfaceFormat.Color, DepthFormat.None);
        SpriteBatch spriteBatch = GameWindow.spriteBatch;

        //  load the textures
        //this.drawTexture = new Texture2D(Game1.graphicsDevice, size.Width, size.Height);
        //this.dirtTextures = TextureSource.LoadDirt();
        this.grassTextures = TextureSource.LoadGrass();
        this.waterTextures = TextureSource.LoadWater();
        this.dirtTextures = TextureSource.LoadDirt();
        this.stoneTextures = TextureSource.LoadStone();
        this.validTileTexture = Texture2D.FromFile(graphicsDevice, GRID_VALIDTILE_TEXTURE_PATH);
        this.inValidTileTexture = Texture2D.FromFile(graphicsDevice, GRID_INVALIDTILE_TEXTURE_PATH);

        spriteBatch.Begin();

            graphicsDevice.SetRenderTarget(renderTargetIsAOffScreenBuffer);
            for (int y = 0; y < this.SourceImage.Width; y++)
            for (int x = 0; x < this.SourceImage.Height; x++)
            {
                //Console.WriteLine($"x : {x}, y : {y}");
                TilesRGB argb = (TilesRGB)SourceImage.GetPixel(x, y).ToArgb();
                Rectangle drawRect = new Rectangle(x * mapPixelToTexturePixel_Multiplier, y * mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier);
                switch (argb)
                {
                    case TilesRGB.Dirt:
                        spriteBatch.Draw(dirtTextures[random.Next() % dirtTextures.Length], drawRect, Color.White);
                        break;
                    case TilesRGB.Grass:
                        spriteBatch.Draw(grassTextures[random.Next() % grassTextures.Length], drawRect, Color.White);
                        break;
                    case TilesRGB.Water:
                        spriteBatch.Draw(waterTextures[random.Next() % waterTextures.Length], drawRect, Color.White);
                        break;
                    case TilesRGB.Stone:
                        spriteBatch.Draw(stoneTextures[random.Next() % stoneTextures.Length], drawRect, Color.White);
                        break;
                    default:
                        Console.WriteLine("Warning not a tile");
                        break;
                }
                //Console.WriteLine(argb);
                
            }

        spriteBatch.End();

        //  annoying syntax to transfer the data from screenBuffer to the texture
        using System.IO.Stream stream = new System.IO.MemoryStream();
        renderTargetIsAOffScreenBuffer.SaveAsPng(stream, drawTextureSize.Width, drawTextureSize.Height);
        this.drawTexture = Texture2D.FromStream(graphicsDevice, stream);
        graphicsDevice.SetRenderTarget(null);   //  give back the rendering target
        Camera.Init(drawTextureSize);

    }

    private void RenderGridStatus()
    {
        
        //  graphic libary stuff
        GraphicsDevice graphicsDevice = GameWindow.graphicsDevice;
        using RenderTarget2D renderTargetIsAOffScreenBuffer = new RenderTarget2D(graphicsDevice, drawTextureSize.Width, drawTextureSize.Height, false, SurfaceFormat.Color, DepthFormat.None);
        SpriteBatch spriteBatch = GameWindow.spriteBatch;

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            graphicsDevice.SetRenderTarget(renderTargetIsAOffScreenBuffer);
            graphicsDevice.Clear(Color.Transparent);
            for (int y = 0; y < this.SourceImage.Width; y++)
            for (int x = 0; x < this.SourceImage.Height; x++)
            {
                bool isTaken = Building.grid.IsTileTaken(x, y);
                Rectangle drawRect = new Rectangle(x * mapPixelToTexturePixel_Multiplier, y * mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier);
                if (isTaken)
                {
                    spriteBatch.Draw(inValidTileTexture, drawRect, Color.White);
                }
                // else
                // {
                //     spriteBatch.Draw(validTileTexture, drawRect, Color.White);
                // }
            }
        spriteBatch.End();

        //  annoying syntax to transfer the data from screenBuffer to the texture
        using System.IO.MemoryStream stream = new System.IO.MemoryStream();
        renderTargetIsAOffScreenBuffer.SaveAsPng(stream, drawTextureSize.Width, drawTextureSize.Height);
        this.gridDrawTexture = Texture2D.FromStream(graphicsDevice, stream);
        File.WriteAllBytes("test.png",stream.ToArray());
        graphicsDevice.SetRenderTarget(null);   //  give back the rendering target

    }


    public void Draw()
    {
        Rectangle drawArea = new Rectangle(drawOffset.X, drawOffset.Y, drawTextureSize.Width, drawTextureSize.Height); 
        drawArea = Camera.ModifiedDrawArea(drawArea, Camera.zoomLevel);
        GameWindow.spriteBatch.Draw(drawTexture, drawArea, Sunlight.Mask);
        if (this.RenderGrid)
        {
            GameWindow.spriteBatch.Draw(gridDrawTexture, drawArea, Color.White);
        }


        //Console.WriteLine($"texture size : {this.drawTexture.Width}, {this.drawTexture.Height}");
    }
}