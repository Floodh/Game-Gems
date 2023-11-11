using System;
using System.Globalization;
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

static class ThemePlayer
{

    private const string Path_MainMenuTheme_Folder = "Data/Audio/Music_MainMenu/";
    private const string Path_MainMenuTheme_1 = $"{Path_MainMenuTheme_Folder}/TestEnviroment_1.wav";
    private const string Path_MainMenuTheme_2 = $"{Path_MainMenuTheme_Folder}/TestEnviroment_2.wav";
    private const string Path_MainMenuTheme_3 = $"{Path_MainMenuTheme_Folder}/TestEnviroment_3.wav";
    private const string Path_MainMenuTheme_4 = $"{Path_MainMenuTheme_Folder}/TestEnviroment_4.wav";
    private const string Path_MainMenuTheme_Master = $"{Path_MainMenuTheme_Folder}/TestEnviroment_1.wav";
    


    //  load 4 sound effect from .wav files
    //  contruct the theme from these

    static SoundEffect soundEffect_1;
    static SoundEffect soundEffect_2;
    static SoundEffect soundEffect_3;
    static SoundEffect soundEffect_4;

    static readonly Thread thread = new Thread(PlayTheme_MainMenu);

    public static MainMenu.State MainMenuState {get; set;}

    public static volatile bool shouldQuit = false;





    public static void Load()
    {
        soundEffect_1 = SoundEffect.FromFile(Path_MainMenuTheme_1);
        soundEffect_2 = SoundEffect.FromFile(Path_MainMenuTheme_2);
        soundEffect_3 = SoundEffect.FromFile(Path_MainMenuTheme_3);
        soundEffect_4 = SoundEffect.FromFile(Path_MainMenuTheme_4);
    }

    public static void Start_PlayTheme_MainMenu(MainMenu.State mainMenuState = MainMenu.State.Start)
    {
        MainMenuState = mainMenuState;
        thread.Start();
    }

    private static SoundEffectInstance soundEffectInstance_1;
    private static SoundEffectInstance soundEffectInstance_2;
    private static SoundEffectInstance soundEffectInstance_3;
    private static SoundEffectInstance soundEffectInstance_4;
        
    private static void PlayTheme_MainMenu()
    {

        soundEffectInstance_1 = soundEffect_1.CreateInstance();
        soundEffectInstance_2 = soundEffect_2.CreateInstance();
        soundEffectInstance_3 = soundEffect_3.CreateInstance();
        soundEffectInstance_4 = soundEffect_4.CreateInstance();

        soundEffectInstance_1.Volume = 0.5f;
        soundEffectInstance_2.Volume = 0.0f;
        soundEffectInstance_3.Volume = 0.0f;
        soundEffectInstance_4.Volume = 0.0f;

        // soundEffectInstance_1.IsLooped = true;
        // soundEffectInstance_2.IsLooped = true;
        // soundEffectInstance_3.IsLooped = true;
        // soundEffectInstance_4.IsLooped = true;

        soundEffectInstance_1.Play();
        soundEffectInstance_2.Play();
        soundEffectInstance_3.Play();
        soundEffectInstance_4.Play();

        while (MainMenuState != MainMenu.State.InActive && shouldQuit == false)
        {

            switch (MainMenuState)
            {
                case MainMenu.State.Start:
                    soundEffectInstance_1.Volume = 0.5f;
                    soundEffectInstance_2.Volume = 0.0f;
                    soundEffectInstance_3.Volume = 0.0f;
                    soundEffectInstance_4.Volume = 0.0f;  
                    break;
                case MainMenu.State.SelectMap:
                    soundEffectInstance_1.Volume = 0.5f;
                    soundEffectInstance_2.Volume = 0.5f;
                    soundEffectInstance_3.Volume = 0.0f;
                    soundEffectInstance_4.Volume = 0.0f;  
                    break;
                case MainMenu.State.SelectAvatar:
                    soundEffectInstance_1.Volume = 0.5f;
                    soundEffectInstance_2.Volume = 0.5f;
                    soundEffectInstance_3.Volume = 0.5f;
                    soundEffectInstance_4.Volume = 0.0f;  
                    break;
                case MainMenu.State.SelectCollcectionBonus:
                    soundEffectInstance_1.Volume = 0.5f;
                    soundEffectInstance_2.Volume = 0.5f;
                    soundEffectInstance_3.Volume = 0.5f;
                    soundEffectInstance_4.Volume = 0.5f;  
                    break;                                        
                case MainMenu.State.Loading:
                    soundEffectInstance_1.Volume = 0.0f;
                    soundEffectInstance_2.Volume = 0.5f;
                    soundEffectInstance_3.Volume = 0.5f;
                    soundEffectInstance_4.Volume = 0.5f;  
                    break;
                case MainMenu.State.InActive:
                    soundEffectInstance_1.Volume = 0.0f;
                    soundEffectInstance_2.Volume = 0.0f;
                    soundEffectInstance_3.Volume = 0.0f;
                    soundEffectInstance_4.Volume = 0.5f;  
                    break;
                default:
                    break;                  
            }

            if (soundEffectInstance_4.State != SoundState.Playing)
            {
                soundEffectInstance_1.Stop();
                soundEffectInstance_2.Stop();
                soundEffectInstance_3.Stop();
                soundEffectInstance_4.Stop();

                soundEffectInstance_1.Play();
                soundEffectInstance_2.Play();
                soundEffectInstance_3.Play();
                soundEffectInstance_4.Play();
            }

            Thread.Sleep(0);

        }

        soundEffectInstance_1.Dispose();
        soundEffectInstance_2.Dispose();
        soundEffectInstance_3.Dispose();
        soundEffectInstance_4.Dispose();

    }


    public static void Dispose(object sender, EventArgs e)
    {
        shouldQuit = true;       
    }

}