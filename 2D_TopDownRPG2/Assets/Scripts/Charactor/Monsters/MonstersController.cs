using CongTDev.ObjectPooling;
using System;
using UnityEngine;

public class MonstersController : BaseCombatCharactorController, IPoolObject
{
    [SerializeField] private BaseStatData statsData;

    protected void OnEnable()
    {
        Initialize(statsData);
    }

    public void Initialize(BaseStatData statData)
    {
        Combat.InstanciateFromStatsData(statData);
        Combat.Health.Fill();
        Combat.Mana.Fill();
        Movement.ClearState();
        Animator.ClearState();
        Animator.SetMovingState(Movement.IsMoving);
        Movement.MoveDirect = MovementInput.InputVector;
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
