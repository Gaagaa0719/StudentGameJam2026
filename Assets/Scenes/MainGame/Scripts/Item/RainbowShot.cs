public class RainbowShot : Item
{
    public override void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        amount *= 1.1f; // 酔い度を10%上昇
    }
}
