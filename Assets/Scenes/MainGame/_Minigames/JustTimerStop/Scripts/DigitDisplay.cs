using UnityEngine;

namespace Sakemottekoi.Minigame.JustTimerStop
{
    public class DigitDisplay : MonoBehaviour
    {
        [SerializeField] private Sprite[] digitSprites;
        private SpriteRenderer digitRenderer;

        private void Awake()
        {
            digitRenderer = GetComponent<SpriteRenderer>();
        }

        public void UpdateDisplay(int number)
        {
            digitRenderer.sprite = digitSprites[number];
        }
    }
}