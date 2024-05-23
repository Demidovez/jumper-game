using EnemySpace;
using PlayerSpace;
using UnityEngine;

namespace AnimationSpace
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        
        private Animator _animatorPlayer;
        private Animator _animatorEnemy;

        private void Start()
        {
            _animatorPlayer = Player.Instance.GetComponent<Animator>();
            _animatorEnemy = _enemy.GetComponent<Animator>();
        }

        private void Update()
        {
            _animatorPlayer.SetBool("isMove",  Player.Instance.IsMoving);
            _animatorPlayer.SetBool("isGrounded",  Player.Instance.IsGrounded);
            _animatorPlayer.SetFloat("velocityY",  Player.Instance.GetVelocityY());
            _animatorPlayer.SetBool("isDead",  Player.Instance.IsDead);
            
            _animatorEnemy.SetBool("isKilledSomeone",  _enemy.IsKilledSomeone);
        }
    }

}
