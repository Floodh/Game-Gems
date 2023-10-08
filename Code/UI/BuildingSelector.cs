using System;
using System.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Size = System.Drawing.Size;

class BuildingSelector
{

    public delegate void UserPlacementOrder(Point selectedPoint, Building.Type building);
    public UserPlacementOrder placementOrderResponse = null;

    public BuildingSelector(Texture2D[] textures, Size windowSize)
    {
        throw new NotImplementedException();
    }


    public void UpdateByKeyboard(KeyboardState keyboardState)
    {
        //keyboardState.IsKeyUp(Keys.RightShift);
        throw new NotImplementedException();
    }

    public static void UpdateByMouse(MouseState mouseState)
    {
        throw new NotImplementedException();
    }
}