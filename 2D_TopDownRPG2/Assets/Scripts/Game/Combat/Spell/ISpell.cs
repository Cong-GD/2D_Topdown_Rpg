using CongTDev.ObjectPooling;
using UnityEngine;

namespace CongTDev.AbilitySystem.Spell
{
    public interface ISpell : IPoolObject
    {
        void KickOff(OrientationAbility ability, Vector2 direction);
    }


}

