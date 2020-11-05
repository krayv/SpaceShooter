using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private GameEventsEnum callEventOnClick;

    public void OnClick()
    {
        Messenger.Broadcast(callEventOnClick.ToString());
    }

}
