using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class HealthBar
{

    private static readonly Color bgColor = Color.Red;
    private static readonly Color fillColor = Color.Green;
    private Texture2D whitePixelTexture;
    private Building building;

    private Rectangle bgDrawArea;
    private Rectangle fillDrawArea;

    public HealthBar(Building building)
    {
        this.building = building;
        this.whitePixelTexture = new(GameWindow.graphicsDevice, 1, 1);
        whitePixelTexture.SetData(new Color[] { Color.White });

    }
    public void update()
    {
        
        // Calculate the position and size of the health bar background
        int barWidth = building.DrawArea.Width;
        int barHeight = 5; // Adjust this value to change the height of the health bar
        int barX = building.DrawArea.X;
        int barY = building.DrawArea.Y - barHeight - 2; // Adjust the vertical position as needed

        bgDrawArea = new Rectangle(barX, barY, barWidth, barHeight);

        // Calculate the position and size of the filled portion of the health bar
        float healthPercentage = (float)building.Hp / building.MaxHp;
        int fillWidth = (int)(barWidth * healthPercentage);
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

}