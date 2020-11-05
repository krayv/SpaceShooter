using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] Text label;

    private bool isTimerStoped = false;
    private float timeSummary;

    private void Start()
    {
        Messenger.AddListener(GameEvents.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvents.PLAYER_RESPAWNED, OnRespawnPlayer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isTimerStoped)
        {
            return;
        }
        timeSummary = Time.timeSinceLevelLoad;
        int minutes = (int)(timeSummary / 60f);
        int seconds = (int)(timeSummary % 60f);
        string minutesString = string.Format("{0:00}", minutes);
        string secondsString = string.Format("{0:00}", seconds);
        label.text = minutesString + ":" + secondsString;
    }

    private void OnPlayerDead()
    {
        isTimerStoped = true;
    }

    private void OnRespawnPlayer()
    {
        isTimerStoped = false;
    }
}
