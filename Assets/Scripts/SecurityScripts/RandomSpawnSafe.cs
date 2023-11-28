using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script should be attached on SpawnSafePosition object on scene
public class RandomSpawnSafe : MonoBehaviour
{
    public Transform[] spawnPosition;
    public GameObject safePrevab;

    public Transform safeLocation;

    private void Start()
    {
        SpawnSafeRandomPosition();
    }

    public void SpawnSafeRandomPosition()
    {
        safeLocation = spawnPosition[Random.Range(0, spawnPosition.Length)];
        Instantiate(safePrevab, safeLocation);
    }
}
