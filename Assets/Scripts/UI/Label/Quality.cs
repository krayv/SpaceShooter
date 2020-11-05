using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Quality : MonoBehaviour
{
    Slider currentSlider;
    private void Awake()
    {
        currentSlider = GetComponent<Slider>();

    }

    public void OnChangeValueFromCurrentObject()
    {
        Messenger.Broadcast(GameEvents.CHANGE_QUALITY, currentSlider.value);
    }
}
