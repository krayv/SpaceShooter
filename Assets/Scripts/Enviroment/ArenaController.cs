using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [SerializeField] float damagePerSecond;

    private bool isPlayerLeaveArena = false;
    private PlayerDamageble playerDamageble;
    private GameObject player;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(LayersAndTags.PLAYER_TAG);
        Messenger.AddListener<GameObject>(GameEvents.ENTITY_DEAD, OnEntityDead);
        playerDamageble = player.GetComponent<PlayerDamageble>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerInputController playerInputController = collision.GetComponent<PlayerInputController>();
        if(playerInputController != null && !collision.GetComponent<Stats>().IsDead())
        {
            Messenger.Broadcast(GameEvents.PLAYER_LEAVE_ARENA);
            isPlayerLeaveArena = true;
            StartCoroutine(DamagePlayer());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInputController playerInputController = collision.GetComponent<PlayerInputController>();
        if (playerInputController != null)
        {
            isPlayerLeaveArena = false;
            Messenger.Broadcast(GameEvents.PLAYER_RETURN_TO_ARENA);
        }
    }

    private void OnEntityDead(GameObject entity)
    {
        if(entity == player)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
        
    }

    private IEnumerator DamagePlayer()
    {
        while(isPlayerLeaveArena)
        {
            yield return new WaitForSeconds(1f);
            playerDamageble.GetDamage(damagePerSecond);
        }
    }
}
