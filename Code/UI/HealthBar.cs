using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class HealthBar
{

    private static readonly Color bgColor = Color.Tan;
    private static readonly Color fillColor = Color.Green;

    private Texture2D whitePixelTexture;

    private Building building;




    public HealthBar(Building building)
    {

        this.building = building;

        this.whitePixelTexture = new(GameWindow.graphicsDevice, 1, 1);
       	whitePixelTexture.SetData(new Color[] { Color.White });


              

    }

    // public void Draw()
    // {
    //     //this.building.Hp;
    //     //this.building.MaxHp;
    //     //this.building.DrawArea

	// 	GameWindow.spriteBatch.Draw(whitePixelTexture, bgDrawArea, bgColor);
	// 	GameWindow.spriteBatch.Draw(whitePixelTexture, fillDrawArea, fillColor);             
    // }

}