using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public class AlcoholSelectionManager : InputPhaseManager<AlcoholSelectionManager, AlcoholGlass>
    {
        private AlcoholGlass selectedGlass;

        protected override void OnPhaseStart()
        {
            AlcoholGlass.OnClicked += SelectGlass;
            selectedGlass = null;
        }

        private void SelectGlass(AlcoholGlass glass)
        {
            selectedGlass = glass;
        }

        protected override void OnTryComplete ()
        {
            if (selectedGlass == null)
            {
                Debug.LogError("グラスが選択されていません！");
                GameLogUI.Instance.AppendErrorLog("グラスが選択されていません！");
                return;
            }

            CompletePhase(selectedGlass);
        }
    }
}