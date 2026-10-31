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
            if(selectedGlass)
                selectedGlass.gameObject.layer = LayerMask.NameToLayer("Default");

            selectedGlass = glass;
            selectedGlass.gameObject.layer = LayerMask.NameToLayer("Outline");
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
            selectedGlass.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}