using CongTDev.ObjectPooling;
using UnityEngine;

public class DamageFeedback : GlobalReference<DamageFeedback>
{
    [SerializeField] private Prefab floatingTextPrefab;

    [SerializeField] private Prefab damageVFXPrefab;

    [SerializeField] private Transform worldSpaceCanvas;

    public bool isDisplayText;
    public bool isUseVFX;

    public void DisplayDamageFeedback(DamageBlock damageBlock)
    {
        DisplayFloatingText(damageBlock);
        DisplayHitVFX(damageBlock);
    }

    private void DisplayHitVFX(DamageBlock damageBlock)
    {
        if (!isUseVFX || damageBlock.State == DamageState.Miss)
            return;

        if(PoolManager.Get<HitVFX>(damageVFXPrefab, out var hitVfx))
        {
            hitVfx.RunAnimation(damageBlock);
        }
    }

    private void DisplayFloatingText(DamageBlock damageBlock)
    {
        if (!isDisplayText)
            return;

        if(PoolManager.Get<FloatingText>(floatingTextPrefab, out var floatingText))
        {
            floatingText.transform.SetParent(worldSpaceCanvas);
            floatingText.DisplayText(damageBlock);
        }
    }
}
