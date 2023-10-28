using System.Globalization;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using MonoGame.Extended;
using System.Diagnostics;
using System.ComponentModel;

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
    private static Texture2D[] projTexture;
    private readonly int textureId;

    private bool rotate = true;
    public bool Rotate { get => rotate; set => rotate = value; }
    private float scale = 0.15f;
    public float Scale { get => scale; set => scale = value; }

    

    static Projectile()
    {
        projTexture = new Texture2D[Directory.GetFiles("Data/Texture/Projectile/").Length];
    }

    public Projectile(int damage, int energyTransfer, float speed, Targetable target, Targetable sender, int textureId)
    {
        if (textureId < 0 || textureId > projTexture.Length)
            throw new ArgumentException($"Texture id does not match any textures : id={textureId}");

        this.damage = damage;
        this.energyTransfer = energyTransfer;
        this.speed = speed;
        this.target = target;
        this.sender = sender;
        this.position = new Vector2(sender.TargetPosition.X, sender.TargetPosition.Y);
        this.textureId = textureId;

        if (textureId != 0)
            projTexture[textureId-1] ??= Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/Texture/Projectile/{textureId}.png");
        allProjectiles.Add(this);
    }

    //  returns true if its a hit
    private bool MoveToTarget()
    {
        Vector2 destination = new(target.TargetPosition.X, target.TargetPosition.Y);
        float dx = destination.X - this.position.X;
        float dy = destination.Y - this.position.Y;
        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

        if(this.Rotate)
            this.rotation = (float)Math.Atan2(dy, dx);
        else
            this.rotation = 0;

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
        if (this.textureId != 0)
        {
            GameWindow.spriteBatch.Draw(
                projTexture[textureId-1], 
                Camera.ModifyPoint(this.position), 
                null, 
                Color.White, 
                this.rotation, 
                new Vector2(projTexture[textureId-1].Width / 2, projTexture[textureId-1].Height / 2), 
                this.scale, 
                SpriteEffects.None, 
                0f);
        }
        
    }


    
}