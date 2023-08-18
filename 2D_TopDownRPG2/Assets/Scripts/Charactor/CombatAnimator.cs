using UnityEngine;

public class CombatAnimator : MovementAnimator
{
    public static readonly int HurtHash = Animator.StringToHash("Hurt");
    public static readonly int DeathHash = Animator.StringToHash("Death");

    public void TriggerHurtAnimation()
    {
        Animator.Play(HurtHash);
    }

    public void TriggerDeathAnimation()
    {
        Animator.Play(DeathHash);
    }
}
