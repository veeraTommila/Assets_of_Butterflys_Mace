using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaayerControls : MonoBehaviour
{
    public float moveSpeed;   //Tätä muuttujaa moveSpeed voidaan editoida Unityssä, private floatia ei voida.
    //public Rigidbody theRB;   //Otetaan Rigidbody käyttöön.
    public float jumpForce;
    public CharacterController controller;

    private Vector3 moveDirection;
    public float gravityScale;

    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;

    public GameObject playerModel;

    // Start is called before the first frame update
    void Start()
    {
        //theRB = GetComponent<Rigidbody>();  //Ei tarvitse raahata Unityn play-moodia varten.
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         Move around. Set speed of axes X, Y and Z.
        //X = Horizontal (vasemmalle ja oikealle), Z = Vertical (eteen ja taakse) sekä Y = ylös ja alas.
        theRB.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, theRB.velocity.y, Input.GetAxis("Vertical") * moveSpeed);

        //Onko Unityssä hypylle tarkoitettua näppäintä painettu.
        if (Input.GetButtonDown("Jump"))
        {
            //Muutetaan Y-akselin arvoa.
            theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
        }*/


        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);

        //Pelinappulan liikkeen kontrollointi on helpompaa.
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;


        //Onko Unityssä hypylle tarkoitettua näppäintä painettu.
        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                //Muutetaan Y-akselin arvoa.
                moveDirection.y = jumpForce;
            }
        }


        moveDirection.y = moveDirection.y + (Physics.gravity.y * Time.deltaTime * gravityScale);

        //Sovelletaan moveDirectionia Character Controlleriin.
        controller.Move(moveDirection * Time.deltaTime);

        //Pelinappula kääntyy kameran katsoman suunnan mukaan.
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }
}
