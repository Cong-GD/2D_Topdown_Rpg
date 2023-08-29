using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class SlashSpell : PoolObject, ISpell
    {
        [SerializeField] private SpriteRenderer render;
        [SerializeField] private float _distance;

        private OrientationAbility _ability;

        public void KickOff(OrientationAbility ability, Vector2 direction)
        {
            _ability = ability;
            bool isLookingRight = ability.Caster.Owner.Movement.IsFacingRight;

            transform.position = ability.Caster.Owner.HitBox.bounds.center;
            transform.Translate((isLookingRight ? Vector2.right : Vector2.left) * _distance);
            
            render.flipX = !isLookingRight;
            AudioManager.Play("Slash");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_ability == null) return;

            if (collision.gameObject.TryGetComponent<Fighter>(out var target) && _ability.IsRightTarget(target))
            {
                _ability.HitThisFighter(target);
            }
        }
    }
}