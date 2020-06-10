using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Spawner : MonoBehaviour
{

    public GameObject[] spawnees;
    public GameObject lastSpawnedObject = null;
    ObjectToSpawn lastSpawned = ObjectToSpawn.nothing;
    public Transform leftWall;
    public Transform rightWall;
    public float inRange = 5f;
    public Transform ground;
    public static int enemySpawnProbability = 40;
    bool doorsSpawned = false;



    #region Level Details
    static public float levelSpeed = 5f;
    static public float levelDistance = 150f;
    public static float levelTime;
    public bool levelEnded = false;
    public static bool newLevel = true;
    

    bool enemySpawnChecker()
    {
        float p = Random.Range(0, 100);
        if (p < enemySpawnProbability) return true;
        return false;
    }
    bool timerTicked(float timer)
    {
        return timer <= 0f;
    }
    void updateLevelTimer()
    {
        if(levelTime>=0f) levelTime -= Time.deltaTime;
    }
    float calculateLevelTime()
    {
        return levelDistance / levelSpeed;

    }
    public static void GenerateNewLevel()
    {
        newLevel = true;
        IncreaseLevelDistance();
        if (Spawner.enemySpawnProbability < 100) Spawner.enemySpawnProbability+=10;
    }
    static void IncreaseLevelDistance()
    {
        levelDistance += 5f;
    }
    bool manageTime()
    {
        if (newLevel)
        {
            levelEnded = false;
            levelTime = calculateLevelTime();
            newLevel = false;
        }
        updateLevelTimer();
        if (timerTicked(levelTime)&&!levelEnded)
        {
            newLevel = true;
            levelEnded = true;
            spawnExit();
            return true;
        }
        return false;
            
    }

    #endregion

    enum ObjectToSpawn
    {
        nothing = -1,
        wall = 0,//gotovo
        door = 1,//gotovo
        enemy = 2,//skoro gotovo
        mainDoors = 3,//delimicno gotovo
        chair = 4,//delimicno gotovo
        exitWalls  = 5,
        bed = 6
    }

    /// <summary>
    /// Converts degrees into radians
    /// </summary>
    /// <param name="degree">Value that should be converted</param>
    /// <returns>Given value in radian</returns>
    float degreeToRadian(float degree)
    {
        return degree/180;
    }
    /// <summary>
    /// Converts percentages to smaller values
    /// </summary>
    /// <param name="numberToConvert"></param>
    /// <returns></returns>
    int convertPercentages(int numberToConvert)
    {
        if (numberToConvert > 50) return 2;
        return 1;
    }

    /// <summary>
    /// Spawns a simple door
    /// </summary>
    /// <returns>Position of the spawned object</returns>
    GameObject spawnDoor()
    {
        int p = convertPercentages(Random.Range(1, 100));
        Vector3 posToSpawn = transform.position;
        Quaternion rotationToSpawn = transform.rotation;
        Vector3 enemySpawnPos;
        Quaternion enemySpawnRot = new Quaternion(rotationToSpawn.x,rotationToSpawn.y,rotationToSpawn.z,rotationToSpawn.w);
        switch (p)
        {
            case 1://spawns near the left wall
                posToSpawn.z = leftWall.position.z + leftWall.localScale.z/2f;
                rotationToSpawn *= Quaternion.AngleAxis(90f, Vector3.right);
                rotationToSpawn *= Quaternion.AngleAxis(180f, Vector3.up);
                if(enemySpawnChecker())
                {
                    enemySpawnPos = new Vector3(posToSpawn.x, posToSpawn.y-0.6f, posToSpawn.z + 0.5f);
                    Instantiate(spawnees[(int)ObjectToSpawn.enemy], enemySpawnPos, enemySpawnRot);
                }
                break;
            case 2://spawns near the right wall
                posToSpawn.z = rightWall.position.z - rightWall.localScale.z / 2f;
                rotationToSpawn *= Quaternion.AngleAxis(90f, Vector3.left);
                if(enemySpawnChecker())
                {
                    enemySpawnPos = new Vector3(posToSpawn.x, posToSpawn.y-0.6f, posToSpawn.z - 0.5f);
                    Instantiate(spawnees[(int)ObjectToSpawn.enemy], enemySpawnPos, enemySpawnRot);
                }
                break;
        }
        doorsSpawned = true;
        lastSpawned = ObjectToSpawn.door;
        posToSpawn.y -= 0.4f;
        
        return Instantiate(spawnees[(int)ObjectToSpawn.door],posToSpawn,rotationToSpawn);
    }
    GameObject spawnExit()
    {
        Vector3 posToSpawn = transform.position;
        Quaternion rotationToSpawn = transform.rotation;
        rotationToSpawn *= Quaternion.AngleAxis(90f, Vector3.left);
        lastSpawned = ObjectToSpawn.mainDoors;
        return Instantiate(spawnees[(int)ObjectToSpawn.mainDoors], posToSpawn, rotationToSpawn);
    }

    void SpawnEnemy(GameObject PotentialEnemy)
    {
        if (PotentialEnemy.tag == "Chair")
            return;
        if (Random.Range(1, 100) > enemySpawnProbability) return;

        Vector3 posToSpawn = PotentialEnemy.transform.position;
        Quaternion rotationToSpawn = PotentialEnemy.transform.rotation;
        if (PotentialEnemy.transform.position.z > 0)
            posToSpawn.z -= 0.3f;
        else posToSpawn.z += 0.3f;
        Instantiate(spawnees[(int)ObjectToSpawn.wall], posToSpawn, rotationToSpawn);

    }
    GameObject spawnWall()
    {
        int p = Random.Range(1, 100);
        p = convertPercentages(p);
        Vector3 posToSpawn = transform.position;
        Quaternion rotationToSpawn = transform.rotation;
        switch (p)
        {
            case 1:
                posToSpawn.z = leftWall.position.z + leftWall.localScale.z / 2f;
                break;
            case 2:
                posToSpawn.z = rightWall.position.z - rightWall.localScale.z / 2f;
                break;
        }
        lastSpawned = ObjectToSpawn.wall;
        return Instantiate(spawnees[(int)ObjectToSpawn.wall], posToSpawn, rotationToSpawn);
    }

    GameObject spawnChair()
    {
        int p = Random.Range(2, 5);
        int wall = convertPercentages(Random.Range(1, 100));
        GameObject t = null;
        Vector3 posToSpawn = transform.position;
        Quaternion rotationToSpawn = transform.rotation;
        for (int i=0;i<p;i++)
        {

            if(t==null)
            {
                Debug.Log("Proso");
                switch (wall)
                {
                    case 1:
                        posToSpawn.z = leftWall.position.z + leftWall.localScale.z / 2f +0.2f;// + spawnees[(int)ObjectToSpawn.chair].transform.localScale.z/2f;
                        rotationToSpawn *= Quaternion.AngleAxis(90f, Vector3.left);
                        break;
                    case 2:
                        posToSpawn.z = rightWall.position.z - rightWall.localScale.z / 2f - 0.2f;// - spawnees[(int)ObjectToSpawn.chair].transform.localScale.z / 2f;
                        rotationToSpawn *= Quaternion.AngleAxis(90f, Vector3.left);
                        rotationToSpawn *= Quaternion.AngleAxis(180f, Vector3.forward);
                        break;
                }
                t = Instantiate(spawnees[(int)ObjectToSpawn.chair], posToSpawn, rotationToSpawn);
            }
            else
            {
                posToSpawn = t.transform.position;
                rotationToSpawn = t.transform.rotation;
                posToSpawn.x -= t.transform.localScale.x;
                
                t = Instantiate(spawnees[(int)ObjectToSpawn.chair], posToSpawn, rotationToSpawn);
            }
        }
        lastSpawned = ObjectToSpawn.chair;
        return t;
    }


    GameObject spawnBed()
    {
        Vector3 posToSpawn = transform.position;
        Quaternion rotationToSpawn = transform.rotation;

        rotationToSpawn *= Quaternion.AngleAxis(90f, Vector3.left);
        lastSpawned = ObjectToSpawn.bed;
        int p = Random.Range(-10, 11);
        posToSpawn.z += (float)p / 10f;
        return Instantiate(spawnees[(int)ObjectToSpawn.bed], posToSpawn,rotationToSpawn);
    }

    Vector3 randomizePosition(Vector3 toRandomize)
    {
        float p = (int)Random.Range(-2f, +2f);
        toRandomize.x += p;
        toRandomize.y -= 0.625f;
        return toRandomize;
    }
    GameObject spawnEnemy()
    {
        Quaternion rotationToSpawn = transform.rotation;
        Vector3 positionToSpawn = transform.position;
        if(lastSpawned == ObjectToSpawn.door)
        {
            if (Random.Range(1, 100) < enemySpawnProbability*2)
                return Instantiate(spawnees[(int)ObjectToSpawn.enemy],lastSpawnedObject.transform);
        }
        if (Random.Range(1, 100) < enemySpawnProbability)
        {
            lastSpawned = ObjectToSpawn.enemy;
            return Instantiate(spawnees[(int)ObjectToSpawn.enemy],randomizePosition(positionToSpawn),rotationToSpawn);
        }
            
        return null;
    }

    bool outOfRange(float range)
    {
        if (lastSpawnedObject == null)
            return true;
        return Mathf.Abs(lastSpawnedObject.transform.position.x - transform.position.x) > range; 
    }

    void Update()
    {
        if (manageTime())
        {
            Debug.Log("manageTime");
            return;
        }
        if(outOfRange(inRange))
            spawnerManager();



    }

    ObjectToSpawn spawneePicker()
    {
        int p = Random.Range(0, 5);
        switch(p)
        {
            case 0:
                if (!outOfRange(7f))
                    return spawneePicker();
                lastSpawnedObject = spawnWall();
                return ObjectToSpawn.wall;
            case 1:
                lastSpawnedObject = spawnDoor();
                return ObjectToSpawn.door;
            case 2:
                lastSpawnedObject = spawnEnemy();
                return ObjectToSpawn.enemy;
            case 3:
                lastSpawnedObject = spawnChair();
                return ObjectToSpawn.chair;
            case 4:
                lastSpawnedObject = spawnBed();
                return ObjectToSpawn.bed;
        }
        return ObjectToSpawn.wall;
    }
    void spawnerManager()
    {
        if (lastSpawnedObject == null || lastSpawned == ObjectToSpawn.nothing || lastSpawned == ObjectToSpawn.wall || lastSpawned == ObjectToSpawn.enemy || lastSpawned == ObjectToSpawn.mainDoors || lastSpawned == ObjectToSpawn.bed)
        {
            spawneePicker();
            return;
        }
        else if(lastSpawned == ObjectToSpawn.chair && !doorsSpawned)
        {
            lastSpawnedObject = spawnDoor();
        }
        else if(lastSpawned == ObjectToSpawn.door)
        {
            int t = Random.Range(1, 100);
            doorsSpawned = false;
            if (t < 40)
               lastSpawnedObject = spawnChair();
            else lastSpawned = spawneePicker();

        }

    }

}
