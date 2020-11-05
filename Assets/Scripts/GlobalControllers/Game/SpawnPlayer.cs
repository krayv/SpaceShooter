using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameStats))]
public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] Vector3 startPosition = new Vector3(0, 0, 0);

    private GameStats stats;
    private Transform currentTranfsorm;
    private Vector3 lastPosition;
    // Start is called before the first frame update
    private void Awake()
    {
        stats = GetComponent<GameStats>();
    }

    private void Start()
    {
        Messenger.AddListener(GameEvents.PLAYER_RESPAWNED, Respawn);
    }

    private void FixedUpdate()
    {
        if(currentTranfsorm != null)
        {
            lastPosition = currentTranfsorm.position;
        }
    }

    // Update is called once per frame
    public void Spawn()
    {
        GameObject playerPrefab = stats.GetPlayerPrefab();
        GameObject renderedPlayer = Instantiate(playerPrefab);
        renderedPlayer.transform.position = startPosition;
        currentTranfsorm = renderedPlayer.transform;
    }
    public void Respawn()
    {
        GameObject playerPrefab = stats.GetPlayerPrefab();
        GameObject renderedPlayer = Instantiate(playerPrefab);
        renderedPlayer.transform.position = lastPosition;
        Messenger.Broadcast(GameEvents.PLAYER_LIVE_CHANGED, renderedPlayer.GetComponent<Stats>());
    }
}
