using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] Image bar;
    private void Start()
    {
        Messenger.AddListener<Stats>(GameEvents.BOSS_LIVE_CHANGED, SetCurrentLive);
        Messenger.AddListener<GameObject>(GameEvents.BOSS_SPAWNED, BossSpawned);
        Messenger.AddListener<BossStats>(GameEvents.BOSS_WERE_KILLED, BossWereKilled);
        bar.transform.parent.gameObject.SetActive(false);
    }

    private void BossSpawned(GameObject gameObject)
    {
        Stats stats = gameObject.GetComponent<Stats>();
        bar.transform.parent.gameObject.SetActive(true);
    }

    private void BossWereKilled(BossStats stats)
    {
        bar.transform.parent.gameObject.SetActive(false);
    }

    private void SetCurrentLive(Stats stats)
    {
        bar.fillAmount = stats.GetCurLife() / stats.GetMaxLife();
    }
}
