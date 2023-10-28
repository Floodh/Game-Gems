using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

abstract class Bar
{

    private const int baseYoffset = Map.mapPixelToTexturePixel_Multiplier / 2;
    private const int barSectionWidth = Map.mapPixelToTexturePixel_Multiplier / 8;
    private const int barHeight = Map.mapPixelToTexturePixel_Multiplier / 8;
    private const int barBorderSize = 2;
    private const int barSectionHp = 100;
    //private const int barSize
    

    private readonly Color borderColor;
    private readonly Color fillColor;

    private static Texture2D whitePixelTexture;
    protected Targetable entity;

    private Rectangle borderDrawArea;
    private Rectangle fillDrawArea;

    private Point offset;

    private int sections;

    public Bar(Targetable building, Color fillColor, Color emptyColor, Color borderColor, Point offset)
    {
        this.fillColor = fillColor;
        this.borderColor = borderColor;
        this.entity = building;
        this.offset = offset;
        if (whitePixelTexture == null)
        {
            whitePixelTexture = new(GameWindow.graphicsDevice, 1, 1);
            whitePixelTexture.SetData(new Color[] { Color.White });
        }

    }
    public void Update()
    {
        Point center = this.entity.TargetPosition;
        center.Y += baseYoffset;

        sections = entity.MaxHp / barSectionHp;
        int width = barBorderSize * (sections + 1) + barSectionWidth * sections;
        borderDrawArea = new Rectangle(center.X - width / 2, center.Y, width, barHeight);

        // Calculate the position and size of the filled portion of the health bar
        double percentace = this.Percentace();
        int fillWidth = (int)((borderDrawArea.Width - barBorderSize * 2) * percentace);
        fillDrawArea = new Rectangle(borderDrawArea.X + barBorderSize, borderDrawArea.Y + barBorderSize, fillWidth, barHeight - barBorderSize * 2);
    }
    public void Draw()
    {
        Rectangle transformedBorderDrawArea = Camera.ModifiedDrawArea(borderDrawArea, Camera.zoomLevel);
        Rectangle transformedFillDrawArea = Camera.ModifiedDrawArea(fillDrawArea, Camera.zoomLevel);
        GameWindow.spriteBatch.Draw(whitePixelTexture, transformedBorderDrawArea, borderColor);
        GameWindow.spriteBatch.Draw(whitePixelTexture, transformedFillDrawArea, fillColor);
        for (int section = 1; section < this.sections; section++)
        {
            Rectangle sectionDividerArea = new Rectangle(this.borderDrawArea.X + section * (barBorderSize + barSectionWidth), this.borderDrawArea.Y, barBorderSize, barHeight);
            Rectangle transformedSectionDividerArea = Camera.ModifiedDrawArea(sectionDividerArea, Camera.zoomLevel);
            GameWindow.spriteBatch.Draw(whitePixelTexture, transformedSectionDividerArea, borderColor);
        }
    }

    protected abstract double Percentace();

}