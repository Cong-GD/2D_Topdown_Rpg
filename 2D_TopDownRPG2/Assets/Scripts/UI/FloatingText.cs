using CongTDev.ObjectPooling;
using TMPro;
using UnityEngine;

public class FloatingText : PoolObject
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private Animator animator;

    private Collider2D _followColider;
    private Vector2 _offset;

    private void OnDisable()
    {
        _followColider = null;
    }

    private void LateUpdate()
    {
        if (_followColider != null)
        {
            transform.position = (Vector2)_followColider.bounds.center + _offset;
        }
    }
    public void DisplayText(DamageBlock damageBlock)
    {
        _followColider = damageBlock.Target.HitBox;
        _offset = new Vector2(0, _followColider.bounds.extents.y) + Random.insideUnitCircle * 0.5f;
        textMeshProUGUI.color = ColorHelper.GetDamageFeedBackTextColor(damageBlock.DamageType);
        int roundedDamage = Mathf.FloorToInt(damageBlock.CurrentDamage);
        textMeshProUGUI.text = damageBlock.State switch
        {
            DamageState.NormalDamage => $"{roundedDamage}",
            DamageState.CriticalDamage => $"{roundedDamage}!",
            DamageState.BlockDamage => $"( {roundedDamage} )",
            DamageState.Miss => "Miss",
            _ => string.Empty
        };
    }

}