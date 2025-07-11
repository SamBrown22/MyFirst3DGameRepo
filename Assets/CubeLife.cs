using UnityEngine;

public class CubeLife : MonoBehaviour
{
    public string targetTag = "Bullet"; //test change

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Destroy(gameObject); // Destroys the cube itself 
        }
    }
}
