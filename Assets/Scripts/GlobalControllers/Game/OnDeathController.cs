using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnDeathController : MonoBehaviour
{
    [SerializeField] Panel deathPanel;
    // Start is called before the first frame update
    private void Start()
    {
        Messenger.AddListener(GameEvents.PLAYER_DEAD, OnDeath);
        Messenger.AddListener(GameEvents.PLAYER_RESPAWNED, OnRespawn);
    }

    // Update is called once per frame
    private void OnDeath()
    {
        StartCoroutine(ChangeEnabledStatusOnDeath());
    }

    private IEnumerator ChangeEnabledStatusOnDeath()
    {
        yield return new WaitForSeconds(1f);
        deathPanel.ChangeEnabled();
    }

    private void OnRespawn()
    {
        deathPanel.ChangeEnabled();
    }
}
