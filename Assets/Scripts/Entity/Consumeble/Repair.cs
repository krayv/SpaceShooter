using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : AbstractConsumable
{
    [SerializeField] float repairValue;
    protected override void OnConsume(GameObject gameObject)
    {
        Stats stats = gameObject.GetComponent<Stats>();
        float maxLife = stats.GetMaxLife();
        stats.AddToCurLife(maxLife * repairValue);
    }
}
