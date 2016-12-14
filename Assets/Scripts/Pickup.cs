public class Pickup : Shootable
{
    public override void OnClick()
    {
        base.OnClick();

        Destroy(gameObject);
    }
}
