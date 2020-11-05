using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof(Stats))]
[RequireComponent(typeof(Collider2D))]
public class PlayerDamageble : MonoBehaviour, IDamageble
{
    [SerializeField] float temporaryImmortalAfterHit = 0f;

    private Stats stats;
    private IRenderer rendererController;
    private float timeFromLastHit = 0f;
    private Collider2D currentCollider2D;


    private void Awake()
    {
        stats = GetComponent<Stats>();
        currentCollider2D = GetComponent<Collider2D>();
        rendererController = GetComponent<IRenderer>();
    }

    public void GetDamage(float damage)
    {
        if(Time.time - timeFromLastHit > temporaryImmortalAfterHit)
        {
            stats.RemoveFromCurLife(damage);
            if (rendererController != null)
            {
                rendererController.Blink();
            }
            timeFromLastHit = Time.time;
        }
    }
}
