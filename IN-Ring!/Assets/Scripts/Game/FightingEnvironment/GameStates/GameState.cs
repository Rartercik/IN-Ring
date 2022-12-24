namespace Game.FightingEnvironment.GameStates
{
    public abstract class GameState
    {
        public GameState(GameStatesHandler statesHandler)
        {
            StatesHandler = statesHandler;
        }

        protected GameStatesHandler StatesHandler { get; private set; }

        public abstract void Enter();

        public abstract void ReportPlayerVictory();

        public abstract void ReportPlayerDeafeat();
    }
}
