using TMPro;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class GameLogUI : SingletonObject<GameLogUI>
    {
        [SerializeField] TextMeshProUGUI logText;

        public void AppendErrorLog(string message)
        {
            TextMeshProUGUI log = Instantiate(logText);
            log.color = Color.red;
            log.text = message;
            log.transform.SetParent(transform, false);
            log.transform.localPosition = Vector3.zero;

            Destroy(log.gameObject, 3f);
        }
    }
}