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
    public State state = State.SelectMap;

    private GameArguments gameArguments;

    private readonly MainMenu_Option[][] mainMenu_Options = new MainMenu_Option[((int)State.Loading)][];


    
    public MainMenu(Point windowSize)
    {
        //
        mainMenu_Options[0] = new MainMenu_Option[0];   //  will use special case buttons for this
        //

        //
        
            string[] mapImagePaths = Directory.GetFiles(Map.PATH_MAPDATA_IMAGE);
            int numberOfMaps = 0;
            foreach (string mapPath in mapImagePaths)
                if (mapPath.EndsWith(".png"))
                    numberOfMaps++;
        mainMenu_Options[1] = new MainMenu_Option[numberOfMaps];
            int mapNumber = 0;
            foreach (string mapPath in mapImagePaths)
                if (mapPath.EndsWith(".png"))
            {
                string previewMapPath = Map.PATH_MAPDATA_PREVIEW + mapPath.Substring(mapPath.LastIndexOf('/') + 1);
                mainMenu_Options[1][mapNumber] = new MainMenu_Option(windowSize, numberOfMaps, mapNumber, mapPath, previewMapPath);
                mapNumber++;
            }
        //

        //
        mainMenu_Options[2] = new MainMenu_Option[((int)GameArguments.Avatar.Length)];
            mainMenu_Options[2][0] = new MainMenu_Option(windowSize, GameArguments.Avatar.Wizard);
            mainMenu_Options[2][1] = new MainMenu_Option(windowSize, GameArguments.Avatar.Orb);
        //

        //
        mainMenu_Options[3] = new MainMenu_Option[((int)GameArguments.CollectionBonus.Length)];
            mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.None);
            mainMenu_Options[3][1] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Blue);
            mainMenu_Options[3][2] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Green);
            mainMenu_Options[3][3] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Purple);
            mainMenu_Options[3][4] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Orange);
            mainMenu_Options[3][5] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.All);
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


    bool blockGoBack = false;
    public void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if (!blockGoBack)
        {
            if (keyboardState.IsKeyDown(Keys.Escape))
                if (this.state != State.Start)
                {
                    this.EnterState(state - 1);
                    blockGoBack = true;
                }
        }
        else if (keyboardState.IsKeyUp(Keys.Escape))
            blockGoBack = false;
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
                this.gameArguments.mapPath = this.mainMenu_Options[((int)State.SelectMap)][index].path;
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
        else if (this.state == State.InActive)
            throw new Exception("Don't retrive game argument from an inactive menu. Keep it in the loading state please.");
        
        throw new Exception("Tried to retrive value from Main menu without it being picked!");
    }

    public void OnResize(Point windowSize)
    {
        foreach (MainMenu_Option[] options in this.mainMenu_Options)
            foreach (MainMenu_Option option in options)
                option.AdjustDrawArea(windowSize);
    }

    public void Draw()
    {
        if (this.state != State.Loading && this.state != State.InActive)
        foreach (MainMenu_Option option in this.mainMenu_Options[(int)this.state])
        {
            option.Draw();
        }
    }


    

}