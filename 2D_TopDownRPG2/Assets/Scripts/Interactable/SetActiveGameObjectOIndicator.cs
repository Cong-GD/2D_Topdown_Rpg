public class SetActiveGameObjectIndicator : BaseIndicator
{
    public override void Active()
    {
        gameObject.SetActive(true);
    }

    public override void Deactive()
    {
        gameObject.SetActive(false);
    }
}
