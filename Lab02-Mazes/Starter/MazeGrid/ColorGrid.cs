using SixLabors.ImageSharp;

namespace MazeGrid;

public class ColorGrid : Grid
{
    private int? maximum;

    public override Distances? distances { get; set; }
    public override Distances? path { get; set; }

    public ColorGrid(int rows, int columns)
        : base(rows, columns)
    {
        includeBackgrounds = true;
    }

    public override string ContentsOf(Cell cell)
    {
        if (path != null &&
            path.ContainsKey(cell))
        {
            return path[cell].ToString().Last().ToString();
        }
        {
            return " ";
        }
    }

    public override Color BackgroundColorFor(Cell cell)
    {
        maximum = distances?.Values.Max();
        if (maximum != null &&
            distances != null &&
            distances.ContainsKey(cell))
        {
            int distance = distances[cell];
            float intensity = ((float)maximum - (float)distance) / (float)maximum;
            byte dark = Convert.ToByte(255 * intensity);
            byte bright = Convert.ToByte(128 + Convert.ToByte(127 * intensity));
            return Color.FromRgba(bright, dark, bright, 255);
        }
        else
        {
            return Color.White;
        }
    }

}
