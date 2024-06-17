using UnityEngine;
using System.Collections.Generic;

//public enum BlockType
//{
//    Roomy,
//    Corridor
//}

//public class MazeBlock
//{
//    public int[,] mask;
//    public BlockType type_;
//    public int area;
//    public List<(int, int)> entryPoints;

//    public MazeBlock(int h, int w)
//    {
//        mask = new int[h, w];
//    }
//}

//public class PenState
//{
//    public enum PenStateType
//    {

//    }

//    private PenStateType state;

//    public void Update()
//    {

//    }

//    public void Execute()
//    {
//        switch (state):
//        {
//            case PenStateType.
//        }
//    }
//}

public class MazeGenerator
{
    public static Maze GenerateMaze()
    {
        Maze maze = new Maze(4, 5);

        maze.contents[0, 0] = new MazeElem(1, 0, 0, 1);
        maze.contents[0, 1] = new MazeElem(1, 0, 1, 0);
        maze.contents[0, 2] = new MazeElem(1, 0, 1, 1);
        maze.contents[0, 3] = new MazeElem(1, 0, 1, 0);
        maze.contents[0, 4] = new MazeElem(0, 0, 1, 1);

        maze.contents[1, 0] = new MazeElem(0, 1, 0, 1);
        maze.contents[1, 1] = new MazeElem(0, 0, 0, 1);
        maze.contents[1, 2] = new MazeElem(0, 1, 0, 1);
        maze.contents[1, 3] = new MazeElem(0, 0, 0, 1);
        maze.contents[1, 4] = new MazeElem(0, 1, 0, 1);

        maze.contents[2, 0] = new MazeElem(0, 1, 0, 1);
        maze.contents[2, 1] = new MazeElem(0, 1, 0, 1);
        maze.contents[2, 2] = new MazeElem(0, 1, 0, 0);
        maze.contents[2, 3] = new MazeElem(0, 1, 0, 1);
        maze.contents[2, 4] = new MazeElem(0, 1, 0, 1);

        maze.contents[3, 0] = new MazeElem(1, 1, 0, 0);
        maze.contents[3, 1] = new MazeElem(1, 1, 1, 0);
        maze.contents[3, 2] = new MazeElem(1, 0, 1, 0);
        maze.contents[3, 3] = new MazeElem(1, 1, 1, 0);
        maze.contents[3, 4] = new MazeElem(0, 1, 1, 0);

        return maze;
    }

    private const int lowerBound_2players = 4;
    private const int lowerBound_3players = 6;
    private const int lowerBound_4players = 8;

    private const int upperBound_2players = 12;
    private const int upperBound_3players = 15;
    private const int upperBound_4players = 18;

    //public Maze GenerateMaze1(int playersAmount)
    //{
    //    Random.InitState((int)Time.time);

    //    (int, int) sizes = GenerateSizes(playersAmount);
    //    Maze maze = new Maze(sizes.Item1, sizes.Item2);

    //    Queue<MazeBlock> mazeBlocks = new Queue<MazeBlock>();
        
    //    ExtractBlocks(maze, mazeBlocks);

    //    while (mazeBlocks.Count > 0)
    //    {

    //    }

    //    return maze;
    //}

    //public void ExtractBlocks(Maze maze_, Queue<MazeBlock> mazeBlocks_)
    //{
    //    MazeBlock currentBlock = new MazeBlock(maze_.Height, maze_.Width);
    //    int[,] mazeMask = new int[maze_.Height, maze_.Width];

    //    bool hasExtracted = true;

    //    while (hasExtracted)
    //    {
    //        (int, int) startingPoint = (-1, -1);

    //        hasExtracted = false;

    //        for (int i = 0; i < maze_.Height; i++)
    //            for (int j = 0; j < maze_.Width; j++)
    //                if ((maze_.contents[i, j] == null) && (mazeMask[i, j] == 0))
    //                {
    //                    startingPoint = (i, j);
    //                    break;
    //                }

    //        if (startingPoint != (-1, -1))
    //        {
    //            Queue<(int, int)> vertices = new Queue<(int, int)>();
    //            vertices.Enqueue(startingPoint);

    //            while (vertices.Count > 0)
    //            {
    //                (int, int) currentVertex = vertices.Dequeue();

    //                currentBlock.mask[currentVertex.Item1, currentVertex.Item2] = 1;
    //                mazeMask[currentVertex.Item1, currentVertex.Item2] = 1;

    //                for (int i = -1; i <= 1; i++)
    //                    for (int j = -1; j <= 1; j++)
    //                        if ((Mathf.Abs(i) + Mathf.Abs(j) == 1f) && 
    //                            (0 <= currentVertex.Item1 + i) && (currentVertex.Item1 + i < maze_.Height) &&
    //                            (0 <= currentVertex.Item2 + j) && (currentVertex.Item2 + j < maze_.Width) &&
    //                            (maze_.contents[currentVertex.Item1 + i, currentVertex.Item2 + j] == null) &&
    //                            (mazeMask[currentVertex.Item1 + i, currentVertex.Item2 + j] == 0))
    //                        {
    //                            vertices.Enqueue((currentVertex.Item1 + i, currentVertex.Item2 + j));
    //                        }
    //            }

    //            mazeBlocks_.Enqueue(currentBlock);
    //            hasExtracted = true;
    //        }
    //    }
    //}

    //public (int, int) GenerateSizes(int playersAmount_)
    //{
    //    int lowerBound = 0;
    //    int upperBound = 0;

    //    switch (playersAmount_)
    //    {
    //        case 2:
    //            lowerBound = lowerBound_2players;
    //            upperBound = upperBound_2players;
    //            break;
    //        case 3:
    //            lowerBound = lowerBound_3players;
    //            upperBound = upperBound_3players;
    //            break;
    //        case 4:
    //            lowerBound = lowerBound_4players;
    //            upperBound = upperBound_4players;
    //            break;
    //    }

    //    int height = Random.Range(lowerBound, upperBound);
    //    int width = Random.Range(lowerBound, upperBound);

    //    return (height, width);
    //}
}
