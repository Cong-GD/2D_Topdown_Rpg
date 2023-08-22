using CongTDev.ObjectPooling;
using UnityEngine;
using UnityEngine.UI;

public class FollowHealthBar : PoolObject
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector2 offset;

    private Vector2 _finalOffset;
    private Fighter _attachedFighter;

    public void AttachToFighter(Fighter fighter)
    {
        DeAttached();
        _attachedFighter = fighter;
        _finalOffset = offset + new Vector2(0, fighter.HitBox.bounds.extents.y);
        fighter.Health.OnValueChange += ChangeValueUI;
        ChangeValueUI(fighter.Health.Current, fighter.Health.Capacity);
    }

    private void OnDisable()
    {
        DeAttached();
    }

    private void LateUpdate()
    {
        if (_attachedFighter != null)
        {
            transform.position = _attachedFighter.Position + _finalOffset;
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
