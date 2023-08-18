using CongTDev.AbilitySystem;
using UnityEngine;

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
        _camera = GameManager.Instance.Cam;

        Combat.InstanciateFromStatsData(statsData);
        Combat.Health.Fill();
        Combat.Mana.Fill();
        Movement.ClearState();
        Animator.SetMovingState(Movement.IsMoving);
        Movement.MoveDirect = MovementInput.InputVector;

    }

    private void Update()
    {
        AbilityCaster.LookDirection = _camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
