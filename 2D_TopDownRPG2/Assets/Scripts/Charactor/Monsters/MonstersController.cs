using CongTDev.ObjectPooling;
using System;
using UnityEngine;

public class MonstersController : BaseCombatCharactorController, IPoolObject
{
    public event Action OnDoneSetup;
    public event Action<Fighter> StageSupportDeathEvent;

    [field: SerializeField] public string MonsterName { get; private set; }
    public int Level { get; private set; }

    private FollowMonsterInfo _monsterInfo;

    protected override void Awake()
    {
        base.Awake();
        Combat.OnDead += DeathEventRaise;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(Combat != null )
        {
            Combat.OnDead -= DeathEventRaise;
        }
    }

    private void OnDisable()
    {
        if(_monsterInfo != null)
        {
            _monsterInfo.ReturnToPool();
        }
    }

    public void Initialize(BaseStatData statData, int level = 1)
    {
        Combat.Stats.ClearAllBonus();
        Combat.InstanciateFromStatsData(statData);
        SetLevel(statData, level);
        Combat.Health.Fill();
        Combat.Mana.Fill();
        Movement.ClearState();
        Animator.ClearState();
        Movement.MoveDirect = MovementInput.InputVector;
        _monsterInfo = MonsterWorldSpaceUIManager.Instance.GetMonsterInfo(this);
        OnDoneSetup?.Invoke();
    }

    private void SetLevel(BaseStatData statData, int level)
    {
        Level = level;
        var statModifiers = statData.growStat.GetGrowingStat(level);
        foreach( var modifier in statModifiers )
        {
            Combat.Stats.ApplyModifier(modifier.Key, modifier.Value);
        }
    }

    private void DeathEventRaise(Fighter fighter)
    {
        StageSupportDeathEvent?.Invoke(fighter);
        StageSupportDeathEvent = null;
    }

    #region Pooling
    private Action<IPoolObject> _returnAction;

    public void Init(Action<IPoolObject> returnAction)
    {
        _returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        if (_returnAction != null)
        {
            _returnAction.Invoke(this);
            _returnAction = null;
        }
        else
        {
            Destroy(gameObject);
        }
    } 
    #endregion
}
