using System;
using System.IO;
using FontStashSharp;
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
    public State state {get; private set;} = State.Start;

    private GameArguments gameArguments;

    private readonly MainMenu_Option[][] mainMenu_Options = new MainMenu_Option[((int)State.Loading)][];
    private readonly MainMenu_Button startButton;
    private readonly MainMenu_Button exitButton;

    public bool shouldQuit = false;

    private Point windowSize;
    
    public MainMenu(Point windowSize)
    {
        this.windowSize = windowSize;

        //
        startButton = new MainMenu_Button(windowSize, 0, "Play");
        exitButton = new MainMenu_Button(windowSize, 1, "Quit");
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
        mainMenu_Options[3] = new MainMenu_Option[((int)GameArguments.CollectionBonus.Length) - 2];
            //mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.None);
            mainMenu_Options[3][0] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Blue);
            mainMenu_Options[3][1] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Green);
            mainMenu_Options[3][2] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Purple);
            mainMenu_Options[3][3] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.Orange);
            //mainMenu_Options[3][5] = new MainMenu_Option(windowSize, GameArguments.CollectionBonus.All);
        //

        this.gameArguments = new GameArguments();
    }

    public void EnterState(State newState)
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
        ThemePlayer.MainMenuState = newState;

    }

    bool blockGoBack = false;
    public void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if (!blockGoBack)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) | keyboardState.IsKeyDown(Keys.Back))
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
    MainMenu_Button possibleButton = null;

    public void UpdateByMouse(MouseState mouseState)
    {
        if (this.state == State.Start)
        {
            if (possibleButton == null)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    possibleButton = FromBounds_Button(mouseState.Position);
                }
            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    if (possibleButton == FromBounds_Button(mouseState.Position))
                    if (possibleButton.valid)
                    {
                        if (possibleButton == this.startButton)
                        {
                            this.EnterState(this.state + 1);
                            possibleButton = null;
                        }
                        else
                        {
                            shouldQuit = true;
                        }
                    }
                }
            }  
        }
        else
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
    private MainMenu_Button FromBounds_Button(Point position)
    {
        if (this.startButton.Bounds.Contains(position))
            return startButton;
        if (this.exitButton.Bounds.Contains(position))
            return exitButton;
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
        this.windowSize = windowSize;
        foreach (MainMenu_Option[] options in this.mainMenu_Options)
            foreach (MainMenu_Option option in options)
                option.AdjustDrawArea(windowSize);

        this.startButton.AdjustDrawArea(windowSize);
        this.exitButton.AdjustDrawArea(windowSize);
    }

    public void Draw()
    {
        if (this.state != State.Loading && this.state != State.InActive)
        {
            DynamicSpriteFont font = ResourcesUi.FontSystem.GetFont(46f);
            string text = $"Highscore night = {Save.HighscoreNight}";
            Vector2 textSize = font.MeasureString(text);
            Vector2 position = new Vector2((this.windowSize.X / 2) - (textSize.X / 2), this.startButton.Bounds.Y / 2);
            Rectangle textBgArea = new Rectangle((int)((this.windowSize.X / 2) - (textSize.X / 2)) - 10, this.startButton.Bounds.Y / 2 - 10, ((int)textSize.X) + 20, ((int)textSize.Y) + 20);

            GameWindow.spriteBatchUi.Draw(GameWindow.whitePixelTexture, textBgArea, ColorConfig.pallet0[0]);
            GameWindow.spriteBatchUi.DrawString(font, text, position, Color.Black);

            if (this.state != State.Start)
                foreach (MainMenu_Option option in this.mainMenu_Options[(int)this.state])
                {
                    option.Draw();
                }
            else
            {
                this.startButton.Draw();
                this.exitButton.Draw();
            }
        }
    }

}