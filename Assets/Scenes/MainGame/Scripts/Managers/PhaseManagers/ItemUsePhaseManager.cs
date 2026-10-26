using UnityEngine;
using System;

namespace Sakemottekoi.MainGame
{
    public class ItemUsePhaseManager : InputPhaseManager<ItemUsePhaseManager>
    {
        [SerializeField] private ItemUseForm form;
        private bool isBusy;

        protected override void OnPhaseStart()
        {
            Item.OnClicked += TryItemUse;
        }

        private async void TryItemUse(Item item)
        {
            isBusy = true;

            try
            {
                var isUse = await form.ShowAsync(item);
                if (!isUse) return;

                await item.Use(GameManager.Instance.Player);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                isBusy = false;
            }
        }

        protected override void EndPhase()
        {
            if (isBusy) return;
            Item.OnClicked -= TryItemUse;
            base.EndPhase();
        }
    }
}