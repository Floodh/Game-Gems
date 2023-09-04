using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

class Map
{

    private Texture2D texture;
    private Size size;

    public Map(Size size, string path)
    {
        //texture = new Texture2D(Game1.graphicsDevice, size.Width, size.Height);
        Console.WriteLine("1");
        texture = Texture2D.FromFile(Game1.graphicsDevice, path);
        Console.WriteLine("2");
    }

    public void Draw()
    {
        Console.WriteLine("3");
        //Game1.spriteBatch.Draw(texture, new Rectangle(0, 0, size.Width, size.Height), Color.White);
        Console.WriteLine("4");
    }
}