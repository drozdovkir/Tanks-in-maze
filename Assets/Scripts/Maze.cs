public enum ElemType
{
    zeroWall,
    oneWall,
    twoWalls,
    threeWalls,
    fourWalls,
    twoWalls_sep
}

public class MazeElem
{
    public ElemType type_;
    public int rotation_; //counting in pi/2

    private int left;
    private int up;
    private int right;
    private int down;

    public MazeElem()
    {
        left = up = right = down = 0;
        type_ = ElemType.zeroWall;
        rotation_ = 0;
    }

    public MazeElem(int r, int u, int l, int d)
    {
        left = l;
        up = u;
        right = r;
        down = d;

        type_ = (ElemType)(4 - (left + right + up + down));

        if ((type_ == ElemType.twoWalls) && ((left + right == 2) || (up + down == 2)))
            type_ = ElemType.twoWalls_sep;

        if ((down == 1) && (right == 0))
            rotation_ = 0;
        if ((right == 1) && (up == 0))
            rotation_ = 1;
        if ((up == 1) && (left == 0))
            rotation_ = 2;
        if ((left == 1) && (down == 0))
            rotation_ = 3;
    }
}

public class Maze
{
    private int height;
    private int width;

    public MazeElem[,] contents;

    public int Height { get => height; }
    public int Width { get => width; }

    public Maze(int h, int w)
    {
        height = h;
        width = w;

        contents = new MazeElem[height, width];
    }
}
