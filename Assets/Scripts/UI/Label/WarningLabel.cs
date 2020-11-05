using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningLabel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text text;

    private void Start()
    {
        Messenger.AddListener(GameEvents.PLAYER_LEAVE_ARENA, PlayerLeaveArena);
        Messenger.AddListener(GameEvents.PLAYER_RETURN_TO_ARENA, PlayerReturnToArena);
        text.gameObject.SetActive(false);
    }

    private void PlayerLeaveArena()
    {
        text.gameObject.SetActive(true);
        text.text = "GET BACK!";
    }
    private void PlayerReturnToArena()
    {       
        text.text = "";
        text.gameObject.SetActive(false);
    }
}
