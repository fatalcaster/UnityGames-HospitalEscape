using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;
    public GameObject referenceObject=null;
    static private GameObject lastSpawnedWall=null;
    public float minimumDistanceFromWall=0.5f;
    Vector3 deafultWallScale=Vector3.zero;
    // pillar data
    public float pillarDifferenceMin = 0.2f;
    public float pillarDifferenceMaxDificulty=5f;
    private float pillarRandom;

    #region Start&Update
    void Start()
    {
        Destroy(lastSpawnedWall);
        deafultWallScale = referenceObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
            if (lastSpawnedWall != null)
            {
                float distance = Mathf.Abs(transform.position.x - lastSpawnedWall.transform.position.x);
                if (distance > pillarRandom)
                {
                    spawnWall();
                    pillarRandom = lastSpawnedWall.transform.localScale.x + Random.Range(pillarDifferenceMin, pillarDifferenceMaxDificulty);
                }
            }
            else spawnWall();

    }

    #endregion

    /// <summary>
    /// Checks the distance  from the last spawned wall
    /// </summary>
    /// <param name="pos">Last spawned wall position</param>
    /// <param name="scale">Last spawned wall scale</param>
    void checkDistances(Vector3 pos, Vector3 scale)//checks distances from walls to given object
    {
        if (pos.z + scale.z / 2f > 2.5f - minimumDistanceFromWall)
            pos.z += 2.5f - (pos.z + scale.z / 2f);
        if (pos.z - scale.z / 2f < 2.5f + minimumDistanceFromWall)
            pos.z -= 2.5f - (pos.z - scale.z / 2f);
    }
    /// <summary>
    /// Instantiate the wall
    /// </summary>
    void spawnWall()
    {
            if (objectToSpawn == null) return;
            if (referenceObject == null) return;
            if (objectToSpawn.tag == "spawnedWall")
            {
                Vector3 randomPos = transform.position;
                randomPos.z += Random.Range(-2, 2);
                Vector3 randomScale = deafultWallScale;
                randomScale.z +=Random.Range(0f,1.2f);
                randomScale.x += Random.Range(0f, 5f);
                checkDistances(randomPos, randomScale);
                objectToSpawn.transform.localScale = randomScale;
                lastSpawnedWall = Instantiate(objectToSpawn, randomPos, transform.rotation);
            }
    }
}
