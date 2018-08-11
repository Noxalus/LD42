using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    public Rigidbody2D RigidBody;

    private List<GameObject> _nearPackages = new List<GameObject>();
    private GameObject _nearestPackage;
    private bool _dirty = false;

    void Start ()
    {
        _nearPackages = new List<GameObject>();
    }
	
	void Update ()
    {
        if (_dirty)
        {
            _nearestPackage = FindNearestPackage();
        }

        if (_nearestPackage)
        {
            var posX = transform.position.x - _nearestPackage.transform.position.x;
            var posY = transform.position.y - _nearestPackage.transform.position.y;
            var angle = (-Mathf.Atan2(posX, posY) * Mathf.Rad2Deg) + 180f;

            RigidBody.rotation = angle;
        }
    }

    GameObject FindNearestPackage()
    {
        _dirty = false;

        if (_nearPackages.Count == 0)
            return null;

        var nearestPackage = _nearPackages[0];
        var nearestDistance = Vector2.Distance(transform.position, nearestPackage.transform.position);
        foreach (var package in _nearPackages)
        {
            var currentDistance = Vector2.Distance(transform.position, package.transform.position);

            if (currentDistance < nearestDistance)
            {
                nearestPackage = package;
                nearestDistance = currentDistance;
            }
        }

        return nearestPackage;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Package")
        {
            _nearPackages.Add(collision.gameObject);
            _dirty = true;
            Debug.Log("A package is detected!");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Package")
        {
            _nearPackages.Remove(collision.gameObject);
            Debug.Log("A package is out!");
            _dirty = true;
        }
    }
}
