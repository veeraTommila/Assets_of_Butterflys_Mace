using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCode : MonoBehaviour
{
    //private GameObject Player;
    public GameObject Enemy;

    public float threshold;

    public Collider area;

    public float range;

    /*public GameObject SpawnPlayer()
    {
        return Spawn(Player);
    }*/

    public GameObject SpawnEnemy()
    {
        return Spawn(Enemy);
    }

    private GameObject Spawn(GameObject obj)
    {
        Vector3 minPoint = area.bounds.min;
        Vector3 maxPoint = area.bounds.max;

        float randomX = Random.Range(minPoint.x, maxPoint.x);
        float randomZ = Random.Range(minPoint.z, maxPoint.z);

        Vector3 pos = new Vector3(randomX, maxPoint.y, randomZ);
        Collider[] colliders = Physics.OverlapSphere(pos, range);
        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Obstacle"))
            {
                Debug.Log("Nothing.");
            }
        }

        // To create an object.
        GameObject newObj = Instantiate(obj, pos, new Quaternion());
        return newObj;
    }
    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            transform.position = new Vector3(0f,0f,0f);
        }
    }
}
