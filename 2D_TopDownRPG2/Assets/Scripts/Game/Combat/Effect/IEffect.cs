namespace CongTDev.AbilitySystem
{
    public interface IEffect
    {
        EffectInfo EffectInfo { get; }
        void Instanciate(Fighter source, Fighter target);
        void CleanUp();
    }
}