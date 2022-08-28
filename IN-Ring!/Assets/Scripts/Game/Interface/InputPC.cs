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
            if (Input.GetKey(KeyCode.W))
            {
                _player.Move(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.A))
            {
                _player.Move(Vector3.left);
            }
            if (Input.GetKey(KeyCode.S))
            {
                _player.Move(Vector3.back);
            }
            if (Input.GetKey(KeyCode.D))
            {
                _player.Move(Vector3.right);
            }
        }

        private void RotatePlayer()
        {
            var yRotation = Input.GetAxisRaw("Mouse X");

            _player.Rotate(yRotation);
        }
    }
}
