using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

interface IUpgradeableBuilding
{
    Resources? GetUpgradeCost();
    int Tier { get; }
    bool TryUpgrade();
}

abstract class UpgradeableBuilding : Building, IUpgradeableBuilding
{

    private const int textureSets = 5;
    protected const int maxTierLevel = 4;// 0-3 (4 steps)
    protected static Texture2D[][] baseTextures;

    public int Tier { get { return this.currentTier; } }
    public int MaxTierLevel { get { return UpgradeableBuilding.maxTierLevel; } }
    private int currentTier = 0;
    protected int CurrentTier { get => currentTier; }
    private readonly int textureSet;

    protected UpgradeableBuilding(string colorName, int textureSet)
        : base(Faction.Player)
    {
        this.textureSet = textureSet;
        baseTextures ??= new Texture2D[textureSets][];

        if (baseTextures[textureSet] == null)
        {
            baseTextures[textureSet] = new Texture2D[4];
            for (int i = 0; i < MaxTierLevel; i++)
                baseTextures[textureSet][i] = Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/Texture/Buildings/{colorName}-tier{i + 1}.png");
        }

        UpdateStats();
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
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][CurrentTier], DrawArea, Sunlight.Mask);
            hpBar.Draw();
        }
        base.Draw();
    }

    public abstract Resources? GetUpgradeCost();
    public virtual bool TryUpgrade()
    {
        bool result = false;

        if (currentTier < MaxTierLevel - 1)
        {
            result = Resources.BuyFor(GetUpgradeCost().Value);

            if (result)
            {
                currentTier++;
                UpdateStats();
            }
        }
        return result;
    }

    public static void Buy()
    {
        throw new NotImplementedException();
    }

    protected abstract void UpdateStats();

}