using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;          // Speed of bullet
    public float maxLifeTime = 2f;     // How long before bullet destroys itself

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        Destroy(gameObject, maxLifeTime);
    }

    void FixedUpdate()
    {
        Vector3 moveDistance = transform.forward * speed * Time.fixedDeltaTime;

        RaycastHit hit;
        if (Physics.Raycast(lastPosition, moveDistance.normalized, out hit, moveDistance.magnitude))
        {
            Debug.Log("Bullet hit: " + hit.collider.name);

            // Add impact logic here (damage, effects, etc.)

            Destroy(gameObject);
        }
        else
        {
            transform.position += moveDistance;
            lastPosition = transform.position;
        }
    }
}
