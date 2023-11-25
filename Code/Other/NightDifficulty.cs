using System;

static class NightDifficulty
{
    public struct DiffucultyModifier
    {
        public double healthModifier = 1.0;
        public double damageModifier = 1.0;

        public DiffucultyModifier(double healthModifier, double damageModifier)
        {
            this.healthModifier = healthModifier;
            this.damageModifier = damageModifier;
        }
    }

    public static readonly DiffucultyModifier n1 = new (1.0, 1.0);
    public static readonly DiffucultyModifier n2 = new (1.5, 1.5);
    public static readonly DiffucultyModifier n3 = new (1.8, 2.0);
    public static readonly DiffucultyModifier n4 = new (2.0, 3.0);
    public static readonly DiffucultyModifier n5 = new (2.5, 4.0);
    public static readonly DiffucultyModifier n6 = new (4.0, 6.0);
    public static readonly DiffucultyModifier n7 = new (7.5, 9.0);
    public static readonly DiffucultyModifier n8 = new (12.5, 12.5);

    public static readonly DiffucultyModifier[] night = new DiffucultyModifier[8]
    {
        n1, n2, n3, n4, n5, n6, n7, n8
    };

    public static DiffucultyModifier GetModifier(int n)
    {
        if (n > night.Length)
        {
            int extraNights = n - night.Length;
            int multiplier = 1 <<  extraNights;
            return new DiffucultyModifier(n8.healthModifier * multiplier, n8.damageModifier * multiplier);
        }
        else
        {
            return night[n];
        }
    }


}