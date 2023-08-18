using CongTDev.ObjectPooling;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public class DirectionalExplosionStrightSpell : StraightExplosionBullet
    {
        protected override void Explosion(Vector2 position)
        {
            if (PoolManager.Get<IPoolObject>(explosionPrefab, out var instance))
            {
                instance.gameObject.transform.position = position;
                instance.gameObject.transform.rotation = transform.rotation;
                var hits = Physics2D.OverlapCircleAll(position, explosionRange, LayerMaskHelper.FigherMask);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<Fighter>(out var fighter))
                    {
                        if (friendlyHit)
                        {
                            ability.HitThisFighter(fighter);
                        }
                        else if (ability.IsRightTarget(fighter))
                        {
                            ability.HitThisFighter(fighter);
                        }

                    }
                }
            }

        }
    }
}