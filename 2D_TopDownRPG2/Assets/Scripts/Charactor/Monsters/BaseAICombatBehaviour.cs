using CongTDev.AbilitySystem;
using System.Collections;
using UnityEngine;

public abstract class BaseAICombatBehaviour : MonoBehaviour
{
    private const float COMBAT_BREAK_TIME = 3f;
    public const float COMMON_UPDATE_INTERVAL = 0.5f;

    [Header("Base fields")]
    [SerializeField] protected float maxMoveRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float vision;

    protected AbilityCaster abilityCaster;
    protected MonstersAI monsterAI;
    private FollowHealthBar _healthBar;

    private void OnDisable()
    {
        GiveBackHealthBar();
    }

    public bool IsAlive() => !abilityCaster.Owner.Health.IsEmpty;

    public virtual void Prepare(MonstersAI monsterAI)
    {
        this.monsterAI = monsterAI;
        abilityCaster = monsterAI.Controller.AbilityCaster;
    }

    public IEnumerator StartCombatState()
    {
        yield return CombatStateEnter();
        yield return CombatHandler();
        yield return CombatStateExit();
    }

    protected virtual IEnumerator CombatStateEnter()
    {
        GiveBackHealthBar();
        _healthBar = MonsterWorldSpaceUIManager.Instance.ReceiveHealthBar(abilityCaster.Owner);
        yield break;
    }

    protected abstract IEnumerator CombatHandler();

    protected virtual IEnumerator CombatStateExit()
    {
        GiveBackHealthBar();
        if(IsAlive())
        {
            monsterAI.MoveTo(monsterAI.StartPosition);
            yield return COMBAT_BREAK_TIME.Wait();
        }
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
        DrawGizmos();
    }

    protected virtual void DrawGizmos()
    {
        DrawCircle(attackRange, Color.red);
        DrawCircle(vision, Color.blue);
        DrawCircle(maxMoveRange, Color.gray);
    }

    protected void DrawCircle(float radius, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
