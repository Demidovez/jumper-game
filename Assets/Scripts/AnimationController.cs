using BulletSpace;
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
        
        private void OnEnable()
        {
            Bullet.OnBulletDestroyEvent += OnBulletDestroy;
        }

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

        private void OnBulletDestroy(GameObject obj)
        {
            Animator animator = obj.GetComponent<Animator>();
                
            animator.SetBool("IsDestroyed",  true);
        }
        
        private void OnDisable()
        {
            Bullet.OnBulletDestroyEvent -= OnBulletDestroy;
        }
    }

}
