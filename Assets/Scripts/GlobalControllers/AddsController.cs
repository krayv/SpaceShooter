using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class AddsController : MonoBehaviour
{
    [SerializeField] GameStats gameStats;
    [SerializeField] Button DoubleRewardButton;
    [SerializeField] Button RespawnButton;

    public void Start()
    {
        if(Advertisement.isSupported)
        {
            Advertisement.Initialize("3878071", false);
        }
    }

    public void ShowRewardAdd()
    {
        if(Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo");
            gameStats.OnWatchRewardAdds();
            Destroy(DoubleRewardButton.gameObject);

        }
    }

    public void ShowRespawnPlayerAdd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo");
            Messenger.Broadcast(GameEvents.PLAYER_RESPAWNED);
            Destroy(RespawnButton.gameObject);
        }
    }
}
