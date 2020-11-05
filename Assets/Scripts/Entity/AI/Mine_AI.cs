using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Mine_AI : MonoBehaviour, IEnemy
{
    [SerializeField] float damage;

    private Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollide(collision.collider);
    }

    private void OnCollide(Collider2D collider)
    {
        IDamageble damageble = collider.GetComponent<IDamageble>();
        if (damageble != null)
        {
            PlayerInputController player = collider.GetComponent<PlayerInputController>();
            if (player != null)
            {
                DealDamage(damageble);
                
            }
        }
    }
    private void DealDamage(IDamageble damageble)
    {
        damageble.GetDamage(damage);
        stats.SetCurLife(0);
    }
}
