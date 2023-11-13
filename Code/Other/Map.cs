#define SAVEPREVIEW

using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;
using System.IO;
using System.Net.NetworkInformation;



class Map
{

    public enum TilesRGB : uint
    {
        Dirt = 0xFF663931,
        Grass = 0xFF4b692F,
        Water = 0xFF5b6ee1,
        Stone = 0xFF595652
    }

    public const string PATH_MAPDATA_PREVIEW = MainMenu.PATH_MAPDATA_PREVIEW;
    public const string PATH_MAPDATA_IMAGE = MainMenu.PATH_MAPDATA;

    public const int mapPixelToGridTile_Multiplier = 1;  //  1 pixel = 2x2 tiles
    public const int mapPixelToTexturePixel_Multiplier = 16 * 2 * 2;
    public const int mapBorderLinePixelSize = 8;

    public static Rectangle hightlightGridArea = Rectangle.Empty;

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
        this.waterTextures = TextureSource.LoadCrystalClearWater();
        this.dirtTextures = TextureSource.LoadDirt();
        this.stoneTextures = TextureSource.LoadStone();

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

            
            for (int y = 1; y < this.SourceImage.Width - 1; y++)
            for (int x = 1; x < this.SourceImage.Height - 1; x++)
            {
                TilesRGB argb = (TilesRGB)SourceImage.GetPixel(x, y).ToArgb();

                if (argb == TilesRGB.Water)
                {
                    Point[] neighborGridPoints = new Point[4]{new Point(x-1,y), new Point(x+1,y),new Point(x,y-1), new Point(x,y+1)};
                    foreach (Point neighborGridPoint in neighborGridPoints)
                    {
                        argb = (TilesRGB)SourceImage.GetPixel(neighborGridPoint.X, neighborGridPoint.Y).ToArgb();
                        if (argb != TilesRGB.Water)
                        {
                            Rectangle drawRect_0 = DrawRectFromGrid(x, y);
                            Rectangle drawRect_1 = DrawRectFromGrid(neighborGridPoint);
                            
                            drawRect_0 = new Rectangle(drawRect_0.X - mapBorderLinePixelSize / 2, drawRect_0.Y - mapBorderLinePixelSize / 2, drawRect_0.Width + mapBorderLinePixelSize, drawRect_0.Height + mapBorderLinePixelSize);
                            drawRect_1 = new Rectangle(drawRect_1.X - mapBorderLinePixelSize / 2, drawRect_1.Y - mapBorderLinePixelSize / 2, drawRect_1.Width + mapBorderLinePixelSize, drawRect_1.Height + mapBorderLinePixelSize);
                            Rectangle lineArea = Rectangle.Intersect(drawRect_0, drawRect_1);
                            spriteBatch.Draw(stoneTextures[random.Next() % stoneTextures.Length], lineArea, Color.Black);
                        }

                    }

                }
                
                
            }
        spriteBatch.End();

        //  annoying syntax to transfer the data from screenBuffer to the texture
        using MemoryStream stream = new MemoryStream();
        renderTargetIsAOffScreenBuffer.SaveAsPng(stream, drawTextureSize.Width, drawTextureSize.Height);
        #if SAVEPREVIEW
            string previewPath = PATH_MAPDATA_PREVIEW + path.Substring(path.LastIndexOf('/') + 1);
            if (!File.Exists(previewPath))
                File.WriteAllBytes(previewPath, stream.ToArray());
        #endif
        this.drawTexture = Texture2D.FromStream(graphicsDevice, stream);
        graphicsDevice.SetRenderTarget(null);   //  give back the rendering target
        Camera.Init(drawTextureSize);

    }

    public void Draw()
    {
        Rectangle drawArea = new Rectangle(drawOffset.X, drawOffset.Y, drawTextureSize.Width, drawTextureSize.Height); 
        drawArea = Camera.ModifiedDrawArea(drawArea, Camera.zoomLevel);
        Console.WriteLine($"sunlight mask : {Sunlight.Mask}");
        GameWindow.spriteBatch.Draw(drawTexture, drawArea, Sunlight.Mask);
        //Console.WriteLine($"texture size : {this.drawTexture.Width}, {this.drawTexture.Height}");
    }
    public static Rectangle DrawRectFromGrid(Point gridPoint)
    {
        return DrawRectFromGrid(gridPoint.X, gridPoint.Y);
    }
    public static Rectangle DrawRectFromGrid(int x, int y)
    {
        return new Rectangle(x * mapPixelToTexturePixel_Multiplier, y * mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier);
    }
}