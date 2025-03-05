using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBuilder : MonoBehaviour
{
    public GameObject fencePrefab;
    public GameObject cornerPrefab;
    public int numberOfFenceSections = 10;
    public float fenceSectionLength = 2.0f;

    void Start()
    {
        BuildFence();
    }

    void BuildFence()
    {
        Vector3 position = transform.position;

        for (int i = 0; i < numberOfFenceSections; i++)
        {
            // Instantiate fence section
            Instantiate(fencePrefab, position, Quaternion.identity, transform);

            // Adjust position for the next section
            position += new Vector3(fenceSectionLength, 0, 0);

            // Add corner at specific intervals (e.g., every 4 sections)
            if ((i + 1) % 4 == 0)
            {
                Instantiate(cornerPrefab, position, Quaternion.Euler(0, 90, 0), transform);
                position += new Vector3(0, 0, fenceSectionLength);
            }
        }

    }
}
