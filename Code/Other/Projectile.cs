using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

class Projectile
{
    public static List<Projectile> allProjectiles = new List<Projectile>();



    public static void TickAll()
    {

        foreach (Projectile projectile in allProjectiles)
        {
            //  this will make a projectile hit instantly
            if (projectile.MoveToTarget())
            {
                projectile.target.TakeDmg(projectile);
                projectile.hasHit = true;
            }

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


    private Vector2 position;
    public readonly int damage;
    public readonly int energyTransfer;
    private readonly float speed;
    private float rotation;
    Targetable target;
    Targetable sender;
    private bool hasHit = false;
    private static Texture2D projTexture;

    public Projectile(int damage, int energyTransfer, float speed, Targetable target, Targetable sender)
    {
        this.damage = damage;
        this.energyTransfer = energyTransfer;
        this.speed = speed;
        this.target = target;
        this.sender = sender;
        this.position = new Vector2(sender.TargetPosition.X, sender.TargetPosition.Y);

        projTexture ??= Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/Bolt2.png");
        allProjectiles.Add(this);
    }

    //  returns true if its a hit
    private bool MoveToTarget()
    {
        Vector2 destination = new(target.TargetPosition.X, target.TargetPosition.Y);
        float dx = destination.X - this.position.X;
        float dy = destination.Y - this.position.Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);
        this.rotation = (float)Math.Atan2(dy, dx);


        if (distance < speed)
        {
            this.position = destination;
            return true;
        }
        else
        {
            float moveX = (dx / distance) * speed; 
            float moveY = (dy / distance) * speed; 
            this.position = new Vector2(this.position.X + moveX, this.position.Y + moveY);
            return false;
        }

    }

    public void Draw()
    {

        float scale = 0.15f;



        GameWindow.spriteBatch.Draw(
            projTexture, 
            Camera.ModifyPoint(this.position), 
            null, 
            Color.White, 
            rotation, 
            new Vector2(projTexture.Width / 2, projTexture.Height / 2), 
            scale, 
            SpriteEffects.None, 
            0f);
    }


    
}