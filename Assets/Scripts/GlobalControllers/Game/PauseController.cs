using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Messenger.AddListener(GameEvents.PRESS_MAIN_PANEL_INGAME_BUTTON, OnPress);
        Time.timeScale = 1;
    }

    private void OnPress()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }      
    }


}
