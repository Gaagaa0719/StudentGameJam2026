using System.Collections;
using System.Linq;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class UnbalancedScale : Item
    {
        public override string DisplayName => "揺れている天秤";

        public override string SimpleDescription => "提示された酒の中で、一番強い酒もしくは一番弱い酒が分かる";

        public override string Description => "提示された酒の中で、一番強い酒もしくは一番弱い酒が分かる";

        protected override IEnumerator InternalUse(Actor source)
        {
            AlcoholGlass[] glasses = GameObject.FindGameObjectsWithTag("AlcoholGlass").Select(v => v.GetComponent<AlcoholGlass>()).ToArray();
            if(Random.value < 0.5f)

            foreach (var glass in glasses)
            {
                
            }
            yield break;
        }
    }
}