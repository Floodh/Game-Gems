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
        texture = Texture2D.FromFile(Game1.graphicsDevice, path);
        this.size = size;

    }

    public void Draw()
    {
        Game1.spriteBatch.Draw(texture, new Rectangle(0, 0, size.Width, size.Height), Color.White);
    }
}