using PlayerSpace;
using UnityEngine;

namespace AnimationSpace
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        
        private Animator _animatorPlayer;

        private void Start()
        {
            _animatorPlayer = _player.GetComponent<Animator>();
        }

        private void Update()
        {
            _animatorPlayer.SetBool("isMove",  _player.IsMoving);
        }
    }

}
