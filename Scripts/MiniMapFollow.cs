using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    /* A variable to the reference of the player's transform component, which contains information about the 
     * rotation and position of the player.
     */
    public Transform player;    

    /* This method is called once per frame, but after all Update methods have been called. It’s ideal for camera
     * adjustments to ensure all movement and rotations are completed first.
     */
    void LateUpdate()
    {
        Vector3 newPosition = player.position;  // To create a new Vector3 variable and set it to the current position of the player.
        newPosition.y = transform.position.y;   // To keep the minimap camera's height constant by setting the y coordinate to its current value.
        transform.position = newPosition;   // To update the position of the minimap camera to a new position.

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);   // To set the minimap camera's rotation.
    }
}
