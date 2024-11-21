using UnityEngine;

public class PoolReturner : MonoBehaviour
{
    [SerializeField] private InteractableObjectSpawner _spawner;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out InteractableObject @object))
        {
            _spawner.ReturnToPool(@object);
        }
    }
}
