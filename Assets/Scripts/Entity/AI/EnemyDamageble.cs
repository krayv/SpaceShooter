using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageble : MonoBehaviour, IDamageble
{
    private Stats stats;
    private IRenderer rendererController;

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
    }
}
