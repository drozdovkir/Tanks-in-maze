using UnityEngine;

public class PlayersManager : GameEventsListener
{
    public static PlayersManager instance;

    public GameObject[] tankPrefabs;

    public OnGameEvent onAllPlayersReadyCallback;
    public OnGameEvent onTanksSetCallback;
    public OnGameEvent onWinnerLeftCallback;

    private bool isGameOver;
    private int tanksLeft;
    private int playersAmount;
    private int readyPlayers;
    private float timer;

    private GameObject[] tanks;

    public bool IsGameOver { get => isGameOver; }
    public GameObject[] Tanks { get => tanks; }

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
        playersAmount = 1;
        readyPlayers = 0;
        tanks = new GameObject[4];
        isGameOver = true;

        base.Start();
    }

    public override void RoundStarted()
    {
        PutTanks();
    }

    public override void RoundBegan()
    { 
        isGameOver = false;
    }

    public override void RoundEnded()
    {
        isGameOver = true;
        readyPlayers = 0;
    }

    private void Update()
    {
        if (TanksSet())
            onTanksSetCallback?.Invoke();

        if (AllPlayersReady())
            onAllPlayersReadyCallback?.Invoke();

        if (WinnerLeft())
            onWinnerLeftCallback?.Invoke();
    }

    private void PutTanks()
    {
        // destroy any tanks that left
        for (int i = 0; i < tanks.Length; i++)
            if (tanks[i] != null)
                Destroy(tanks[i]);

        for (int i = 0; i < playersAmount; i++)
        {
            Vector2 tankPosition = MazeBuilder.instance.GetRandomSpot();

            tanks[i] = Instantiate(tankPrefabs[i], tankPosition, Quaternion.identity);
            tanks[i].GetComponent<TankController>().onTankDestroyCallback += TankDestroyed;
            tanks[i].GetComponent<TankController>().onTankAppearedCallback += TankAppeared;
        }
    }

    private bool TanksSet()
    {
        if (tanksLeft == playersAmount)
            return true;
        return false;
    }

    private bool AllPlayersReady()
    {
        if (readyPlayers == playersAmount)
            return true;
        return false;
    }

    private bool WinnerLeft()
    {
        if (tanksLeft == 1)
        {
            if (timer <= 0f)
                return true;
            else
                timer -= Time.deltaTime;
        }
        else if (tanksLeft == 0)
            return true;

        return false;
    }

    public void TankAppeared()
    {
        tanksLeft++;
    }

    private void TankDestroyed()
    {
        tanksLeft--;
    }

    public void NextRoundRequest()
    {
        readyPlayers++;
    }
}
