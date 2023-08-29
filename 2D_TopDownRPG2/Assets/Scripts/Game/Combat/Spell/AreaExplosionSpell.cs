using CongTDev.AudioManagement;
using CongTDev.ObjectPooling;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class AreaExplosionSpell : PoolObject, ISpell
    {
        [SerializeField] private float exploreRadius;
        private OrientationAbility _ability;

        public void KickOff(OrientationAbility ability, Vector2 _)
        {
            _ability = ability;
            transform.position = ability.Caster.Owner.Position;
            AudioManager.Play("MediumExplosion");
            
        }

        public void Explore()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, exploreRadius, LayerMaskHelper.FigherMask);
            foreach (var hit in hits)
            {
                if(hit.TryGetComponent<Fighter>(out var fighter) && _ability.IsRightTarget(fighter))
                {
                    _ability.HitThisFighter(fighter);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, exploreRadius);
        } 
#endif
    }

}

