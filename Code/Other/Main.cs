
using System;
using System.Drawing;
using Microsoft.Xna.Framework;

class MainClass
{
    static int Main(string[] args)
    {
        Console.WriteLine("Start");

        //EnergyBeam energyBeam = new(new Point(8,8), new Point(1,2), EnergyBeam.Type.Line);

        // for (int i = 0; i < 4; i++)
        // {
        //     using Bitmap bitmap = new Bitmap($"Data/Texture/GemStructure/Orange_{i}.png");
        //     for (int y = 0; y < bitmap.Height; y++)
        //     for (int x = 0; x < bitmap.Width; x++)
        //     {
        //         System.Drawing.Color pixelColor = bitmap.GetPixel(x, y);
        //         bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pixelColor.A, pixelColor.G, pixelColor.B, pixelColor.R));

        //     }
        //     bitmap.Save($"Cache/Purple_{i}.png");
        // }


        using GameWindow game = new GameWindow();
        game.Run();

        Console.WriteLine("End");
        return 0;
    }

}


