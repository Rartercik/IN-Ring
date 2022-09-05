namespace Game.Enemies.States
{
    public abstract class EnemyState
    {
        protected readonly EnemyIntelligence Intelligence;

        public EnemyState(EnemyIntelligence enemyIntelligence)
        {
            Intelligence = enemyIntelligence;
        }

        public void FixedUpdateLogic()
        {
            UpdateLogic();

            TrySwitchState();
        }

        protected abstract void UpdateLogic();

        protected abstract void TrySwitchState();
    }
}