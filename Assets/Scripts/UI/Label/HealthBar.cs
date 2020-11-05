using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image bar;
    private void Start()
    {
        Messenger.AddListener<Stats>(GameEvents.PLAYER_LIVE_CHANGED, SetCurrentLive);
    }

    private void SetCurrentLive(Stats stats)
    {
        bar.fillAmount = stats.GetCurLife() / stats.GetMaxLife();
    }
}
