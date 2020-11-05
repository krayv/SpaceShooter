using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent (typeof(GameStats))]
public class SceneController : MonoBehaviour
{
    [SerializeField] GameStats gameStats;
    [SerializeField] RecordsController recordsController;

    private void Awake()
    {
        Messenger.Cleanup();
        gameStats = GetComponent<GameStats>();
        recordsController = GetComponent<RecordsController>();
    }

    private void Start()
    {
        Messenger.AddListener(GameEvents.START_GAME_SCENE, StartGameScene);
    }

    public void BeforeLoad()
    {
        gameStats.SaveGame();
    }

    public void StartGameScene()
    {
        BeforeLoad();
        SceneManager.LoadScene(Scenes.GameScene);
    }
    public void MainMenuScene()
    {
        BeforeLoad();
        BeforeLeaveGameScene();
        SceneManager.LoadScene(Scenes.MainMenuScene);
    }

    public void BeforeLeaveGameScene()
    {
        recordsController.Save();
    }
}
