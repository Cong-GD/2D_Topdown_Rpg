using CongTDev.ObjectPooling;
using UnityEngine;

public class HitVFX : PoolObject
{
    [SerializeField]
    private Animator anim;

    private static readonly int physicalAnim = Animator.StringToHash("PhysicalDamage");
    private static readonly int magicAnim = Animator.StringToHash("MagicDamage");

    public void RunAnimation(DamageBlock damageBlock)
    {
        transform.SetParent(damageBlock.Target.HitBox.transform);
        transform.position = damageBlock.Target.HitBox.bounds.center;
        switch (damageBlock.DamageType)
        {
            case DamageType.PhysicalDamage:
                anim.Play(physicalAnim);
                break;
            case DamageType.MagicDamage:
                anim.Play(magicAnim);
                break;
            default:
                anim.Play(physicalAnim);
                break;
        }
    }
}
