using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;
using FontStashSharp;

public class UpgradeButton
{
    private readonly Texture2D _texture;
    private readonly Point _textureSize;
    private Vector2 _offsetVec;
    private Rectangle _rect;
    protected bool _mousePressed = false;
    protected bool _mouseOver = false;

    public UpgradeButton(Vector2 offsetVec)
    {
        _texture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/UpgradeBuilding/glow.png");;
        _textureSize = new Point(this._texture.Width/2, this._texture.Height/2);
        _offsetVec = offsetVec;
    }

    public bool Update(MouseState mouseState, Vector2 vector)
    {
        // Clicked = (MouseState.LeftButton == ButtonState.Pressed) && (LastMouseState.LeftButton == ButtonState.Released);
        vector = vector - new Vector2(_textureSize.X/2, _textureSize.Y/2) + _offsetVec;
        _rect = new Rectangle(vector.ToPoint(), _textureSize);

        if(this._rect.Contains(mouseState.Position))
        {
            _mouseOver = true;
            this.Over();
            if(mouseState.LeftButton == ButtonState.Released && this._mousePressed == false)
            {
                // Console.WriteLine("Hover ContextMenu");
            }
            else if(mouseState.LeftButton == ButtonState.Pressed)
            {
                this._mousePressed = true;
            }
            else if(mouseState.LeftButton == ButtonState.Released && this._mousePressed == true)
            {
                this._mousePressed = false;
                this.Click();
            }
            return true;
        }

        this._mousePressed = false;
        _mouseOver = false;
        return false;
    }

    public event EventHandler OnClick;
    public event EventHandler OnOver;

    private void Click()
    {
        OnClick?.Invoke(this, EventArgs.Empty);
    }

    private void Over()
    {
        OnOver?.Invoke(this, EventArgs.Empty);
    }

    public void Draw()
    {
        if(_mouseOver)
        {
            GameWindow.spriteBatch.Draw(
                    _texture, _rect, null, new Color(Color.White, 1f), 0f, 
                    new Vector2(0, 0), SpriteEffects.None, 0f);
        }
        
    }
}