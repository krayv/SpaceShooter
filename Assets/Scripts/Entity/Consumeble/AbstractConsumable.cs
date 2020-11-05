using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractConsumable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerInputController>() != null)
        {
            OnConsume(collision.gameObject);
            Destroy();
        }
    }

    protected virtual void Destroy()
    {
        Messenger.Broadcast(GameEvents.ENTITY_DEAD, gameObject);
        Destroy(gameObject);
    }
    protected abstract void OnConsume(GameObject gameObject);

}
