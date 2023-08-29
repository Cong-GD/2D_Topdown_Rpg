using CongTDev.AbilitySystem;
using System.Collections;
using UnityEngine;

public class BossMinionAI : SeekerMovingAI
{
    [SerializeField] private float updateInterval;
    [SerializeField] private float movingAngle;
    [SerializeField] private float movingRange;

    [Header("Ability handler")]
    [SerializeField] private MonstersController controller;
    [SerializeField] private ActiveRune abilityRune;
    [SerializeField] private float abilityUseInterval;
    [SerializeField] private float attackRange;
    [SerializeField] private float aimTime;

    private IActiveAbility _ability;
    private Fighter _target;
    private FollowHealthBar _healthBar;
    private AbilityCaster _caster;

    private void Awake()
    {
        _caster = controller.AbilityCaster;
        controller.OnDoneSetup += StartAIRoutine;
    }

    private void OnDisable()
    {
        GiveBackHealthBar();
    }

    private void OnDestroy()
    {
        if(controller != null)
        {
            controller.OnDoneSetup -= StartAIRoutine;
        }
    }

    private void StartAIRoutine()
    {
        _healthBar = MonsterWorldSpaceUIManager.Instance.ReceiveHealthBar(_caster.Owner);
        StartCoroutine(AICoroutine());
    }

    private IEnumerator AICoroutine()
    {
        _target = PlayerController.Instance.Combat;
        _ability = (IActiveAbility)abilityRune.CreateItem();
        _ability.Install(_caster);
        var timeToUseAbility = Time.time;

        
        while (!_caster.Owner.Health.IsEmpty)
        {
            var distanceToTarget = Vector2.Distance(transform.position, _target.Position);
            if (distanceToTarget < attackRange && Time.time > timeToUseAbility)
            {
                StartCoroutine(AimCoroutine(_ability.CastDelay));
                _ability.TryUse();
                timeToUseAbility = Time.time + abilityUseInterval;
            }
            MoveTo(GetRandomPositionFromMovingRange());
            yield return updateInterval.Wait();
        }
        GiveBackHealthBar();
        yield return 1.5f.Wait();
    }

    private Vector2 GetRandomPositionFromMovingRange()
    {
        var direction = ((Vector2)transform.position - _target.Position).normalized;
        var rotateRad = Random.Range(-movingAngle / 2, movingAngle / 2) * Mathf.Deg2Rad;
        var newRad = rotateRad + Mathf.Atan2(direction.y, direction.x);
        direction = new Vector2(Mathf.Cos(newRad), Mathf.Sin(newRad));
        return _target.Position + direction * movingRange;
    }

    private IEnumerator AimCoroutine(float castDelay)
    {
        yield return (castDelay - aimTime).Wait();
        _caster.LookDirection = _target.Position;
    }
    private void GiveBackHealthBar()
    {
        if (_healthBar != null)
        {
            _healthBar.ReturnToPool();
            _healthBar = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, movingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    } 
#endif

}