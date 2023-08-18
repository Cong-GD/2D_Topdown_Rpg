namespace CongTDev.AbilitySystem
{
    public interface IActiveAbility : IAbility
    {
        float CastDelay { get; }

        float MaxUseRange { get; }

        float CurrentCoolDown { get; }

        float GetCooldownTime();
        bool IsEnoughMana();
        bool IsInstantCast();
        bool IsReady();
        Respond TryUse();
    }

}

