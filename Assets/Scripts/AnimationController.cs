using EnemySpace;
using PlayerSpace;
using UnityEngine;

namespace AnimationSpace
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Enemy _enemy;
        
        private Animator _animatorPlayer;
        private Animator _animatorEnemy;

        private void Start()
        {
            _animatorPlayer = _player.GetComponent<Animator>();
            _animatorEnemy = _enemy.GetComponent<Animator>();
        }

        private void Update()
        {
            _animatorPlayer.SetBool("isMove",  _player.IsMoving);
            _animatorPlayer.SetBool("isGrounded",  _player.IsGrounded);
            _animatorPlayer.SetFloat("velocityY",  _player.GetVelocityY());
            _animatorPlayer.SetBool("isDead",  _player.IsDead);
            
            _animatorEnemy.SetBool("isKilledSomeone",  _enemy.IsKilledSomeone);
        }
    }

}
