﻿using Algorithms;
using MazeGrid;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using SixLabors.ImageSharp;
using MazeGeneration;

namespace maze_web.Controllers;

public class MazeController : Controller
{
    private readonly ILogger<MazeController> _logger;

    public MazeController(ILogger<MazeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int size, string algo, MazeColor color)
    {
        // return Content($"{size}, {algo}, {color}");

        int mazeSize = GetSize(size);
        IMazeAlgorithm algorithm = GetAlgorithm(algo);
        Image mazeImage = Generate(mazeSize, algorithm, color);
        byte[] byteArray = ConvertToByteArray(mazeImage);
        return File(byteArray, "image/png");
    }

    public int GetSize(int size)
    {
        int mazeSize = 15;
        if (size > 0)
        {
            mazeSize = size;
        }
        return mazeSize;
    }

    public IMazeAlgorithm GetAlgorithm(string algo)
    {
        IMazeAlgorithm? algorithm = new RecursiveBacktracker();
        if (!string.IsNullOrEmpty(algo))
        {
            Assembly? assembly = Assembly.GetAssembly(typeof(RecursiveBacktracker));
            Type? algoType = assembly?.GetType($"Algorithms.{algo}", false, true);
            if (algoType != null)
            {
                algorithm = Activator.CreateInstance(algoType) as IMazeAlgorithm;
            }
        }
        return algorithm!;
    }

    public Image Generate(int mazeSize, IMazeAlgorithm algorithm, MazeColor color)
    {
        IMazeGenerator generator =
            new MazeGenerator(
                new ColorGrid(mazeSize, mazeSize, color),
                algorithm);

        _logger.LogDebug($"{DateTime.Now:G} - Starting maze generation");
        generator.GenerateMaze();
        _logger.LogDebug($"{DateTime.Now:G} - Ending maze generation");

        _logger.LogDebug($"{DateTime.Now:G} - Starting image generation");
        Image maze = generator.GetGraphicalMaze(true);
        _logger.LogDebug($"{DateTime.Now:G} - Ending image generation");
        return maze;
    }

    public static byte[] ConvertToByteArray(Image img)
    {
        using var stream = new MemoryStream();
        img.SaveAsPng(stream);
        return stream.ToArray();
    }
}
