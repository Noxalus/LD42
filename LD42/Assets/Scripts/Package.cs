using UnityEngine;

public class Package : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public BoxCollider2D PhysicsBox;

    private bool _pickingUp;
    private bool _pickedUp;
    private bool _canColliderWithPlayer;
    private int _ignoredByPlayerLayer;

    // Audio
    public AudioClip FallSound;
    public AudioClip PickupSound;

    private void Start()
    {
        _pickingUp = false;
        _pickedUp = false;
        _canColliderWithPlayer = false;
        _ignoredByPlayerLayer = LayerMask.NameToLayer("PlayerIgnoredPackage");
    }

    void Update ()
    {
        if (!_pickingUp && _pickedUp && _canColliderWithPlayer && PhysicsBox.gameObject.layer == _ignoredByPlayerLayer)
        {
            PhysicsBox.gameObject.layer = LayerMask.NameToLayer("Package");
            _pickedUp = false;
        }
    }

    public void PickedUp()
    {
        GameManager.Instance().PlaySound(PickupSound);
        _pickingUp = true;
        _canColliderWithPlayer = false;
        PhysicsBox.gameObject.layer = _ignoredByPlayerLayer;
    }

    public void Released()
    {
        _pickingUp = false;
        _pickedUp = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (collision.relativeVelocity.magnitude > 2.5f)
                GameManager.Instance().PlaySound(FallSound);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _pickedUp)
        {
            _canColliderWithPlayer = true;
        }
    }
}
