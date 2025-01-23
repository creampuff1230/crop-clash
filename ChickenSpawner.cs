using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    public GameObject chickenPrefab; // this is our chicken model with our animations (Maya Autocad)
    public int numberOfChickens = 5000; // number of chickens
    private Vector3 spawnAreaSize = new(200, 0, 200); //  spawning area size

    void Start()
    {
        SpawnChickens();
    }

    private void SpawnChickens()
    {
        for (int i = 0; i < numberOfChickens; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            GameObject newChicken = Instantiate(chickenPrefab, spawnPosition, Quaternion.identity);

            float scaleMultiplier = Random.Range(0, 2) == 0 ? 2f : 1f; // vary size (sometimes 2x scale)
            newChicken.transform.localScale *= scaleMultiplier;
        }
    }

    private Vector3 GetRandomPosition()
        // randomize position over terrain
    {
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        return new Vector3(x, transform.position.y, z);
    }
}
