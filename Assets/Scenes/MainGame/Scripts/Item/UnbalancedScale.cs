using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    [System.Serializable]
    public class UnbalancedScaleParticleSetting
    {
        public DrunkennessLevel level;
        public float particleScale;
    }


    public class UnbalancedScale : Item
    {
        public override string DisplayName => "揺れている天秤";

        public override string SimpleDescription => "提示された酒の中で、一番強い酒もしくは一番弱い酒が分かる";

        public override string Description => "提示された酒の中で、一番強い酒もしくは一番弱い酒が分かる";

        [SerializeField] private ParticleSystem particle;

        [SerializeField] private List<UnbalancedScaleParticleSetting> particleSettings;

        protected override UniTask InternalUse(Actor source)
        {
            List<AlcoholGlass> glasses = GameObject
                                            .FindGameObjectsWithTag("Glass")
                                            .Select(v => v.GetComponent<AlcoholGlass>())
                                            .ToList();
            // 一番弱い酒の場合
            if (Random.value < 0.5)
            {
                var min = glasses.Select(v => v.Amount).Min();
                glasses.RemoveAll(v => v.Amount > min);
            }
            // 一番強い酒の場合
            else
            {
                var max = glasses.Select(v => v.Amount).Max();
                glasses.RemoveAll(v => v.Amount < max);
            }

            // 一回で与えられる最大の酔い値
            var maxAmount = GameManager.Instance.DrunkennessSettings.GetAmount(DrunkennessLevel.VeryHigh);

            // パーティクルを出す処理
            foreach (var glass in glasses)
            {
                float particleScale = particleSettings.Find(v => v.level == glass.Level)?.particleScale ?? 1f;

                var p = Instantiate(particle, glass.transform);
                p.transform.localPosition = Vector3.up * 0.8f;
                p.transform.localScale = Vector3.one * particleScale;
                Destroy(p, 2.5f);
            }
            return UniTask.CompletedTask;
        }
    }
}