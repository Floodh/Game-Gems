

using System;
using Microsoft.Xna.Framework.Graphics;

class Mineral : Building
{

    private const string Path_BaseTexture = "Data/Texture/Mineral.png";
    private Texture2D baseTexture;

    public int quantity = 10000;

    public Mineral()
        : base(Faction.Neutral)
    {
        throw new NotImplementedException("Has not yet fixed a sprite for this building!");
        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
    }

    public override Building CreateNew()
    {
        return new Mineral();
    }

    public override void Tick()
    {
        if (this.quantity == 0)
            this.Die();
    }

}