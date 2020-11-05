using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordRow : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text dateTimeText;
    [SerializeField] Text lengtTimeText;
    [SerializeField] Image image;
    
    public void SetInfo(SessionRecord sessionRecord)
    {
        scoreText.text = "Score: "+ sessionRecord.score.ToString();
        dateTimeText.text = sessionRecord.sessionTime.ToString("MM/dd/yyyy HH:mm");
        int minutes = (int)(sessionRecord.sessionLenght / 60f);
        int seconds = (int)(sessionRecord.sessionLenght % 60f);
        string minutesString = string.Format("{0:00}", minutes);
        string secondsString = string.Format("{0:00}", seconds);
        lengtTimeText.text = minutesString + ":" + secondsString;
        string path = GameConstants.prefabShipPath + sessionRecord.prefabShipName;
        GameObject prefab = Resources.Load<GameObject>(path);
        image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
    }
}
