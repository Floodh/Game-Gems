using System.Collections.Generic;

class Grid
{

    private List<Building> buildings;
    private bool[][] isTaken;

    public bool IsTileTaken(int x, int y)
    {
        return isTaken[y][x];
    }

    public bool PlaceIfPossible(int x, int y, Building building)
    {

        

        return true;
    }

}