using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitcherController : MonoBehaviour
{
    [SerializeField] Button buttonLeft;
    [SerializeField] Button buttonRight;
    [SerializeField] HangarController hangarController;

    private void Awake()
    {
        hangarController = GetComponent<HangarController>();
    }

    public void OnSwitch()
    {
        if(hangarController.IsCanSwitchLeft())
        {
            buttonLeft.gameObject.SetActive(true);
        }
        else
        {
            buttonLeft.gameObject.SetActive(false);
        }
        if (hangarController.IsCanSwitchRight())
        {
            buttonRight.gameObject.SetActive(true);
        }
        else
        {
            buttonRight.gameObject.SetActive(false);
        }
    }
}
