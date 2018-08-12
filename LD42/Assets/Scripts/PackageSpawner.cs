using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    public List<GameObject> PackagePrefabs;
    public float StartDelay;
    public Animator Animator;
    public SpriteRenderer Sprite;

    public List<float> SpawnFrequencyMin;
    public List<float> SpawnFrequencyMax;

    private List<Color> difficultyToColor;

    private bool _disabled;
    private bool _started;

    private int _difficultyLevel = 0;

    public void Start()
    {
        _disabled = false;
        _started = false;

        difficultyToColor = new List<Color>()
        {
            new Color(0.23f, 0.54f, 0.09f),
            new Color(0.96f, 0.82f, 0.16f),
            new Color(0.95f, 0.60f, 0.11f),
            new Color(0.95f, 0.04f, 0.09f)
        };

        Sprite.color = difficultyToColor[_difficultyLevel];
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

            yield return new WaitForSeconds(Random.Range(SpawnFrequencyMin[_difficultyLevel], SpawnFrequencyMax[_difficultyLevel]));
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

    public void SetDifficultyLevel(int level)
    {
        _difficultyLevel = level;
        Sprite.color = difficultyToColor[_difficultyLevel];
    }
}
