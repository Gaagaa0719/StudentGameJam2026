namespace Sakemottekoi.MainGame
{
    public class ItemUsePhaseManager : PhaseManager<ItemUsePhaseManager>
    {
        protected override GamePhase TargetPhase => GamePhase.ItemUse;
    }
}