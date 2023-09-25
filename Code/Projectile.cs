using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

class Projectile
{
    public static List<Projectile> allProjectiles = new List<Projectile>();



    public static void TickAll()
    {

        foreach (Projectile projectile in allProjectiles)
        {
            //  this will make a projectile hit instantly
            projectile.target.TakeDmg(projectile);
            projectile.hasHit = true;
        }

        for (int i = 0; i < allProjectiles.Count; i++)
        {
            if (allProjectiles[i].hasHit == true)
            {
                allProjectiles.RemoveAt(i--);
            }
        }

    }

    public static void DrawAll()
    {
        foreach(Projectile proj in allProjectiles)
        {
            proj.Draw();
        }
    }
    public void Draw()
    {

        GameWindow.spriteBatch.Draw(projTexture,Camera.ModifiedDrawArea(projRectangle, Camera.zoomLevel),Color.Purple );
    }

    public Projectile(int damage, Targetable target, Targetable sender)
    {
        this.damage = damage;
        this.target = target;
        this.sender = sender;
        

        this.projTexture = new(GameWindow.graphicsDevice,1,1);
        projTexture.SetData(new Color[] {Color.White});
        projRectangle = new Rectangle(target.TargetPosition.X,target.TargetPosition.Y, 5,5);


        allProjectiles.Add(this);
    }

    //Vector2 vector2;
    public int damage;
    //double speed = 3.14;
    Targetable target;
    Targetable sender;
    bool hasHit = false;
    public static readonly Color projColor = Color.White;
    public Rectangle projRectangle;
    public Texture2D projTexture;

    
}