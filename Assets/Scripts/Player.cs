using System.Collections;

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


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

    public GameObject Throw;

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

    public TextMeshProUGUI reloadText;

    public TextMeshProUGUI ammoCountText;
    

    //interaction distance
    public float interactionRange = 5f;
    void Start()

    {
        //helps to lock and unlock cursor so user can press buttons on win scene
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "WinScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;
        }

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Footstep.SetActive(false);
        Throw.SetActive(false);

        if(pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }

        if (reloadText != null)
        {
            reloadText.gameObject.SetActive(false);
        }

        UpdateAmmoUI();

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
            
            Vector3 throwDirection = PlayerTrans.forward * 200f;

            throwDirection.y = 9f;

            proj.GetComponent<Rigidbody>().AddForce(throwDirection, ForceMode.Impulse);

            Chuck();
            //StopChuck();


            if (ammo <=0 )
            {
                ShowReloadPrompt();
                reload();
            }

            // Destroy each projectile after 5 seconds
            Destroy(proj, 5f);

            UpdateAmmoUI();
        }
    }

    void Chuck()
    {
        Throw.SetActive(true);
        Invoke("StopChuck", .1f);
    }

    void StopChuck()
    {
        Throw.SetActive(false);
    }

    //display reload text
    void ShowReloadPrompt()
    {
        if(reloadText!= null)
        {
            reloadText.gameObject.SetActive(true);
        }
       
    }

    //hide reload prompt
    void HideReloadText()
    {
        if(reloadText != null)
        {
            reloadText.gameObject.SetActive(false);
        }
    }


    void reload()
    {

        // yield return new WaitForSeconds(3);
        Invoke("completeReload", 3f);
    
    }

    void completeReload()
    {
        ammo = 5;

        Debug.Log("reload");
        HideReloadText();
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        //update ammo count
        if(ammoCountText != null)
        {
            ammoCountText.text = ammo.ToString();
        }
    }

}
