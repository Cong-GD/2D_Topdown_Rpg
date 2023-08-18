using CongTDev.ObjectPooling;
using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : PoolObject
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private Animator animator;

    private static readonly WaitForEndOfFrame wait = new();
    public void DisplayText(DamageBlock damageBlock)
    {
        StopAllCoroutines();
        StartCoroutine(FollowCoroutine(damageBlock.Target.HitBox));
        textMeshProUGUI.color = ColorHelper.GetDamageFeedBackTextColor(damageBlock.DamageType);
        textMeshProUGUI.text = damageBlock.State switch
        {
            DamageStates.NormalDamage => $"{damageBlock.CurrentDamage}",
            DamageStates.CriticalDamage => $"{damageBlock.CurrentDamage}!",
            DamageStates.BlockDamage => $"d{damageBlock.CurrentDamage}b",
            DamageStates.Miss => "Miss",
            _ => string.Empty
        };
    }

    private IEnumerator FollowCoroutine(Collider2D collider2)
    {
        var offSet = new Vector2(0, collider2.bounds.extents.y) + Random.insideUnitCircle * 0.5f;
        while (collider2 != null)
        {
            transform.position = (Vector2)collider2.bounds.center + offSet;
            yield return wait;
        }
    }
}