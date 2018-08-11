using UnityEngine;

public class PlayerArmsManager : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody2D LeftArmRigidBody;
    public Rigidbody2D RightArmRigidBody;
    public Transform LeftHand;
    public Transform RightHand;

    private PackageDetector _packageDetector;
    private CharacterController2D _playerController;
    private Rigidbody2D _playerRigidBody;
    private GameObject _nearestPackage;

    // Picking up
    private bool _pickingUp;
    private bool _isInsideHands;
    private GameObject _pickedObject;
    private TargetJoint2D _pickupTargetJoint;

    public void Start()
    {
        _packageDetector = Player.GetComponent<PackageDetector>();
        _playerController = Player.GetComponent<CharacterController2D>();
        _playerRigidBody = Player.GetComponent<Rigidbody2D>();
        
        _packageDetector.OnNearestPackageChanged.AddListener(SetNearestPackage);
    }

    public void SetNearestPackage(GameObject package)
    {
        if (_pickingUp)
            return;

        _nearestPackage = package;
    }

    public void Update()
    {
        // Pickup nearest package
        if (Input.GetButtonDown("Fire1"))
        {
            PickupPackage(_nearestPackage);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            ReleasePackage();
        }

        if (_pickingUp)
        {
            if (_pickupTargetJoint)
            {
                var pickupPosition = GetPickupPosition();
                _pickupTargetJoint.target = pickupPosition;

                var distance = Vector2.Distance(_pickedObject.transform.position, pickupPosition);
                Debug.Log("Distance: " + distance);
                _isInsideHands = distance < 0.7f;
            }
        }

        if (_nearestPackage)
        {
            var leftArmDirectionX = LeftArmRigidBody.position.x - _nearestPackage.transform.position.x;
            var leftArmDirectionY = LeftArmRigidBody.position.y - _nearestPackage.transform.position.y;
            var leftArmAngle = (-Mathf.Atan2(leftArmDirectionX, leftArmDirectionY) * Mathf.Rad2Deg) + 180f;
            LeftArmRigidBody.rotation = leftArmAngle;

            var rightArmDirectionX = LeftArmRigidBody.position.x - _nearestPackage.transform.position.x;
            var rightArmDirectionY = LeftArmRigidBody.position.y - _nearestPackage.transform.position.y;
            var rightArmAngle = (-Mathf.Atan2(rightArmDirectionX, rightArmDirectionY) * Mathf.Rad2Deg) + 180f;
            RightArmRigidBody.rotation = rightArmAngle;
        }
    }

    public void PickupPackage(GameObject package)
    {
        if (!package)
            return;

        package = package.transform.parent.gameObject;

        _pickupTargetJoint = package.GetComponent<TargetJoint2D>();

        if (!_pickupTargetJoint)
            _pickupTargetJoint = package.AddComponent<TargetJoint2D>();

        _pickupTargetJoint.target = GetPickupPosition();

        _pickingUp = true;
        _pickedObject = package;

        _pickedObject.GetComponent<Package>().PickedUp();
    }

    public Vector2 GetPickupPosition()
    {
        return LeftHand.position + (RightHand.position - LeftHand.position) / 2;
    }

    public void ReleasePackage()
    {
        if (!_pickingUp)
            return;

        if (_isInsideHands)
        {
            var forceDirection = Player.GetComponent<Rigidbody2D>().velocity;
            //forceDirection.y += 0.5f;

            if (forceDirection != Vector2.zero)
            {
                //forceDirection.Normalize();
                //_pickedObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 500f);
                _playerController.IsFacingRight();
                _pickedObject.GetComponent<Rigidbody2D>().velocity = Player.GetComponent<Rigidbody2D>().velocity;
            }
        }

        _pickedObject.GetComponent<Package>().Released();

        Destroy(_pickupTargetJoint);
        _pickedObject = null;

        _pickingUp = false;
        _isInsideHands = false;
    }
}
