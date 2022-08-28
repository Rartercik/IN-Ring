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

        public override void FixedUpdateLogic()
        {
            Intelligence.Body.Punch();

            var vectorToPlayer = Intelligence.PlayerTransform.position - _transform.position;

            var correctDistance = Intelligence.CheckDistanceCorrectness(vectorToPlayer);
            var correctRotation = Intelligence.CheckRotationCorrectness(vectorToPlayer);

            if (correctDistance == false || correctRotation == false)
            {
                Intelligence.SwitchState<PursuesPlayer>();
            }
        }
    }
}