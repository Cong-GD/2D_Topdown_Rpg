namespace CongTDev.AbilitySystem
{
    public abstract class BaseEffectAndFactory : BaseEffectFactory, IEffect
    {
        public virtual void CleanUp() { }
        public abstract void Instanciate(Fighter source, Fighter target);

        public override IEffect Build()
        {
            return this;
        }
    }
}