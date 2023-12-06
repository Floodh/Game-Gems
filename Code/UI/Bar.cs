using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

abstract class Bar
{
    protected const int _baseYOffset = -Map.mapPixelToTexturePixel_Multiplier / 2;
    protected const int _barSectionWidth = Map.mapPixelToTexturePixel_Multiplier / 8;
    protected const int _barHeight = Map.mapPixelToTexturePixel_Multiplier / 8;
    protected const int _barSectionHp = 100;
    protected const int _barBorderSize = 2;
    private static Texture2D _whitePixelTexture;
    protected Targetable _entity;
    private Vector2 _offsetVec;
    private readonly Color _borderColor;
    private readonly Color _emptyColor;
    private readonly Color _fillColor;
    protected Vector2 _position;
    protected int _numSections;

    public Bar(Targetable building, Color fillColor, Color emptyColor, Color borderColor, Vector2 offsetVec)
    {
        _entity = building;
        _offsetVec = offsetVec;
        _fillColor = fillColor;
        _emptyColor = emptyColor;
        _borderColor = borderColor;
    }

    public Point Size
    {
        get
        {
            if (MaxUnit() <= 1600) // fix
            {
                return new Point(
                    (SectionSize.X + _barBorderSize) * _numSections + _barBorderSize * 1,
                     SectionSize.Y + _barBorderSize * 2);
            }
            else
            {
                return new Point(
                (SectionSize.X + _barBorderSize) * _numSections / 2 + _barBorderSize * 1,
                 SectionSize.Y + _barBorderSize * 2);
            }
        }
    }

    public static Point SectionSize
    {
        get { return new Point(_barSectionWidth - _barBorderSize * 0, _barHeight - _barBorderSize * 2); }
    }

    public static Texture2D GetWhiteTexture()
    {
        if (_whitePixelTexture == null)
        {
            _whitePixelTexture = new(GameWindow.graphicsDevice, 1, 1);
            _whitePixelTexture.SetData(new Color[] { Color.White });
        }
        return _whitePixelTexture;
    }


    public void Update()
    {
        Vector2 centerVec = _entity.TargetPosition.ToVector2();
        centerVec += _offsetVec + new Vector2(0, _baseYOffset); // What is _baseYOffset?
        _position = centerVec - Size.ToVector2() / 2;


        _numSections =  Math.Max(MaxUnit() / 100, 1); // fix

    }

    public void Draw()
    {
        int gaugeMaxWidth = Size.X - 2 * _barBorderSize;

        if (MaxUnit() <= 1600) // fix
        {
            int width = (int)(gaugeMaxWidth * Percentace());
            DrawBar(_position,
                width, gaugeMaxWidth, _numSections);
        }
        else // If above 1600, draw in 2 bars
        {
            int dmg = Convert.ToInt32((1 - Percentace()) * gaugeMaxWidth * 2);
            int width1 = gaugeMaxWidth * 2 - dmg;
            width1 = Math.Clamp(width1, 0, gaugeMaxWidth);
            int width2 = gaugeMaxWidth - dmg;

            DrawBar(_position,
                width1, gaugeMaxWidth, _numSections / 2);
            DrawBar(_position + new Vector2(0, Size.Y - _barBorderSize),
                width2, gaugeMaxWidth, _numSections / 2);
        }
    }

    protected void DrawBar(Vector2 position, int width, int gaugeMaxWidth, int numSections)
    {
        // Black background
        Rectangle outerRect = new(position.ToPoint(), Size);
        GameWindow.spriteBatch.Draw(GetWhiteTexture(), outerRect, _borderColor);

        // First section location
        Vector2 sectionPos = position + new Vector2(_barBorderSize, _barBorderSize);

        // Gauge background
        Point gaugeBgSize = new(gaugeMaxWidth, SectionSize.Y);
        Rectangle gaugeBgRect = new(sectionPos.ToPoint(), gaugeBgSize);
        GameWindow.spriteBatch.Draw(GetWhiteTexture(), gaugeBgRect, _emptyColor);

        // Gauge foreground (current health/energy)
        Point gaugeSize = new(width, SectionSize.Y);
        Rectangle gaugeRect = new(sectionPos.ToPoint(), gaugeSize);
        GameWindow.spriteBatch.Draw(GetWhiteTexture(), gaugeRect, _fillColor);

        // Draw divider lines
        for (int dividerIndex = 1; dividerIndex < numSections; dividerIndex++)
        {
            Vector2 from = sectionPos + new Vector2(dividerIndex * (SectionSize.X + _barBorderSize) - _barBorderSize, -_barBorderSize);
            Vector2 to = from + new Vector2(0, SectionSize.Y + _barBorderSize);
            GameWindow.spriteBatch.DrawLine(from, to, _borderColor, _barBorderSize);
        }
    }

    protected abstract double Percentace();
    protected abstract int MaxUnit();

}