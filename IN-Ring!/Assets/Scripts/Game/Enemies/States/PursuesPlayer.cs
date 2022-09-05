using UnityEngine;

namespace Game.Enemies.States
{
    public class PursuesPlayer : EnemyState
    {
        private readonly Transform _transform;
        private readonly float _rotationDelta;

        public PursuesPlayer(EnemyIntelligence enemyIntelligence, float rotationDelta)
            : base(enemyIntelligence)
        {
            _transform = enemyIntelligence.Body.transform;
            _rotationDelta = rotationDelta;
        }

        protected override void UpdateLogic()
        {
            var vectorToPlayer = Intelligence.PlayerTransform.position - _transform.position;

            var correctDistance = Intelligence.CheckDistanceCorrectness(vectorToPlayer);
            var correctRotation = Intelligence.CheckRotationCorrectness(vectorToPlayer);

            var dotProduct = Vector3.Dot(_transform.rotation * Vector3.left, vectorToPlayer);
            var rotationDelta = dotProduct > 0 ? -_rotationDelta : _rotationDelta;

            PursuePlayer(vectorToPlayer, rotationDelta, correctDistance, correctRotation);
        }

        protected override void TrySwitchState()
        {
            var vectorToPlayer = Intelligence.PlayerTransform.position - _transform.position;

            var correctDistance = Intelligence.CheckDistanceCorrectness(vectorToPlayer);
            var correctRotation = Intelligence.CheckRotationCorrectness(vectorToPlayer);

            if (correctDistance && correctRotation)
            {
                Intelligence.Body.Stop();
                Intelligence.SwitchState<FightsPlayer>();
            }
        }

        private void PursuePlayer(Vector3 vectorToPlayer, float rotationDelta, bool correctDistance, bool correctRotation)
        {
            vectorToPlayer.y = 0;

            if (correctDistance == false)
            {
                Intelligence.Body.Move(vectorToPlayer, true);
            }

            if (correctRotation == false)
            {
                Intelligence.Body.Rotate(rotationDelta);
            }
        }
    }
}