using System;

[System.Serializable]
public struct Coord
{
    public int x;
    public int y;

    public Coord(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }

    public static Coord operator +(Coord a, Coord b) => new Coord(a.x + b.x, a.y + b.y);
}
