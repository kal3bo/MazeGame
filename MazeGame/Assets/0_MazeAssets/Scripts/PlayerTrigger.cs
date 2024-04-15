using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public delegate void TriggerEvent(Collider other);
    public event TriggerEvent OnPlayerEnterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnterTrigger?.Invoke(other);
        }
    }
}
