using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameStats))]
public class BuyingController : MonoBehaviour
{
    [SerializeField] Button buyButton;
    [SerializeField] Text priceLabel;

    private GameStats gameStats;

    private GameObject currentShip;
    private Stats currentShipStats;

    public void BuyShip()
    {
        if(currentShip != null)
        {
            gameStats.BuyShip(currentShip);
        }
        else
        {

        }    
    }

    private void Start()
    {
        gameStats = GetComponent<GameStats>();
        Messenger.AddListener<GameObject>(GameEvents.SHIP_SWITCHED, OnSwitchShip);
        buyButton.gameObject.SetActive(false);
    }

    private void OnSwitchShip(GameObject ship)
    {
        currentShip = ship;
        currentShipStats = ship.GetComponent<Stats>();
        int price = currentShipStats.GetPrice();
        priceLabel.text = price.ToString() + "$";
        buyButton.gameObject.SetActive(!gameStats.IsHaveThatShip(ship));
        buyButton.interactable = gameStats.IsEnoughtMoney(price) && gameStats.IsPreviousShipbuyed(ship); 
    }
}
