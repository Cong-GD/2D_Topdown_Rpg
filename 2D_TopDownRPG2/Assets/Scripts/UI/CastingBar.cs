using CongTDev.AbilitySystem;
using CongTDev.ObjectPooling;
using UnityEngine;
using UnityEngine.UI;

public class CastingBar : PoolObject
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector2 offset;

    private AbilityCaster _abilityCaster;
    private Vector3 _finalOffSet;

    public void ShowBar(AbilityCaster abilityCaster)
    {
        if (abilityCaster != null)
        {
            _abilityCaster = abilityCaster;
            _finalOffSet = offset + new Vector2(0, -abilityCaster.Owner.HitBox.bounds.extents.y);
            fillImage.fillAmount = 0;
            _abilityCaster.OnCastingProgress += ProgressCasting;
            _abilityCaster.OnCastingEnd += EndCasting; 
        }
        else
        {
            ReturnToPool();
        }
    }

    private void LateUpdate()
    {
        if(_abilityCaster != null) 
        {
            transform.position = _abilityCaster.Owner.HitBox.bounds.center + _finalOffSet;
        }
    }

    private void ProgressCasting(float value)
    {
        fillImage.fillAmount = value;
    }

    private void EndCasting()
    {
        if(_abilityCaster != null)
        {
            _abilityCaster.OnCastingProgress -= ProgressCasting;
            _abilityCaster.OnCastingEnd -= EndCasting;
        }
        _abilityCaster = null;
        ReturnToPool();
    }
}
