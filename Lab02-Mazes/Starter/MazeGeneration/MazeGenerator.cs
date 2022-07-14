using Algorithms;
using MazeGrid;
using SixLabors.ImageSharp;

namespace MazeGeneration;

public class MazeGenerator
{
    private Grid mazeGrid;
    private bool isGenerated = false;

    public MazeGenerator()
    {
        mazeGrid = new ColorGrid(15, 15);
    }

    public void GenerateMaze()
    {
        var algorithm = new BinaryTree();
        algorithm?.CreateMaze(mazeGrid);
        isGenerated = true;
    }

    public string GetTextMaze(bool includePath = false)
    {
        if (!isGenerated)
            GenerateMaze();

        if (includePath)
        {
            Cell start = mazeGrid.GetCell(mazeGrid.Rows / 2, mazeGrid.Columns / 2);
            mazeGrid.path = start.GetDistances().PathTo(mazeGrid.GetCell(mazeGrid.Rows - 1, 0));
        }

        return mazeGrid.ToString();
    }

    public Image GetGraphicalMaze(bool includeHeatMap = false)
    {
        if (!isGenerated)
            GenerateMaze();

        if (includeHeatMap)
        {
            Cell start = mazeGrid.GetCell(mazeGrid.Rows / 2, mazeGrid.Columns / 2);
            mazeGrid.distances = start.GetDistances();
        }
        return mazeGrid.ToPng(30);
    }
}
