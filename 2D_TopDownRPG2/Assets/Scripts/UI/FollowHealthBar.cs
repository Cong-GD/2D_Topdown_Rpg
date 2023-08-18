using CongTDev.ObjectPooling;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FollowHealthBar : PoolObject
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset;

    private Vector3 _finalOffset;
    private Fighter _attachedFighter;

    public void AttachToFighter(Fighter fighter)
    {
        DeAttached();
        _attachedFighter = fighter;
        _finalOffset = offset + new Vector3(0, fighter.HitBox.bounds.extents.y);
        fighter.Health.OnValueChange += ChangeValueUI;
        ChangeValueUI(fighter.Health.Current, fighter.Health.Capacity);
        StartCoroutine(FollowCoroutine());
    }

    private void OnDisable()
    {
        DeAttached();
    }

    private IEnumerator FollowCoroutine()
    {
        while (_attachedFighter != null)
        {
            transform.position = _attachedFighter.HitBox.bounds.center + _finalOffset;
            yield return CoroutineHelper.endOfFrameWait;
        }
    }

    private void ChangeValueUI(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }

    private void DeAttached()
    {
        if (_attachedFighter != null)
        {
            _attachedFighter.Health.OnValueChange -= ChangeValueUI;
        }
        _attachedFighter = null;
    }
}
