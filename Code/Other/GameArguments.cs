using System;

struct GameArguments
{

    public enum Avatar
    {
        Wizard,
        Orb,
        Length
    }

    // using collectionBonus = 
    // public enum CollectionBonus
    // {
    //     None,
    //     Blue,
    //     Green,
    //     Purple,
    //     Orange,
    //     All,
    //     Length
    // }

    public string mapPath;
    public Avatar avatar;
    public Mineral.Type collectionBonus;

}