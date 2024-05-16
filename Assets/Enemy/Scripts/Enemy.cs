using UnityEngine;

namespace EnemySpace
{
    public class Enemy: MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _deathCollider;
        [SerializeField] private BoxCollider2D _damageCollider;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.otherCollider == _deathCollider)
            {
                gameObject.SetActive(false);
                // event?
            }
            else if(other.otherCollider == _damageCollider)
            {
                Debug.Log("Damage");
            }
        }
    }
}