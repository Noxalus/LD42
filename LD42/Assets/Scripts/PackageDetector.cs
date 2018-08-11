using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectEvent : UnityEvent<GameObject> {}

public class PackageDetector : MonoBehaviour
{
    public UnityEvent<GameObject> OnNearestPackageChanged = new GameObjectEvent();

    private List<GameObject> _nearPackages = new List<GameObject>();
    private bool _dirty = false;

    void Start()
    {
        _nearPackages = new List<GameObject>();
    }

    void Update()
    {
        if (_dirty)
        {
            OnNearestPackageChanged.Invoke(FindNearestPackage());
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
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Package")
        {
            _nearPackages.Remove(collision.gameObject);
            _dirty = true;
        }
    }
}
