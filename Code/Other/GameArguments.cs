using System;

struct GameArguments
{

    public enum Avatar
    {
        Wizard,
        Orb,
        Length
    }
    public enum CollectionBonus
    {
        None,
        Blue,
        Green,
        Purple,
        Orange,
        All,
        Length
    }

    public string mapPath;
    public Avatar avatar;
    public CollectionBonus collectionBonus;

    // public string GetPathMap()
    // {
    //     switch (this.map)
    //     {
    //         case Map.TwoSides:
    //             return $"Data/MapDataPreview/{Map.TwoSides}.png";
    //         default:
    //             throw new Exception($"Can't get map path from map enum : {this.map}");
    //     }
    // }
}