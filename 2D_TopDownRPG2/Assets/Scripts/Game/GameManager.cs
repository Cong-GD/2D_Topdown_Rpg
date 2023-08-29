using CongTDev.EventManagers;
using CongTDev.IOSystem;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : GlobalReference<GameManager>
{
    public static string CurrentMap { get; private set; }
    public static bool IsPausing { get; private set; }


    [Serializable]
    public class GameStatus
    {
        public const string FILE_NAME = "GameStatus";

        public bool hasGuided;
        public int gold;
    }

    private static GameStatus _status = new();
    public static event Action OnGoldChange;

    public static int PlayerGold
    {
        get => _status.gold;
        set
        {
            if (value <= 0)
                value = 0;

            _status.gold = value;
            OnGoldChange?.Invoke();
        }
    }


    [SerializeField] private Slider loadingSlider;

    protected override void Awake()
    {
        loadingSlider.gameObject.SetActive(true);
        base.Awake();
        EventManager.AddListener("OnGameSave", SaveStatus);
        EventManager.AddListener("OnGameLoad", LoadStatus);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener("OnGameSave", SaveStatus);
        EventManager.RemoveListener("OnGameLoad", LoadStatus);
    }

    private IEnumerator Start()
    {
        EventManager<string>.RaiseEvent("SendSystemMessage", "Wellcome to summorner's rift!");
        yield return null;
        EventManager.RaiseEvent("OnGameLoad");
        yield return ChangeMapCoroutine("Town");
        if(!_status.hasGuided)
        {
            _status.hasGuided = true;
            var guide = Resources.Load<GameObject>("Guide Canvas");
            if(guide != null)
            {
                InputCentral.Disable();
                Instantiate(guide);
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = IsPausing ? 1 : 0;
            IsPausing = !IsPausing;
        }
    }

    public void SaveStatus()
    {
        SaveLoadHandler.SaveToFile(GameStatus.FILE_NAME, _status);
    }

    public void LoadStatus()
    {
        var status = SaveLoadHandler.LoadFromFile<GameStatus>(GameStatus.FILE_NAME);
        if(status != null)
        {
            _status = status;
        }
        else
        {
            _status = new();
            _status.gold = 1500;
        }
    }

    public void ChangeMap(string mapName)
    {
        StartCoroutine(ChangeMapCoroutine(mapName));
    }

    private bool CheckIfSceneExits(string mapName)
    {
        var numberOfScene = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < numberOfScene; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneName == mapName)
            {
                return true;
            }
        }
        Debug.LogWarning($"{mapName} scene does not exits!");
        return false;
    }

    private IEnumerator ChangeMapCoroutine(string mapName)
    {
        if (!CheckIfSceneExits(mapName))
            yield break;

        loadingSlider.gameObject.SetActive(true);
        InputCentral.Disable();
        yield return 0.2f.WaitInRealTime();
        EventManager.RaiseEvent("OnGameSave");
        EventManager.RaiseEvent("OnMapChanging");
        AsyncOperation asynsOperation;
        if (SceneManager.sceneCount > 1)
        {
            DOTween.KillAll();
            asynsOperation = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            CurrentMap = "None";
            while (!asynsOperation.isDone)
            {
                loadingSlider.value = asynsOperation.progress / 2;
                yield return null;
            }
        }
        asynsOperation = SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive);
        CurrentMap = mapName;
        while (!asynsOperation.isDone)
        {
            loadingSlider.value = 0.5f + asynsOperation.progress / 2;
            yield return null;
        }
        EventManager.RaiseEvent("OnMapChanged");
        EventManager.RaiseEvent("OnGameLoad");
        yield return 0.2f.WaitInRealTime();
        InputCentral.Enable();
        loadingSlider.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Game.Quit();
    }
}
