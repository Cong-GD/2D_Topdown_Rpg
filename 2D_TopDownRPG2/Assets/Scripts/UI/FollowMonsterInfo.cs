using CongTDev.ObjectPooling;
using TMPro;
using UnityEngine;

public class FollowMonsterInfo : PoolObject
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Vector2 offset;

    private Vector2 _finalOffset;
    private MonstersController _attachedMonster;

    public void AttachToMonster(MonstersController attachedMonster)
    {
        _attachedMonster = attachedMonster;
        _finalOffset = offset + new Vector2(0, _attachedMonster.Combat.HitBox.bounds.extents.y);
        nameText.text = _attachedMonster.MonsterName;
        levelText.text = $"Lv.{_attachedMonster.Level}";
    }

    private void LateUpdate()
    {
        if( _attachedMonster != null )
        {
            transform.position = _attachedMonster.Combat.Position + _finalOffset;
        }
    }

    private void OnDisable()
    {
        _attachedMonster = null;
    }

}
