public class ScullShot : Item
{
    public override void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        alcohol *= 1.5f;
    }
}
