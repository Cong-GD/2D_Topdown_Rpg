using CongTDev.ObjectPooling;
using System;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    private static Action<DamageBlock> _instanceCallback;

    public static void Display(DamageBlock damageBlock)
    {
        _instanceCallback?.Invoke(damageBlock);
    }

    [SerializeField] private Prefab floatingTextPrefab;

    [SerializeField] private Prefab damageVFXPrefab;

    [SerializeField] private Transform worldSpaceCanvas;

    public bool isDisplayText;
    public bool isUseVFX;

    private void Awake()
    {
        _instanceCallback = DisplayDamageFeedback;
    }

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
