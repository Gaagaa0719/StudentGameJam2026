using UnityEngine;

public class BunbbleShot : Item
{
    public override void ChangeGlassParams(ref float alcohol, ref float amount)
    {
        alcohol += Random.Range(12f, 20f);
    }
}
