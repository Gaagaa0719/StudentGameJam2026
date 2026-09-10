public class DoroShot : Item
{
    public override void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        alcohol += 16;
    }
}
