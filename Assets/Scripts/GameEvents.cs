using UnityEngine;

public delegate void OnGameEvent();

public class GameEvents : GameEventsListener
{
    public static GameEvents instance;

    public OnGameEvent onRoundStartedCallback;
    public OnGameEvent onRoundPreparedCallback;
    public OnGameEvent onRoundBeganCallback;
    public OnGameEvent onRoundEndedCallback;
    public OnGameEvent onRoundFinishedCallback;

    private bool isMazeBuilt = false;
    private bool isMazeDestroyed = false;
    private bool areTanksSet = false;
    private bool isCountdownDone = false;
    private bool isWinnerLeft = false;
    private bool areAllPlayersReady = false;

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
        PlayersManager.instance.onAllPlayersReadyCallback += PlayersReady;
        PlayersManager.instance.onTanksSetCallback += TanksSet;
        PlayersManager.instance.onWinnerLeftCallback += WinnerLeft;

        MazeBuilder.instance.onMazeBuiltCallback += MazeBuilt;
        MazeBuilder.instance.onMazeDestroyedCallback += MazeDestroyed;

        base.Start();

        MazeDestroyed();
    }

    public override void RoundStarted()
    {
        Debug.Log("started");
        isMazeDestroyed = false;
    }

    public override void RoundPrepared()
    {
        isMazeBuilt = false;
        areTanksSet = false;
    }

    public override void RoundBegan()
    {
        Debug.Log("began");
        isMazeBuilt = false;
        areTanksSet = false;
    }

    public override void RoundEnded()
    {
        Debug.Log("ended");
        isWinnerLeft = false;
    }

    public override void RoundFinished()
    {
        Debug.Log("finished");
        areAllPlayersReady = false;
    }

    private void Update()
    {
        if (RoundToStart())
            onRoundStartedCallback?.Invoke();

        if (RoundToPrepare())
            onRoundPreparedCallback?.Invoke();

        if (RoundToBegin())
            onRoundBeganCallback?.Invoke();

        if (RoundToEnd())
            onRoundEndedCallback?.Invoke();

        if (RoundToFinish())
            onRoundFinishedCallback?.Invoke();
    }

    private bool RoundToStart()
    {
        if (isMazeDestroyed)
            return true;
        return false;
    }

    private bool RoundToPrepare()
    {
        return false;
    }

    private bool RoundToBegin()
    {
        if (areTanksSet && isMazeBuilt)
            return true;
        return false;
    }

    private bool RoundToEnd()
    {
        if (isWinnerLeft)
            return true;
        return false;
    }

    private bool RoundToFinish()
    {
        if (areAllPlayersReady)
            return true;
        return false;
    }

    public void TanksSet()
    {
        areTanksSet = true;
    }

    public void WinnerLeft()
    {
        isWinnerLeft = true;
    }

    public void PlayersReady()
    {
        areAllPlayersReady = true;
    }

    public void MazeBuilt()
    {
        isMazeBuilt = true;
    }

    public void MazeDestroyed()
    {
        isMazeDestroyed = true;
    }
}
