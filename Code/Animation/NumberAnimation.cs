using System;
using System.Collections.Generic;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class NumberAnimation : Animation
{

    public static FontSystem fontSystem = null;
    public static DynamicSpriteFont font = null;

    private const int frameDuration = 2;
    private const int frames = 32;
    private const int textPxSpeed = 1;
    private const int fontPxSize = 32;

    private static Dictionary<Tuple<string, Color>, Texture2D[]> textureData = new();




    public NumberAnimation(Rectangle drawArea, string text, Color color)
        : base(GetFrames(text, color), drawArea, frameDuration)
    {

    }


    private static Texture2D[] GetFrames(string text, Color color)
    {
        if (fontSystem == null)
        {
            fontSystem = new FontSystem();
            fontSystem.AddFont(File.ReadAllBytes(@"Data/Fonts/PTC55F.ttf"));
            font = fontSystem.GetFont(fontPxSize);
        }

        if (textureData.ContainsKey(new Tuple<string, Color>(text, color)))
        {
            return textureData[new Tuple<string, Color>(text, color)];
        }

        Vector2 textSize = font.MeasureString(text);


        int width = (int)textSize.X + 2;
        int height = (int)textSize.Y + (textPxSpeed * frames);

        using RenderTarget2D renderTarget = new (GameWindow.graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
        GameWindow.graphicsDevice.SetRenderTarget(renderTarget);  

        Texture2D[] textures = new Texture2D[frames];
        for (int i = 0; i < frames; i++)
        {
            Vector2 drawPosition = new Vector2((width - textSize.X) / 2, height - textSize.Y - i * textPxSpeed);
            GameWindow.graphicsDevice.Clear(Color.Transparent);
            GameWindow.spriteBatch.Begin();
            GameWindow.spriteBatch.DrawString(font, text, drawPosition, color);
            GameWindow.spriteBatch.End();
            using MemoryStream stream = new MemoryStream();
            renderTarget.SaveAsPng(stream, width, height);
            File.WriteAllBytes($"Cache/debugg_{i}.png", stream.ToArray());
            textures[i] = Texture2D.FromStream(GameWindow.graphicsDevice, stream);
        }


        GameWindow.graphicsDevice.SetRenderTarget(null);

        textureData[new Tuple<string, Color>(text, color)] = textures;

        return textures;
    }


}