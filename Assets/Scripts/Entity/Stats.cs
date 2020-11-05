using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxLife;
    [SerializeField] private bool isCanGetDamage;
    [SerializeField] private float baseDamageMultiplier;
    [SerializeField] private float baseSpeedMultiplie;
    [SerializeField] private float baseLifeMultiplier;
    [SerializeField] private int rewardScore;
    [SerializeField] private int rewardMoney;
    [SerializeField] private string prefabName;
    [SerializeField] private int price;
    [SerializeField] private List<GameObject> buyedShips;


    private float curSpeed;
    private float curLife;


    private void Awake()
    {
        curLife = GetMaxLife();
    }

    public void AddToCurLife(float value)
    {
        if(value > 0)
        {
            curLife = curLife + value > GetMaxLife() ? GetMaxLife() : curLife + value;
            Messenger.Broadcast(GameEvents.PLAYER_LIVE_CHANGED, this);
        }
        
    }
    public void RemoveFromCurLife(float value)
    {       
        curLife -= value;
        if (GetComponent<PlayerInputController>() != null)
        {
            Messenger.Broadcast(GameEvents.PLAYER_LIVE_CHANGED, this);
        }
    }
    public void SetCurLife(float value)
    {
        if (GetComponent<PlayerInputController>() != null)
        {
            Messenger.Broadcast(GameEvents.PLAYER_LIVE_CHANGED, this);
        }
        curLife = value;
    }
    public void SetCurSpeed(float value)
    {
        curSpeed = value;
    }

    public void SetMaxSpeed(float value)
    {
        maxSpeed = value;
    }
    public void SetDamageMultiplier(float value)
    {
        baseDamageMultiplier = value;
    }

    public void SetLiveMultiplier(float value)
    {
        float newCurrentLifeValue = (GetCurLife() / GetMaxLife());
        baseLifeMultiplier = value;
        SetCurLife(GetMaxLife() * newCurrentLifeValue);     
    }

    public int GetPrice()
    {
        return price;
    }

    public float GetCurSpeed()
    {
        return curSpeed;
    }
    public float GetDamageMultiplier()
    {
        return baseDamageMultiplier;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public int GetMoneyReward()
    {
        return rewardMoney;
    }
    public int GetScoreReward()
    {
        return rewardScore;
    }
    public float GetMaxLife()
    {
        return maxLife * baseLifeMultiplier;
    }
    public float GetCurLife()
    {
        return curLife;
    }

    public string GetPrefabName()
    {
        return prefabName;
    }

    public bool IsCanGetDamage()
    {
        return isCanGetDamage;
    }

    public bool IsDead()
    {
        return curLife <= 0f;
    }

}
