using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Stats))]
public class CurrentStateController : MonoBehaviour
{
    [SerializeField] Stats stats;
    [SerializeField] EngineController engineController;
    [SerializeField] GameObject explosionWhenDead;
    [SerializeField] float explosionSize = 0.5f;

    private void Awake()
    {
        stats = GetComponent<Stats>();
        engineController = GetComponent<EngineController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(stats.IsDead())
        {
           
            Die();
        }
        if(stats.GetCurSpeed() <= 0.1f)
        {
            if(engineController != null)
            {
                engineController.EngineOff();
            }
        }
        else
        {
            if (engineController != null)
            {
                engineController.EngineOn();
            }
        }
    }

    protected virtual void Die()
    {
        Messenger.Broadcast(GameEvents.ENTITY_DEAD, gameObject);
        if (explosionWhenDead != null)
        {
            GameObject explosionGameObject = Instantiate(explosionWhenDead);
            explosionGameObject.transform.position = transform.position;
            explosionGameObject.transform.localScale *= explosionSize;
        }
        Destroy(gameObject);
    }

}
