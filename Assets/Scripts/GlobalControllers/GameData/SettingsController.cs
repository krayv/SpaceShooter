using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;


public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider qualityValueSlider;

    private float currentQualityValue;
    private string filename;

    public float GetCurrentQualityValue()
    {
        return currentQualityValue;
    }

    private void Awake()
    {
        filename = Path.Combine(Application.persistentDataPath, "settings.dat");
    }

    private void Start()
    {
        Messenger.AddListener<float>(GameEvents.CHANGE_QUALITY, OnChangeQuality);
        Load();
        if(qualityValueSlider != null)
        {
            qualityValueSlider.value = currentQualityValue;
        }
        BroadcastQualityValue();
    }

    public void Save()
    {
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("game_quality", currentQualityValue);
        FileStream stream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate);
        stream.Close();
    }

    private void Load()
    {
        if (!File.Exists(filename))
        {
            currentQualityValue = 0.5f;
            return;
        }
        Dictionary<string, object> gamestate;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        currentQualityValue = (float)gamestate["game_quality"];     
    }

    private void OnChangeQuality(float value)
    {
        currentQualityValue = value;
        Save();
    }

    private IEnumerator BroadcastQualityValue()
    {
        yield return new WaitForEndOfFrame();
        Messenger.Broadcast(GameEvents.CHANGE_QUALITY, currentQualityValue);
    }
}
    
