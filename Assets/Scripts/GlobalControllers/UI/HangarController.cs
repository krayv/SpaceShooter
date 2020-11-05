using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SwitcherController))]
public class HangarController : MonoBehaviour
{

    [SerializeField] Button butButton;
    [SerializeField] Transform spawnShipPoint;
    [SerializeField] List<GameObject> ships;
    [SerializeField] GameObject selectedShip;
    SwitcherController switcher;

    private void Awake()
    {
        switcher = GetComponent<SwitcherController>();
    }
    

    private int currentIndex;
    
    public void SwitchLeft()
    {
        SelectShip(ships[currentIndex - 1]);   
    }

    public void SwitchRight()
    {
        SelectShip(ships[currentIndex + 1]);
        
    }

    public List<GameObject> GetShips()
    {
        return ships;
    }

    public void BuyShip()
    {

    }

    public void SelectShip(GameObject ship)
    {
        currentIndex = ships.IndexOf(ship);
        RenderShip(ship);
        switcher.OnSwitch();
        StartCoroutine(BroadcastWithDelay(ship));
    }

    public bool IsCanSwitchLeft()
    {
        return currentIndex - 1 >= 0;
    }

    public bool IsCanSwitchRight()
    {
        return currentIndex + 2 <= ships.Count;
    }

    public void RenderShip(GameObject ship)
    {
        Vector3 position = spawnShipPoint.position;
        if(selectedShip != null)
        {
            position = selectedShip.transform.position;
            Destroy(selectedShip);
        }
        GameObject renderedShip = Instantiate(ship);
        renderedShip.transform.position = position;
        PlayerInputController playerInputController = renderedShip.GetComponent<PlayerInputController>();
        playerInputController.LockInput(renderedShip.transform.up);
        selectedShip = renderedShip;
    }

    private IEnumerator BroadcastWithDelay(GameObject ship)
    {
        yield return new WaitForSeconds(0.001f);
        Messenger.Broadcast(GameEvents.SHIP_SWITCHED, ship);
        Messenger.Broadcast(GameEvents.SHIP_SELECTED, ship);
    }
}
