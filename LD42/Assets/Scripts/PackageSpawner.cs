using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    public List<GameObject> PackagePrefabs;
    public float StartDelay;
    public Animator Animator;

    public List<float> SpawnFrequencyMin;
    public List<float> SpawnFrequencyMax;

    private bool _disabled;
    private bool _started;

    public void Start()
    {
        _disabled = false;
        _started = false;
    }

    private IEnumerator SpawnPackage()
    {
        yield return new WaitForSeconds(StartDelay);

        while (!_disabled)
        {
            if (GameManager.Instance().GameIsOver())
                break;

            Animator.SetTrigger("AddPackage");
            Instantiate(PackagePrefabs[Random.Range(0, PackagePrefabs.Count)], transform.position, Quaternion.identity);
            var difficultyLevel = GameManager.Instance().GetDifficultyLevel();
            yield return new WaitForSeconds(Random.Range(SpawnFrequencyMin[difficultyLevel], SpawnFrequencyMax[difficultyLevel]));
        }
    }

    public void Update()
    {
        if (!_started && Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            StartSpawner();
        }
    }

    private void StartSpawner()
    {
        _started = true;
        StartCoroutine(SpawnPackage());
    }
}
