using UnityEngine;

public class Respawnable : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        // Bewaar beginpositie
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Respawn()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}