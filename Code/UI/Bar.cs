using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

abstract class Bar
{

    private static readonly Color bgColor = Color.Red;
    private readonly Color fillColor = Color.Green;

    private Texture2D whitePixelTexture;
    protected Building building;

    private Rectangle bgDrawArea;
    private Rectangle fillDrawArea;

    private Point offset;

    public Bar(Building building, Color fillColor, Point offset)
    {
        this.fillColor = fillColor;
        this.building = building;
        this.offset = offset;
        this.whitePixelTexture = new(GameWindow.graphicsDevice, 1, 1);
        whitePixelTexture.SetData(new Color[] { Color.White });

    }
    public void Update()
    {
        
        // Calculate the position and size of the health bar background
        int barWidth = building.DrawArea.Width;
        int barHeight = 5; // Adjust this value to change the height of the health bar
        int barX = building.DrawArea.X + this.offset.X;
        int barY = building.DrawArea.Y - barHeight - 2 + this.offset.Y; // Adjust the vertical position as needed

        bgDrawArea = new Rectangle(barX, barY, barWidth, barHeight);

        // Calculate the position and size of the filled portion of the health bar
        double percentace = this.Percentace();
        int fillWidth = (int)(barWidth * percentace);
        fillDrawArea = new Rectangle(barX, barY, fillWidth, barHeight);
    }
    public void Draw()
    {
        // this.building.Hp;
        // this.building.MaxHp;
        // this.building.DrawArea;
        Rectangle transformedBgDrawArea = Camera.ModifiedDrawArea(bgDrawArea, Camera.zoomLevel);
        Rectangle transformedFillDrawArea = Camera.ModifiedDrawArea(fillDrawArea, Camera.zoomLevel);

        GameWindow.spriteBatch.Draw(whitePixelTexture, transformedBgDrawArea, bgColor);
        GameWindow.spriteBatch.Draw(whitePixelTexture,transformedFillDrawArea, fillColor);
        
    }

    protected abstract double Percentace();

}