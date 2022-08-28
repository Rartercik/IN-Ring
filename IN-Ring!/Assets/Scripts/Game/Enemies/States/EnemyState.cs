namespace Game.Enemies.States
{
    public abstract class EnemyState
    {
        protected readonly EnemyIntelligence Intelligence;

        public EnemyState(EnemyIntelligence enemyIntelligence)
        {
            Intelligence = enemyIntelligence;
        }

        public abstract void FixedUpdateLogic();
    }
}