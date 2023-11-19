using System.Globalization;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

static class EffectPlayer
{

    private const string Path_Plop = "Data/Audio/SoundEffects/Plop.wav";


    private const int Instances_Plop = 32;


    private static Instance[] data = new Instance[1];

    public static void Load()
    {
        data[0] = new Instance(SoundEffect.FromFile(Path_Plop), Instances_Plop, 0.2f);
    }

    public static void Play(EffectType type)
    {
        if (type > EffectType.None)
            throw new ArgumentException("Invalid Argument, needs to be an existing enum value!");
        if (type == EffectType.None)
            return;


        Instance instance = data[(int)type];
        instance.soundEffectInstances[instance.counter++].Play();
        instance.counter %=  instance.soundEffectInstances.Length;

    }


    //
        public enum EffectType
        {
            Plop,
            None,
        }
        private class Instance
        {
            public readonly SoundEffect soundEffect;
            public readonly SoundEffectInstance[] soundEffectInstances;
            public int counter;
            public Instance(SoundEffect soundEffect, int capacity, float volume)
            {
                this.soundEffect = soundEffect;
                this.soundEffectInstances = new SoundEffectInstance[capacity];
                for (int i = 0; i < Instances_Plop; i++)
                {
                    soundEffectInstances[i] = soundEffect.CreateInstance();
                    soundEffectInstances[i].Volume = volume;
                }
                this.counter = 0;
            }
        }
    //

}