using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

class BuildingOption
{
    const float floatPI = (float)Math.PI;
    private readonly BuildingSelector root;
    private Point center;
    private float bgAngle;
    private float fgAngle;
    private Size bgSize;
    private Size fgSize;
    Texture2D bgTexture;
    Texture2D fgTexture;

    Resources cost;
    String buildingName = String.Empty;

    public delegate Building BuildingDelegate();
    public BuildingDelegate _buildingCallback;
    public delegate Texture2D[] TextureDelegate();
    public TextureDelegate _textureCallback;
    public delegate Rectangle RectangleDelegate(Point point);
    public RectangleDelegate _rectCallback;
    public event Action<BuildingOption> OnOver;

    public BuildingOption(
        BuildingSelector root, BuildingDelegate buildingCallback, TextureDelegate textureCallback, RectangleDelegate rectCallback,
         float angle, Resources cost, string buildingName, string foregroundTexturePath = null)
    {
        this._buildingCallback = buildingCallback;
        this._textureCallback = textureCallback;
        this._rectCallback = rectCallback;
        this.root = root;
        this.bgAngle = angle;
        this.bgSize = new Size(160, 160);
        this.fgSize = new Size(128, 128);
        this.cost = cost;
        this.state = new State(this.cost);
        this.buildingName = buildingName;

        this.fgAngle = this.bgAngle - floatPI / 2;
        var vec = new Vector2((float)Math.Cos(this.fgAngle), (float)Math.Sin(this.fgAngle)) * 185;
        this.center = root.Center + vec.ToPoint();

        // TODO replace by using ref to building texture
        this.bgTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/pentagon3_128.png");
        if (foregroundTexturePath == null)
            this.fgTexture = textureCallback()[0];
        else
            this.fgTexture = Texture2D.FromFile(GameWindow.graphicsDevice, foregroundTexturePath);
    }

    public Texture2D PlacementTexture
    {
        get
        {
            return this._textureCallback()[0];
        }
    }

    public Point Center { get => center; set => center = value; }

    private State state;

    public float Angle
    {
        get
        {
            return this.bgAngle;
        }
    }

    private void Over()
    {
        OnOver?.Invoke(this);
    }

    private class State
    {
        private bool _highlight = false;

        public bool Highlight { get => _highlight; set => _highlight = value; }

        private Resources _cost;
        // internal Resources Cost { get => _cost; set => _cost = value; }

        public State(Resources cost)
        {
            _cost = cost;
        }


        public Color Colour
        {
            get
            {
                if (!Resources.CanBuy(_cost))
                    return new Color(Color.DimGray, 0.7f);
                else if (!this._highlight)
                    return new Color(Color.White, 0.7f);
                else
                    return new Color(Color.White, 1f);

            }
        }


    }

    public void Update(MouseState mouseState)
    {
        if (root.State != BuildingSelector.EState.Visible)
            return;



        if (GetDistance(this.center.X, this.center.Y, mouseState.X, mouseState.Y) < this.Radius)
        {
            OnOver(this);
            if (!Resources.CanBuy(this.Cost))
                return;

            this.root.SelectedItem = this;
            state.Highlight = true;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine($"Distance:{GetDistance(this.center.X, this.center.Y, mouseState.X, mouseState.Y)}, R:{this.Radius}");
                Console.WriteLine(state.Colour.ToString());
            }
        }
        else
        {
            state.Highlight = false;
        }
    }

    public void Draw()
    {
        var bgDrawArea = new Rectangle(this.root.Center.X, this.root.Center.Y, this.bgSize.Width, this.bgSize.Height);
        GameWindow.spriteBatchUi.Draw(
            this.bgTexture, bgDrawArea, null, state.Colour, this.bgAngle,
            new Vector2(this.bgTexture.Width / 2, this.bgTexture.Height / 2 + 155), SpriteEffects.None, 0f);

        var fgDrawArea = new Rectangle(this.Center.X, this.Center.Y - 128, this.fgSize.Width, this.fgSize.Height * 3);
        GameWindow.spriteBatchUi.Draw(
            this.fgTexture, fgDrawArea, null, state.Colour, 0f,
            new Vector2(this.fgTexture.Width / 2, this.fgTexture.Height / 2), SpriteEffects.None, 0f);
    }

    private static double GetDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
    }

    public int Radius
    {
        get
        {
            return 70;
        }
    }

    public string BuildingName { get => buildingName; }
    internal Resources Cost { get => cost; }
}