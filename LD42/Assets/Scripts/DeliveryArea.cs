using UnityEngine;

public class DeliveryArea : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance().GameIsOver())
            return;

        if (collision.tag == "Package" && collision.name == "PhysicsBox")
        {
            Destroy(collision.transform.parent.gameObject);
            GameManager.Instance().IncreaseScore(1);
        }
        else if (collision.tag == "Player")
        {
            GameManager.Instance().GameOver();
        }
    }
}
