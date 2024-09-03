using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Set the player object as a camera target.
    public Transform target;
    // The distance between the camera and the target when following the target.
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed;
    public float maxWatchingAngle;
    public float minWatchingAngle;
    public Transform pivot;
    public bool invertY;
    public GameObject spawnedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (!useOffsetValues)
        {
            // The distance between the camera and the target when following the target.
            offset = target.position - transform.position;
        }
        // The Pivot will follow the target (player object), when the game is on.
        pivot.transform.position = target.transform.position;
        //pivot.transform.parent = target.transform;
        pivot.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // If there is no player, the program will look for a player.
        // After that one follows the player if the player is found.
        if (spawnedPlayer = null)
        {
            spawnedPlayer = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            //target = pivot.transform;
            transform.position = pivot.position + offset;
        }


        if (Input.GetMouseButtonDown(0))
        {
            // The Pivot-point will move with the target.
            pivot.transform.position = target.transform.position;

            // Get the Y-location of mouse and turn the target.
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            pivot.Rotate(0, horizontal, 0);

            // Get the X-location of mouse and turn the target.
            //float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

            /*if (invertY)
            {
                pivot.Rotate(vertical, 0, 0);
            }
            else
            {
                pivot.Rotate(-vertical, 0, 0);
            }*/

            // A smooth camera turning when looking the target from the ground and above the target.
            if (pivot.rotation.eulerAngles.x > maxWatchingAngle && pivot.rotation.eulerAngles.x < 180f)
            {
                pivot.rotation = Quaternion.Euler(maxWatchingAngle, 0, 0);
            }

            if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minWatchingAngle)
            {
                pivot.rotation = Quaternion.Euler(360f + minWatchingAngle, 0, 0);
            }
        }
        // Move the camera based on the current rotation of the target and the original offset.
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);

        // The distance between the camera and the target when following the target.
        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(target.position.x, target.position.y - .5f, transform.position.z);
        }
        // Set the player object as a camera target.
        transform.LookAt(target.transform);

    }
}
