using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public delegate void OnCheckpointCollision(GameObject other);
    public static event OnCheckpointCollision OnCheckpointCollisionEvent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        OnCheckpointCollisionEvent?.Invoke(other.gameObject);
    }
}
