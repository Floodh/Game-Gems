using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

abstract class Bar
{

    protected const int baseYoffset = -Map.mapPixelToTexturePixel_Multiplier / 2;
    protected const int barSectionWidth = Map.mapPixelToTexturePixel_Multiplier / 8;
    protected const int barHeight = Map.mapPixelToTexturePixel_Multiplier / 8;
    protected const int barBorderSize = 2;
    protected const int barSectionHp = 100;
    

    private readonly Color borderColor;
    private readonly Color emptyColor;
    private readonly Color fillColor;

    private static Texture2D whitePixelTexture;
    protected Targetable entity;

    private Rectangle borderDrawArea;
    private Rectangle emptyDrawArea;
    private Rectangle fillDrawArea;

    private Point offset;

    private int sections;

    public Bar(Targetable building, Color fillColor, Color emptyColor, Color borderColor, Point offset)
    {
        this.fillColor = fillColor;
        this.emptyColor = emptyColor;
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
        center.X += this.offset.X;
        center.Y += baseYoffset + this.offset.Y;

        sections = entity.MaxHp / barSectionHp;
        sections = Math.Max(1, sections);
        int width = barBorderSize * (sections + 1) + barSectionWidth * sections;
        borderDrawArea = new Rectangle(center.X - width / 2, center.Y, width, barHeight);

        // Calculate the position and size of the filled portion of the health bar
        double percentace = this.Percentace();
        int internalWidth = (borderDrawArea.Width - barBorderSize * 2);
        int fillWidth = (int)(internalWidth * percentace);
        this.emptyDrawArea = new Rectangle(borderDrawArea.X + barBorderSize, borderDrawArea.Y + barBorderSize, internalWidth, barHeight - barBorderSize * 2);
        this.fillDrawArea = new Rectangle(borderDrawArea.X + barBorderSize, borderDrawArea.Y + barBorderSize, fillWidth, barHeight - barBorderSize * 2);
    }
    public void Draw()
    {
        Rectangle transformedBorderDrawArea = Camera.ModifiedDrawArea(borderDrawArea, Camera.zoomLevel);
        Rectangle transformedEmptyDrawArea = Camera.ModifiedDrawArea(emptyDrawArea, Camera.zoomLevel);
        Rectangle transformedFillDrawArea = Camera.ModifiedDrawArea(fillDrawArea, Camera.zoomLevel);
        GameWindow.spriteBatch.Draw(whitePixelTexture, transformedBorderDrawArea, borderColor);
        GameWindow.spriteBatch.Draw(whitePixelTexture, transformedEmptyDrawArea, emptyColor);
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