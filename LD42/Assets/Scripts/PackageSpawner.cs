using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    public List<GameObject> PackagePrefabs;
    public float StartDelay;

    private bool _disabled;

    public void Start()
    {
        _disabled = false;
        StartCoroutine(SpawnPackage());
    }

    private IEnumerator SpawnPackage()
    {
        yield return new WaitForSeconds(StartDelay);

        while (!_disabled)
        {
            Instantiate(PackagePrefabs[Random.Range(0, PackagePrefabs.Count)], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
    }
}
