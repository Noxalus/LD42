using UnityEngine;

public class PlayerArmsManager : MonoBehaviour
{
    public PackageDetector PackageDetector;
    public Rigidbody2D LeftArmRigidBody;
    public Rigidbody2D RightArmRigidBody;
    public Transform LeftHand;
    public Transform RightHand;

    private GameObject _nearestPackage;

    // Picking up
    private bool _pickingUp;
    private bool _isInsideHands;
    private GameObject _pickedObject;
    private TargetJoint2D _pickupTargetJoint;

    public void Start()
    {
        PackageDetector.OnNearestPackageChanged.AddListener(SetNearestPackage);
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

        _pickupTargetJoint = package.GetComponent<TargetJoint2D>();

        if (!_pickupTargetJoint)
            _pickupTargetJoint = package.AddComponent<TargetJoint2D>();

        _pickupTargetJoint.target = GetPickupPosition();

        _pickingUp = true;
        _pickedObject = package;

        // Disable collision box
        package.GetComponent<BoxCollider2D>().enabled = false;
    }

    public Vector2 GetPickupPosition()
    {
        return LeftHand.position + (RightHand.position - LeftHand.position) / 2;
    }

    public void ReleasePackage()
    {
        var forceDirection = (_pickedObject.transform.position - PackageDetector.transform.position);
        forceDirection.y += 0.5f;
        forceDirection.Normalize();

        if (_isInsideHands)
        {
            _pickedObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 500f);
        }

        // Disable collision box
        _pickedObject.GetComponent<BoxCollider2D>().enabled = true;

        Destroy(_pickupTargetJoint);
        _pickingUp = false;
        _isInsideHands = false;
    }
}
