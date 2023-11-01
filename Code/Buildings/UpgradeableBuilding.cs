using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

interface IUpgradeableBuilding
{
    Resources GetUpgradeCost();
    int Tier {get;}
    bool TryUpgrade();
}

abstract class UpgradeableBuilding : Building, IUpgradeableBuilding
{

    private const int textureSets = 5;
    protected const int maxTier = 4;
    protected static Texture2D[][] baseTextures;

    public int Tier{get{return this.currentTier;}}
    public int MaxTier{get{ return UpgradeableBuilding.maxTier; }}
    private int currentTier = 1;
    protected int currentTierIndex {get {return this.currentTier-1;}}
    private readonly int textureSet;

    protected UpgradeableBuilding(string colorName, int textureSet)
        : base(Faction.Player)
    {
        this.textureSet = textureSet;
        baseTextures ??= new Texture2D[textureSets][];
        
        if (baseTextures[textureSet] == null)
        {
            baseTextures[textureSet] = new Texture2D[4];
            for (int i = 0; i < maxTier; i++)
                baseTextures[textureSet][i] = Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/TextureSources/{colorName}-tier{i+1}.png");
        }
        
        UppdateStats();
    }

    public static Texture2D[] GetTextures()
    {
        throw new NotImplementedException();
    }

    public static Rectangle GetRectangle(Point point)
    {
        throw new NotImplementedException();
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][currentTierIndex], Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
            hpBar.Draw();
        }
        base.Draw();
    }

    public abstract Resources GetUpgradeCost();
    public virtual bool TryUpgrade()
    {
        bool result = false;

        if(currentTier < maxTier)
        {   
            result = Resources.BuyFor(GetUpgradeCost());

            if(result)
            {
                currentTier++;
                UppdateStats();
            }
        }
        return result;
    }

    protected abstract void UppdateStats();

}