using UnityEngine;

public class ItemLocater : GameEventsListener
{
    public static ItemLocater instance;

    public GameObject[] items;

    public float frequency;
    public int maxItemAmount;

    private bool[,] placedItems;

    private float timeStamp;
    private int itemsCount;
    private bool isWorking;

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
        timeStamp = frequency;
        itemsCount = 0;

        isWorking = false;

        base.Start();
    }

    public override void RoundBegan()
    {
        int N = MazeBuilder.instance.Maze.Height;
        int M = MazeBuilder.instance.Maze.Width;
        placedItems = new bool[N, M];

        isWorking = true;
    }

    public override void RoundEnded()
    {
        isWorking = false;
    }

    public override void RoundFinished()
    {
        timeStamp = frequency;
        itemsCount = 0;
    }

    private void Update()
    {
        if (!isWorking)
            return;

        timeStamp -= Time.deltaTime;

        if (timeStamp <= 0f)
        {
            if (itemsCount < maxItemAmount)
                PutItem();
            timeStamp = frequency;
        }
    }

    private void PutItem()
    {
        int index = Random.Range(0, items.Length);

        Vector2 itemPosition;
        (int, int) itemCell;

        do
        {
            itemPosition = MazeBuilder.instance.GetRandomSpot();
            Debug.Log("itemPosition " + itemPosition.ToString());
            itemCell = MazeBuilder.instance.getCell(itemPosition);
            Debug.Log("itemCell " + itemCell.ToString());
        }
        while (placedItems[itemCell.Item1, itemCell.Item2]);

        placedItems[itemCell.Item1, itemCell.Item2] = true;

        GameObject newItem = Instantiate(items[index], itemPosition, Quaternion.identity);
        newItem.GetComponent<Item>().onItemPickCallback += ItemPicked;

        itemsCount++;
    }

    public void ItemPicked()
    {
        itemsCount--;
        timeStamp = frequency; //?????
    }
}
