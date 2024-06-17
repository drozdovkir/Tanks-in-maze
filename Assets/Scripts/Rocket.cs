using UnityEngine;
using System.Collections.Generic;

class MazeGridNode
{
    private Vector2 pos;
    private bool[] adjacents;

    public Vector2 Pos { get => pos; }
    public bool[] Adjacents { get => adjacents; }

    public MazeGridNode(Vector2 p, bool[] adj)
    {
        pos = new Vector2(p.x, p.y);
        adjacents = new bool[4] { adj[0], adj[1], adj[2], adj[3] };
    }
}

class MazeGrid
{
    private float size;

    private MazeGridNode[,] nodes;

    public MazeGridNode[,] Nodes { get => nodes; }

    public MazeGrid(float size_, Vector2 topLeftCorner, int height, int width)
    {
        size = size_;
        nodes = new MazeGridNode[height, width];

        Vector2 currentPosition = new Vector2(topLeftCorner.x, topLeftCorner.y); //shitcode

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                bool up = false;
                bool down = false;
                bool right = false;
                bool left = false;

                RaycastHit2D hitUp = Physics2D.Raycast(currentPosition, Vector2.up, size);
                if (hitUp.collider == null)
                    up = true;

                RaycastHit2D hitDown = Physics2D.Raycast(currentPosition, Vector2.down, size);
                if (hitDown.collider == null)
                    down = true;

                RaycastHit2D hitLeft = Physics2D.Raycast(currentPosition, Vector2.left, size);
                if (hitLeft.collider == null)
                    left = true;

                RaycastHit2D hitRight = Physics2D.Raycast(currentPosition, Vector2.right, size);
                if (hitRight.collider == null)
                    right = true;

                nodes[i, j] = new MazeGridNode(currentPosition, new bool[4] { up, down, left, right } );

                currentPosition.x += size;
            }
            currentPosition.x = topLeftCorner.x;
            currentPosition.y -= size;
        }
    }

    public List<(int, int)> GetAdjacents((int, int) node)
    {
        List<(int, int)> result = new List<(int, int)>();

        if (nodes[node.Item1, node.Item2].Adjacents[0])
            result.Add((node.Item1 + 1, node.Item2));

        if (nodes[node.Item1, node.Item2].Adjacents[1])
            result.Add((node.Item1 - 1, node.Item2));

        if (nodes[node.Item1, node.Item2].Adjacents[2])
            result.Add((node.Item1, node.Item2 - 1));

        if (nodes[node.Item1, node.Item2].Adjacents[3])
            result.Add((node.Item1, node.Item2 + 1));

        return result;
    }
}

public enum RocketState
{
    Idle,
    DistantFollow,
    CloseFollow
}

class Rocket : MonoBehaviour
{
    public float speed;
    public float sleepTime;

    private Rigidbody2D rigidBody2D;

    private RocketState rocketState;
    private Maze maze;
    private MazeGrid grid;
    private int[,] mazeDistances;
    private GameObject[] tanks;
    private (int, int)?[] tankCells;
    private (int, int) rocketCell;
    private bool rocketMoved, tankMoved, tankExploded;
    private float timer;

    private Queue<Vector2> path;
    private Vector2 currentTarget;

    private void Start()
    {
        maze = MazeBuilder.instance.Maze;
        tanks = PlayersManager.instance.Tanks;

        mazeDistances = new int[maze.Height, maze.Width];

        tankCells = new (int, int)?[4];

        path = new Queue<Vector2>();

        rigidBody2D = GetComponent<Rigidbody2D>();

        rocketState = RocketState.Idle;
        timer = sleepTime;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (rocketState == RocketState.Idle)
        {
            if (timer <= 0f)
            {
                timer = 100f;
                rocketState = RocketState.DistantFollow;
                BuildGrid();
            }
            timer -= Time.deltaTime;
            return;
        }

        bool toUpdate = false;

        toUpdate = CheckForUpdate();

        if (toUpdate)
            UpdatePath();

        if (Vector2.Distance(transform.position, currentTarget) < Mathf.Epsilon)
        {
            currentTarget = path.Dequeue();
            if (path.Count == 0)
            {
                rocketState = RocketState.CloseFollow;
                BuildGrid();
            }
            UpdatePath();
        }

        rigidBody2D.MovePosition(transform.position + transform.up * speed * Time.deltaTime);
        //rigidBody2D.MoveRotation();
    }

