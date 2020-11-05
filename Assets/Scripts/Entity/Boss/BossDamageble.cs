using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class BossDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] float temporaryImmortalAfterHit = 0f;

    private Stats stats;
    private IRenderer rendererController;
    private float timeFromLastHit = 0f;


    private void Awake()
    {
        stats = GetComponent<Stats>();
        rendererController = GetComponent<IRenderer>();
    }

    public void GetDamage(float damage)
    {

        stats.RemoveFromCurLife(damage);
        if (rendererController != null)
        {
            rendererController.Blink();
        }
        timeFromLastHit = Time.time;
        Messenger.Broadcast(GameEvents.BOSS_LIVE_CHANGED, stats);

    }
}
