using System.Collections;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class AlcholSelectionManager : MonoBehaviour
    {
        public static AlcholSelectionManager Instance { private set; get; }
        public bool IsAlcholSelected { private set; get; } = false;

        private void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}