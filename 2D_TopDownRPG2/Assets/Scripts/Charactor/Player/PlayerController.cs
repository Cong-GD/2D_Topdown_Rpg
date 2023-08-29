using CongTDev.AbilitySystem;
using CongTDev.EventManagers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : BaseCombatCharactorController
{
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<PlayerController>();
            }
            return _instance;
        }
    }

    public static bool IsDead { get; private set; } = true;

    [SerializeField] private BaseStatData statsData;

    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
        {
            _instance = this;
        }
    }

    protected void OnEnable()
    {
        _camera = CameraManager.MainCam;
        RevivePlayer();
    }
    private void Update()
    {
        AbilityCaster.LookDirection = _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    protected override void OnDead(Fighter _)
    {
        if (IsDead)
            return;

        IsDead = true;
        base.OnDead(_);
        InputCentral.Disable();
        EventManager.RaiseEvent("OnPlayerDead");
        StartCoroutine(AskForRevivePlayer());
    }

    private IEnumerator AskForRevivePlayer()
    {
        yield return 2f.Wait();
        ConfirmPanel.Ask("You are died and has lost 1 level.\n" +
                        "Do you want to pay 1000G to play again?",
        () => {
            if (GameManager.PlayerGold >= 1000)
            {
                
                GameManager.PlayerGold -= 1000;
                ReviveDelay(GameManager.CurrentMap);
            }
            else
            {
                ConfirmPanel.Ask("You don't have enough gold!", () => ReviveDelay("Town"), () => ReviveDelay("Town"));
            }
        }, () => ReviveDelay("Town")); 
    }
    private void ReviveDelay(string mapName)
    {
        StartCoroutine(ReviveDelayCoroutine(mapName));
    }

    private IEnumerator ReviveDelayCoroutine(string mapName)
    {
        yield return 1f.Wait();
        RevivePlayer();
        GameManager.Instance.ChangeMap(mapName);
    }

    public void RevivePlayer()
    {
        Combat.InstanciateFromStatsData(statsData);
        Combat.Health.Fill();
        Combat.Mana.Fill();
        IsDead = false;
        Movement.ClearState();
        Animator.ClearState();
        Animator.SetMovingState(Movement.IsMoving);
        Movement.MoveDirect = MovementInput.InputVector;
    }
}
