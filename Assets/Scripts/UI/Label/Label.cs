using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Label : MonoBehaviour
{
    [SerializeField] protected GameDataList gameData;
    [SerializeField] protected Text text;
    protected Action<object> callback;


    public void RefreshData()
    {
        RequestGameData(gameData);
    }

    public GameDataList GetGameData()
    {
        return gameData;
    }

    protected virtual void RequestGameData(GameDataList gameDataList)
    {
        Messenger.Broadcast(GameEvents.REQUEST_GAME_DATA, gameDataList, callback);
    }

    protected virtual void Awake()
    {
        callback = ChangeValue;
    }
    protected virtual void Start()
    {
        Messenger.AddListener<KeyValuePair<GameDataList, object>>(GameEvents.GAMEDATA_VALUE_CHANGED, CheckData);
    }

    protected virtual void CheckData(KeyValuePair<GameDataList, object> typeAndValue)
    {
        if(gameData == typeAndValue.Key)
        {
            try
            {
                ChangeValue(typeAndValue.Value);
            }
            catch(Exception exc)
            {
                Debug.LogError(exc.Message);
            }            
        }
    }

    public virtual void ChangeValue(object value)
    {
        text.text = value.ToString();
    }
}
