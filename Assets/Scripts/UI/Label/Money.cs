using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Money : Label
{
    public override void ChangeValue(object value)
    {
        text.text = ((int)value).ToString() + "$";
    }

}
