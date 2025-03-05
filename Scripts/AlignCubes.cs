using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignCubes : MonoBehaviour
{
    public GameObject cubePrefab; // Assign your cube prefab in the Inspector
    public int numberOfCubes = 10; // Number of cubes to align
    public float spacing = 1.0f; // Spacing between cubes

    void Start()
    {
        AlignCubesInWorldCenter();
    }

    void AlignCubesInWorldCenter()
    {
        Vector3 worldCenter = new Vector3(0, 0, 0); // Center of the world axes

        for (int i = 0; i < numberOfCubes; i++)
        {
            Vector3 position = worldCenter + new Vector3(i * spacing, 0, 0); // Adjust the position along the X axis
            Instantiate(cubePrefab, position, Quaternion.identity);

            position = worldCenter + new Vector3(0, i * spacing, 0); // Adjust the position along the Y axis
            Instantiate(cubePrefab, position, Quaternion.identity);

            position = worldCenter + new Vector3(0, 0, i * spacing); // Adjust the position along the Z axis
            Instantiate(cubePrefab, position, Quaternion.identity);
        }
    }
}