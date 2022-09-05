using UnityEngine;

namespace Game.Enemies.States
{
    public class FightsPlayer : EnemyState
    {
        private readonly Transform _transform;
        
        public FightsPlayer(EnemyIntelligence enemyIntelligence) : base(enemyIntelligence)
        {
            _transform = enemyIntelligence.Body.transform;
        }

        protected override void UpdateLogic()
        {
            Intelligence.Body.Punch();
        }

        protected override void TrySwitchState()
        {
            var vectorToPlayer = Intelligence.PlayerTransform.position - _transform.position;

            var correctDistance = Intelligence.CheckDistanceCorrectness(vectorToPlayer);
            var correctRotation = Intelligence.CheckRotationCorrectness(vectorToPlayer);

            if (correctDistance == false || correctRotation == false && Intelligence.IsAttacking() == false)
            {
                Intelligence.SwitchState<PursuesPlayer>();
            }
        }
    }
}