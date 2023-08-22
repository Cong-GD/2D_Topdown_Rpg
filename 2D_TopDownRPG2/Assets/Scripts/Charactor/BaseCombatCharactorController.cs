using CongTDev.AbilitySystem;
using UnityEngine;

public abstract class BaseCombatCharactorController : MonoBehaviour
{
    [field: SerializeField] public BaseMovementInput MovementInput { get; private set; }
    [field: SerializeField] public Movement Movement { get; private set; }
    [field: SerializeField] public Fighter Combat { get; private set; }
    [field: SerializeField] public AbilityCaster AbilityCaster { get; private set; }
    [field: SerializeField] public CombatAnimator Animator { get; private set; }

    protected virtual void Awake()
    {
        Movement.OnStartMoving += StartMoving;
        Movement.OnStopMoving += StopMoving;
        MovementInput.OnInputChange += OnInputValueChange;
        Combat.OnTakeDamage += OnTakeDamage;
        Combat.OnDead += OnDead;
        AbilityCaster.OnCastingStart += ShowCastingBar;
    }

    protected virtual void OnDestroy()
    {
        if(Movement != null)
        {
            Movement.OnStartMoving -= StartMoving;
            Movement.OnStopMoving -= StopMoving;
            MovementInput.OnInputChange -= OnInputValueChange;
        }
        if(Combat != null)
        {
            Combat.OnTakeDamage -= OnTakeDamage;
            Combat.OnDead -= OnDead;
        }
        if(AbilityCaster != null)
        {
            AbilityCaster.OnCastingStart -= ShowCastingBar;
        }
    }

    protected virtual void StartMoving()
    {
        Animator.SetMovingState(true);
    }

    protected virtual void StopMoving()
    {
        Animator.SetMovingState(false);
    }

    protected virtual void OnInputValueChange(Vector2 vector)
    {
        Movement.MoveDirect = vector;
    }
    protected virtual void OnTakeDamage(DamageBlock block)
    {
        Animator.TriggerHurtAnimation();
        AbilityCaster.CollapseCasting();
    }

    protected virtual void OnDead(Fighter _)
    {
        Animator.TriggerDeathAnimation();
        AbilityCaster.CollapseCasting();
        Movement.BlockMovement = true;
    }

    protected virtual void ShowCastingBar(IAbility _)
    {
        CastingBarManager.Instance.ShowCastingBar(AbilityCaster);
    }
}
