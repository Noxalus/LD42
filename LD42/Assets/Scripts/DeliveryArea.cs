using UnityEngine;

public class DeliveryArea : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Package" && collision.name == "PhysicsBox")
        {
            Destroy(collision.transform.parent.gameObject);
            GameManager.Instance().IncreaseScore(1);
        }
    }
}
