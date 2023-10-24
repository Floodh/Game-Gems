using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;




class Ray
{
    private ParticleEffect _particleEffect;
    private Texture2D _particleTexture;
    private TextureRegion2D _textureRegion;
    private Color _color;
    public Vector2 _source = new(0, 0);
    public Vector2 _target = new(0,0);
    private bool _active = false;


    public Ray()
    {

        _color = new Color(Color.LightGreen, 10);
        _particleTexture = new Texture2D(GameWindow.graphicsDevice, 1, 1);
        _particleTexture.SetData(new[] { Color.White });

        _textureRegion = new TextureRegion2D(_particleTexture);

        this.Init();
    }

    public void Init()
    {
        _particleEffect = new ParticleEffect(autoTrigger: false)
        {
            // Position = new Vector2(200, 0),
            Position = new Vector2(200, 100),
            Emitters = new List<ParticleEmitter>
            {
                new ParticleEmitter(_textureRegion, 500, TimeSpan.FromSeconds(1),
                    // Profile.BoxUniform(100,250))
                    //Profile.Line(new Vector2(1, 0), 5))
                    Profile.Line(new Vector2(2, 2), 75)) // Direction and lenght
                {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(0f, 50f),
                        Quantity = 30,//3
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(3.0f, 4.0f)
                    },
                    Modifiers =
                    {
                        new AgeModifier
                        {
                            Interpolators =
                            {
                                new ColorInterpolator
                                {
                                    // StartValue = new HslColor(0.33f, 0.5f, 0.5f),
                                    // EndValue = new HslColor(0.5f, 0.9f, 1.0f)
                                    StartValue = _color.ToHsl(),
                                    EndValue = _color.ToHsl()
                                }
                            }
                        },
                        new RotationModifier {RotationRate = -2.1f},
                        // new RectangleContainerModifier {Width = 800, Height = 480},
                        new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
                    }
                }
            }
        };
    }

    private ParticleEmitter Test()
    {
        return new ParticleEmitter(_textureRegion, 500, TimeSpan.FromSeconds(1),
                    Profile.Line(this._target, 1))
                {
                    Parameters = new ParticleReleaseParameters
                    {
                        Speed = new Range<float>(0f, 50f),
                        Quantity = 30,//3
                        Rotation = new Range<float>(-1f, 1f),
                        Scale = new Range<float>(3.0f, 4.0f)
                    },
                    Modifiers =
                    {
                        new AgeModifier
                        {
                            Interpolators =
                            {
                                new ColorInterpolator
                                {
                                    // StartValue = new HslColor(0.33f, 0.5f, 0.5f),
                                    // EndValue = new HslColor(0.5f, 0.9f, 1.0f)
                                    StartValue = _color.ToHsl(),
                                    EndValue = _color.ToHsl()
                                }
                            }
                        },
                        new RotationModifier {RotationRate = -2.1f},
                        // new RectangleContainerModifier {Width = 800, Height = 480},
                        new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f},
                    }
                };
    }

    public void UnloadContent()
    {
        _particleTexture.Dispose();
        _particleEffect.Dispose();
    }

    public void UpdateByMouse(MouseState mouseState)
    {   
        if(mouseState.LeftButton == ButtonState.Pressed)
        {
            this._source = new Vector2(mouseState.X, mouseState.Y);
            this._particleEffect.Position = this._source;
        }

        if(mouseState.RightButton == ButtonState.Pressed)
        {
            Vector2 mouse = new Vector2(mouseState.X, mouseState.Y);
            
            this._target = this._source - mouse;
            this._particleEffect.Emitters[0] = Test();
        }
    }

    public void UpdateByKeyboard(KeyboardState keyboardState)
    {  
        _active = keyboardState.IsKeyDown(Keys.R)?true:false;
    }

    public void Update(GameTime gameTime)
    {
        this._particleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void Draw()
    {
        if(!_active)
            return;

        GameWindow.spriteBatch.Draw(_particleEffect);
    }

}