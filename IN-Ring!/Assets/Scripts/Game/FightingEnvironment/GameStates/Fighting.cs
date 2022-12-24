namespace Game.FightingEnvironment.GameStates
{
    public class Fighting : GameState
    {
        public Fighting(GameStatesHandler statesHandler) : base(statesHandler) { }

        public override void Enter()
        {

        }

        public override void ReportPlayerVictory()
        {
            StatesHandler.SwitchState<PlayerVictory>();
        }

        public override void ReportPlayerDeafeat()
        {
            StatesHandler.SwitchState<PlayerDefeat>();
        }
    }
}
