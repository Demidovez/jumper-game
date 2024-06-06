using UnityEngine;

public class Fruit : MonoBehaviour
{
    public delegate void OnFruitCollision(GameObject obj, GameObject other);
    public static event OnFruitCollision OnFruitCollisionEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnFruitCollisionEvent?.Invoke(gameObject, other.gameObject);
    }
}
