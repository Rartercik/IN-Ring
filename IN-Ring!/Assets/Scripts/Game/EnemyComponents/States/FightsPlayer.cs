namespace Game.EnemyComponents.States
{
    public class FightsPlayer : EnemyState
    {
        public FightsPlayer(EnemyIntelligence enemyIntelligence) : base(enemyIntelligence) { }

        protected override void UpdateLogic()
        {
            Intelligence.Body.Punch();
        }

        protected override void TrySwitchState()
        {
            var correctDistance = Intelligence.CheckDistanceCorrectness();
            var correctRotation = Intelligence.CheckRotationCorrectness();

            if (correctDistance == false || correctRotation == false && Intelligence.IsAttacking() == false)
            {
                Intelligence.SwitchState<PursuesPlayer>();
            }
        }
    }
}