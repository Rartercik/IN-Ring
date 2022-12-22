using UnityEngine;
using Game.BodyComponents;

namespace Game.Interface
{
    public class InputPC : MonoBehaviour
    {
        [SerializeField] private Body _player;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _player.TryJump();
            }

            if (Input.GetMouseButtonDown(0))
            {
                _player.Punch();
            }
        }

        private void FixedUpdate()
        {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == false)
            {
                _player.Stop();
            }

            MovePlayer();

            RotatePlayer();
        }

        private void MovePlayer()
        {
            var moveDirection = GetActualDirection();

            _player.Move(moveDirection);
        }

        private void RotatePlayer()
        {
            var yRotation = Input.GetAxisRaw("Mouse X");

            _player.Rotate(yRotation);
        }

        private Vector3 GetActualDirection()
        {
            var moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection += Vector3.back;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += Vector3.right;
            }

            return moveDirection;
        }
    }
}
