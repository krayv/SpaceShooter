using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : Label
{
    public override void ChangeValue(object value)
    {
        text.text = ((int)value).ToString();
    }
}
