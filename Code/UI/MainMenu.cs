using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class MainMenu
{
    public const string PATH_MAPDATA = "Data/MapData/";
    public const string PATH_MAPDATA_PREVIEW = "Data/MapDataPreview/";

    public enum State
    {
        Start,
        SelectMap,
        SelectAvatar,
        SelectCollcectionBonus,
        Loading,
        InActive,
    }
    private State state = State.SelectMap;

    private GameArguments gameArguments;

    private readonly MainMenu_Option[][] mainMenu_Options = new MainMenu_Option[((int)State.Loading)][];


    
    public MainMenu(Point windowSize)
    {
        //
        mainMenu_Options[0] = new MainMenu_Option[0];   //  will use special case buttons for this
        //

        //
        mainMenu_Options[1] = new MainMenu_Option[((int)GameArguments.Map.Length)];
            string[] mapImagePaths = Directory.GetFiles(Map.PATH_MAPDATA_IMAGE);
            if (mainMenu_Options.Length != mapImagePaths.Length)
                throw new Exception("Number of map images does not match the Length of the Map enum!");
            for (int i = 0; i < mapImagePaths.Length; i++)
            {
                string previewMapPath = Map.PATH_MAPDATA_PREVIEW + mapImagePaths[i].Substring(mapImagePaths[i].LastIndexOf('/') + 1);
                mainMenu_Options[1][i] = new MainMenu_Option(windowSize, (GameArguments.Map)i, previewMapPath);
            }
        //

        //
        mainMenu_Options[2] = new MainMenu_Option[((int)GameArguments.Avatar.Length)];
            mainMenu_Options[2][0] = new MainMenu_Option(windowSize, GameArguments.Avatar.Wizard);
            mainMenu_Options[2][0] = new MainMenu_Option(windowSize, GameArguments.Avatar.Orb);
        //

        //
        mainMenu_Options[3] = new MainMenu_Option[((int)GameArguments.CollectionBonus.Length)];
            mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.None);
            mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Blue);
            mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Green);
            mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Purple);
            mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Orange);
            mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.All);
        //

        this.gameArguments = new GameArguments();
    }

    private void EnterState(State newState)
    {

        switch (newState)
        {
            case State.Start:
                break;
            case State.SelectMap:
                break;
            case State.SelectAvatar:
                break;
            case State.SelectCollcectionBonus:
                break; 
            case State.Loading:
                break;
            default:
                break;                                               
        }
        this.state = newState;

    }



    public void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.Escape))
            if (this.state != State.Start)
                this.EnterState(state - 1);
    }

    MainMenu_Option possibleOption = null;

    public void UpdateByMouse(MouseState mouseState)
    {
        if (possibleOption == null)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                possibleOption = FromBounds(mouseState.Position);
            }
        }
        else
        {
            if (mouseState.LeftButton == ButtonState.Released)
            {
                if (possibleOption == FromBounds(mouseState.Position))
                if (possibleOption.valid)
                {
                    SetGameArgument(possibleOption.index);
                    this.EnterState(this.state + 1);
                    possibleOption = null;
                }
            }
        }   

    }

    private MainMenu_Option FromBounds(Point position)
    {
        foreach (MainMenu_Option option in this.mainMenu_Options[((int)this.state)])
        {
            Rectangle bounds = option.Bounds;
            if (bounds.Contains(position))
                return option;
        }   
        return null;       
    }

    private void SetGameArgument(int index)
    {
        switch (this.state)
        {
            case State.Start:
                break;
            case State.SelectMap:
                this.gameArguments.map = (GameArguments.Map)index;
                break;
            case State.SelectAvatar:
                this.gameArguments.avatar = (GameArguments.Avatar)index;
                break;
            case State.SelectCollcectionBonus:
                this.gameArguments.collectionBonus = (GameArguments.CollectionBonus)index;
                break; 
            case State.Loading:
                break;
            default:
                break;                                               
        }


    }

    public GameArguments GetGameArguments()
    {
        if (this.state == State.Loading)
            return this.gameArguments;
        
        throw new Exception("Tried to retrive value from Main menu without it being picked!");
    }

    public void Draw()
    {
        foreach (MainMenu_Option option in this.mainMenu_Options[((int)this.state)])
        {
            option.Draw();
        }
    }


    

}