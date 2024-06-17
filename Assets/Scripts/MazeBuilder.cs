using UnityEngine;

public class MazeBuilder : GameEventsListener
{
    public static MazeBuilder instance;

    public GameObject oneWallTile;
    public GameObject twoWallsTile;
    public GameObject twoWallsTile_sep;
    public GameObject threeWallsTile;
    public GameObject fourWallsTile;

    public OnGameEvent onMazeBuiltCallback;
    public OnGameEvent onMazeDestroyedCallback;

    private Vector2 topLeftCorner; // center of the top left tile of the maze in global coordinates
    private float tileSize = 8.5f;
    private float screenRatio = 9f / 16f;

    private MazeGenerator mazeGenerator;
    private Maze maze;
    private GameObject[,] builtMaze;

    public Maze Maze { get => maze; }
    public float TileSize { get => tileSize; }
    public Vector2 TopLeftCorner { get => topLeftCorner; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        base.Start();
    }

    public override void RoundStarted()
    {
        if (builtMaze != null)
            DestroyMaze();
        BuildMaze();
        onMazeBuiltCallback.Invoke(); // not right idea
    }

    public override void RoundFinished()
    {
        DestroyMaze();
        onMazeDestroyedCallback.Invoke(); // not right idea
    }

    public void BuildMaze()
    {
        maze = MazeGenerator.GenerateMaze(); // create maze as a data structure

        topLeftCorner = new Vector2(tileSize * (1f - maze.Width) / 2f, tileSize * (maze.Height - 1f) / 2f);

        builtMaze = new GameObject[maze.Height, maze.Width];

        Vector2 currentPosition = new Vector2(topLeftCorner.x, topLeftCorner.y); //shitcode

        for (int i = 0; i < maze.Height; i++)
        {
            for (int j = 0; j < maze.Width; j++)
            {
                switch (maze.contents[i, j].type_)
                {
                    case ElemType.oneWall:
                        builtMaze[i, j] = Instantiate(oneWallTile, currentPosition, 
                            Quaternion.Euler(0f, 0f, (90f * maze.contents[i, j].rotation_)));
                        break;
                    case ElemType.twoWalls:
                        builtMaze[i, j] = Instantiate(twoWallsTile, currentPosition, 
                            Quaternion.Euler(0f, 0f, (90f * maze.contents[i, j].rotation_)));
                        break;
                    case ElemType.twoWalls_sep:
                        builtMaze[i, j] = Instantiate(twoWallsTile_sep, currentPosition, 
                            Quaternion.Euler(0f, 0f, (90f * maze.contents[i, j].rotation_)));
                        break;
                    case ElemType.threeWalls:
                        builtMaze[i, j] = Instantiate(threeWallsTile, currentPosition, 
                            Quaternion.Euler(0f, 0f, (90f * maze.contents[i, j].rotation_)));
                        break;
                    case ElemType.fourWalls:
                        builtMaze[i, j] = Instantiate(fourWallsTile, currentPosition, Quaternion.identity);
                        break;
                }
                currentPosition.x += tileSize;
            }
            currentPosition.x = topLeftCorner.x;
            currentPosition.y -= tileSize;
        }

        //calibrate camera
        if (screenRatio * maze.Width >= maze.Height)
            Camera.main.orthographicSize = tileSize * (screenRatio * maze.Width + 0f) / 2f;
        else
            Camera.main.orthographicSize = tileSize * (maze.Height + 0f) / 2f;
    }

    public void DestroyMaze()
    {
        if (builtMaze == null)
            return;

        for (int i = 0; i < maze.Height; i++)
            for (int j = 0; j < maze.Width; j++)
                Destroy(builtMaze[i, j]);

        builtMaze = null;
    }

    public Vector2 GetRandomSpot()
    {
        //Random.InitState();

        Vector2 Coordinates = new Vector2(tileSize * Random.Range(0, maze.Width), -tileSize * Random.Range(0, maze.Height));
        Vector2 Position = topLeftCorner + Coordinates;

        return Position;
    }

    public (int, int) getCell(Vector2 pos)
    {
        Vector2 relativePos = pos - topLeftCorner;

        int i = (int)Mathf.Floor(-relativePos.y / tileSize);
        int j = (int)Mathf.Floor(relativePos.x / tileSize);

        return (i, j);
    }
}
