using UnityEngine;

public class PlayerArmsManager : MonoBehaviour
{
    public PackageDetector PackageDetector;
    public Rigidbody2D LeftArmRigidBody;
    public Rigidbody2D RightArmRigidBody;

    private GameObject _nearestPackage;

    public void Start()
    {
        PackageDetector.OnNearestPackageChanged.AddListener(SetNearestPackage);
    }

    public void SetNearestPackage(GameObject package)
    {
        _nearestPackage = package;
    }

    public void Update()
    {
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
}
