struct GameArguments
{
    public enum Map
    {
        TwoSides,
        Circle,
        Length
    }

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

    public Map map;
    public Avatar avatar;
    public CollectionBonus collectionBonus;
}