    private bool CheckForUpdate()
    {
        bool res = false;

        rocketMoved = false;
        tankMoved = false;
        tankExploded = false;

        if (rocketState == RocketState.CloseFollow)
            res = true;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i] != null)
            {
                (int, int) position = MazeBuilder.instance.getCell(tanks[i].transform.position);
                if (position != tankCells[i])
                {
                    tankCells[i] = position;
                    tankMoved = true;
                    res = true;
                }
            }
            else if (tankCells[i] != null)
            {
                tankCells[i] = null;
                tankExploded = true;
                res = true;
            }
        }

        (int, int) rPosition = MazeBuilder.instance.getCell(gameObject.transform.position);
        if (rPosition != rocketCell)
        {
            rocketMoved = true;
            res = true;
        }

        return res;
    }

    private void UpdatePath()
    {
        if (rocketState == RocketState.DistantFollow)
        {
            if (rocketMoved)
            {
                WaveSearch(grid, rocketCell);
            }
        }
        else if (rocketState == RocketState.CloseFollow)
            AStarSearch();
    }

    private void WaveSearch(MazeGrid grid_, (int, int) start)
    {
        for (int i = 0; i < mazeDistances.GetLength(0); i++)
            for (int j = 0; j < mazeDistances.GetLength(1); j++)
                mazeDistances[i, j] = -1;

        int currentDistance = 0;
        mazeDistances[start.Item1, start.Item2] = 0;

        List<(int, int)> newCellLayer = new List<(int, int)>();
        List<(int, int)> currentCellLayer = new List<(int, int)>();

        currentCellLayer.Add(start);

        while (currentCellLayer.Count != 0)
        {
            currentDistance++;
            newCellLayer.Clear();

            foreach (var cell in currentCellLayer)
            {
                mazeDistances[cell.Item1, cell.Item2] = currentDistance;
                List<(int, int)> adjecents = grid_.GetAdjacents(cell);
                foreach (var c in adjecents)
                {
                    if (mazeDistances[c.Item1, c.Item2] == -1)
                        newCellLayer.Add(c);
                }
            }

            var buffer = currentCellLayer;
            currentCellLayer = newCellLayer;
            newCellLayer = buffer;
        }
    }

    private void AStarSearch()
    {

    }

    private void BuildGrid()
    {
        float size_;
        Vector2 topLeftCorner;
        int height;
        int width;

        if (rocketState == RocketState.DistantFollow)
        {
            size_ = MazeBuilder.instance.TileSize;
            topLeftCorner = MazeBuilder.instance.TopLeftCorner;
            height = MazeBuilder.instance.Maze.Height;
            width = MazeBuilder.instance.Maze.Width;
        }
        else
        {
            size_ = MazeBuilder.instance.TileSize;
            topLeftCorner = MazeBuilder.instance.TopLeftCorner;
            height = MazeBuilder.instance.Maze.Height;
            width = MazeBuilder.instance.Maze.Width;
        }

        grid = new MazeGrid(size_, topLeftCorner, height, width);
    }

    private Vector2 GetClosestTank(GameObject[] tanks_)
    {
        Vector2 result = new Vector2(0f, 0f);
        float minDist = Mathf.Infinity;
        float curDist = 0f;

        for (int i = 0; i < tanks_.Length; i++)
            if (tanks_[i] != null)
            {
                curDist = Vector2.Distance(transform.position, tanks_[i].transform.position);
                if (curDist < minDist)
                {
                    minDist = curDist;
                    result = tanks_[i].transform.position;
                }
            }

        return result;
    }
}