using System.Collections;

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour

{

    public Camera playerCamera;

    public Transform PlayerTrans;

    public float walkSpeed = 6f;

    public float runSpeed = 12f;

    public float jumpPower = 7f;

    public float gravity = 10f;

    public float lookSpeed = 2f;

    public float lookXLimit = 45f;

    public float defaultHeight = 2f;

    public float crouchHeight = 1f;

    public float crouchSpeed = 3f;

    public int ammo = 5;

    public GameObject Footstep;

    public GameObject Projectile;

    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;
    private bool invoked = false;
    

    public GameManager gameManager;

    //pickup variables
    public float pickupRange = 2f;
    public TextMeshProUGUI pickupText;

    //interaction distance
    public float interactionRange = 5f;
    void Start()

    {

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
        Footstep.SetActive(false);

        if(pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }

    }



    void Update()

    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);



        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;

        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        int MovementSpeed = (int)curSpeedX + (int)curSpeedY;

        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        checkForItems();

        //collection mechanic
        if (Input.GetKeyDown(KeyCode.G))
        {
            collectItem();
        }

        //interaction with ovens
        if(Input.GetKeyDown(KeyCode.F))
        {
            interactWithObjective();
        }



        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)

        {
            Debug.Log("boing");
            moveDirection.y = jumpPower;

        }


        else

        {

            moveDirection.y = movementDirectionY;

        }



        if (!characterController.isGrounded)

        {

            moveDirection.y -= gravity * Time.deltaTime;

        }



        if (Input.GetKey(KeyCode.R) && canMove)

        {

            characterController.height = crouchHeight;

            walkSpeed = crouchSpeed;

            runSpeed = crouchSpeed;



        }

        else

        {

            characterController.height = defaultHeight;

            walkSpeed = 6f;

            runSpeed = 12f;

        }

        if (Input.GetKeyUp("e"))
        {
            Debug.Log("e pressed");

            Shoot();
        }



        characterController.Move(moveDirection * Time.deltaTime);

        //Animation
        if(MovementSpeed == 0)
        {
            //idle
            animator.SetFloat("Speed", 0);
            StopFootsteps();
        }
        else
        {
            //walk/run
            animator.SetFloat("Speed", 1);
            Footsteps();
        }



        if (canMove)

        {

            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;


            // TO RE-ENABLE CAMERA ROTAION,
            //MAKE THE FIRST lookXLimit NEGATIVE V--(this one, like this -lookXlimit)
            rotationX = Mathf.Clamp(rotationX, lookXLimit, lookXLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        }

        void Footsteps()
        {
            Footstep.SetActive(true);
        }
        void StopFootsteps()
        {
            Footstep.SetActive(false);
        }

    }

    void checkForItems()
    {
      Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Collectible"))
            {
                // Display pickup message
                ShowPickupMessage();

                return; // Exit the loop after finding one collectible
            }
        }

        // No collectible nearby, hide the pickup message
        HidePickupMessage();
    }

    void ShowPickupMessage()
    {
        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(true);
        }
    }

    void HidePickupMessage()
    {
        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }
    }


    void collectItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f); // Radius to check for colliders

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Collectible"))
            {
                // Collect the item

                CollectCroissant(collider.gameObject);
            }
        }
    }

    void CollectCroissant(GameObject item)
    {
        // Inform the GameManager that an item is collected
        gameManager.ItemCollected();

        // Implement logic to collect the item (e.g., disable the item's renderer and collider)
        item.SetActive(false);

        

    }

    void interactWithObjective()
    {
        Debug.Log("interacting");
        float distanceToObjective = Vector3.Distance(transform.position, gameManager.objectiveArea.position);
        Debug.Log("distance: " + distanceToObjective);

        if (distanceToObjective <= interactionRange)
        {
            Debug.Log("player is close enough");
            gameManager.TryInteractWithObjective();
        }
        else{
            Debug.Log("not close");
        }
    }

    void Shoot()
    {

        if (ammo > 0)
        {
            // Instantiate a projectile matching the players position and rotation
            GameObject proj = Instantiate(Projectile, PlayerTrans.transform.position + (transform.forward * 2), Quaternion.identity);
            ammo--;
            Debug.Log(ammo + " remaining");

            // Apply forces to the projectile in the direction the player is facing 
            //DEBUG: proj.GetComponent<Rigidbody>().AddForce((PlayerTrans.forward) * 15f, ForceMode.Impulse);
            proj.GetComponent<Rigidbody>().AddForce((PlayerTrans.forward) * 15f, ForceMode.Impulse);

            // Destroy each projectile after 5 seconds
            Destroy(proj, 5f);
        }
 
        if (ammo == 0)
        {
            StartCoroutine(reload());
        }

    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(3);

        ammo = 5;

        Debug.Log(ammo);

        Shoot();
    }

}
