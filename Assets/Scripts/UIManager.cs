using UnityEngine;
using UnityEngine.UI;

public class UIManager : GameEventsListener
{
    public static UIManager instance;

    public Text[] scoreTexts;

    public GameObject nextRoundButton;

    private int[] playerScores;

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
        playerScores = new int[4] { 0, 0, 0, 0};

        for (int i = 0; i < scoreTexts.Length; i++)
            scoreTexts[i].text = "Player " + i.ToString() + ": " + playerScores[i].ToString();

        base.Start();
    }

    public override void RoundEnded()
    {
        GameObject[] tanks = PlayersManager.instance.Tanks;

        int i;
        for (i = 0; i < tanks.Length; i++)
            if (tanks[i] != null)
                break;

        if (i == tanks.Length)
            i = -1;

        UpdateScores(i);
        nextRoundButton.SetActive(true);
        nextRoundButton.GetComponent<Button>().interactable = true;
    }

    public override void RoundFinished()
    {
        nextRoundButton.SetActive(false);
    }

    public void UpdateScores(int winnerIndex)
    {
        if (winnerIndex != -1)
        {
            playerScores[winnerIndex]++;
            scoreTexts[winnerIndex].text = "Player " + winnerIndex.ToString() + ": " + playerScores[winnerIndex].ToString();
        }
    }
}
