using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] GameEventsEnum enableEvent;
    [SerializeField] bool deactiveListener = false;
    [SerializeField] Label[] labels;
    private void Start()
    {
        if(!deactiveListener)
        {
            Messenger.AddListener(enableEvent.ToString(), ChangeEnabled);
        }     
        enabled = false;
        gameObject.SetActive(false);
    }

    public void ChangeEnabled()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        RefreshLabels();
    }
    private void RefreshLabels()
    {
        foreach(Label label in labels)
        {
            label.RefreshData();
        }
    }
}
