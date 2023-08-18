using CongTDev.ObjectPooling;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class SwordTrail : PoolObject, ISpell
    {
        private OrientationAbility _ability;

        public void KickOff(OrientationAbility ability, Vector2 direction)
        {
            _ability = ability;
            direction.Normalize();
            transform.position = ability.Caster.Owner.HitBox.bounds.center;
            transform.Translate(direction * ability.Caster.Owner.HitBox.bounds.size.y);
            var rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Fighter>(out var target))
            {
                _ability.HitThisFighter(target);
            }
        }
    }
}