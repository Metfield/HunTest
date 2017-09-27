using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint
{
    Enemy enemy;
    bool isSpawned;

    public void Spawn(GameObject enemyGameObject, int enemyHealth, GameObject spawnPoint, Main main)
    {
        // Move to position
        enemyGameObject.transform.position = spawnPoint.transform.position;

        enemy = new Enemy(main, enemyHealth, enemyGameObject, spawnPoint.transform.Find("PatrolLeftBound").gameObject.transform.position, spawnPoint.transform.Find("PatrolRightBound").gameObject.transform.position);

        // Update enemy patrol bounds
        enemy.SetPatrolBounds(spawnPoint.transform.Find("PatrolLeftBound").gameObject.transform.position,
                              spawnPoint.transform.Find("PatrolRightBound").gameObject.transform.position);

        isSpawned = true;
    }
}

public class EnemySpawner : MonoBehaviour
{
    List<SpawnPoint> spawnPoints;
    Main main;
    int enemyHealth;

    public void Init(Main inMain, int inEnemyHealth)
    {
        // Init variables
        spawnPoints = new List<SpawnPoint>();
        main = inMain;
        enemyHealth = inEnemyHealth;

        // Get spawnpoints in scene
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");

        // Spawn them enemies!
        SpawnPoint newSpawnPoint;

        foreach (GameObject spawnPoint in spawnPointObjects)
        {
            newSpawnPoint = new SpawnPoint();
            newSpawnPoint.Spawn(Instantiate(Resources.Load<GameObject>("Enemies/Prefabs/Enemy")), enemyHealth, spawnPoint, main);
            spawnPoints.Add(newSpawnPoint);
        }
    }

    public void Run()
    {

    }

    /*GameObject enemy = Instantiate(Resources.Load<GameObject>("Enemies/Prefabs/Enemy"));
    enemy.transform.position = spawnPoints[0].transform.position;*/
}
