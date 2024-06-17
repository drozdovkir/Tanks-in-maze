using UnityEngine;

public abstract class GameEventsListener : MonoBehaviour
{
    private GameEvents events;

    protected virtual void Start()
    {
        events = GameEvents.instance;

        events.onRoundStartedCallback += RoundStarted;
        events.onRoundPreparedCallback += RoundPrepared;
        events.onRoundBeganCallback += RoundBegan;
        events.onRoundEndedCallback += RoundEnded;
        events.onRoundFinishedCallback += RoundFinished;
    }

    protected virtual void OnDestroy()
    {
        events = GameEvents.instance;

        events.onRoundStartedCallback -= RoundStarted;
        events.onRoundPreparedCallback -= RoundPrepared;
        events.onRoundBeganCallback -= RoundBegan;
        events.onRoundEndedCallback -= RoundEnded;
        events.onRoundFinishedCallback -= RoundFinished;
    }

    public virtual void RoundStarted() { }
    public virtual void RoundPrepared() { }
    public virtual void RoundBegan() { }
    public virtual void RoundEnded() { }
    public virtual void RoundFinished() { }
}
