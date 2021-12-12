using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Declaring Rigidbody Component variable
    //private Rigidbody playerRb;

    // Private Variables to fix the speed and bounary
    private float speed = 20.0f;
    [SerializeField] private float zBound = 20f;
    public float rotationSpeed;

    public Animator animator;

    public GameObject powerupIndicator;

    public bool isTriggered;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Getting Rigidbody Component to the variable
        // playerRb = GetComponent<Rigidbody>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
       // animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.isGameOn)
        {
            // Calling the method for player control
            PlayerMovement();

            // Calling the method for restricting area
            PlayerConstrains();
        }
    }

    // Method Created to get control over Player
    public void PlayerMovement()
    {

        // Declaring and Assigning Input Manager
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Adding force with the keyCodes refered in Input Manager using rigidbody.Addforce()
 

        Vector3 movementDirections = new Vector3(horizontalInput, 0, verticalInput);
        movementDirections.Normalize();

        transform.Translate(movementDirections * speed * Time.deltaTime, Space.World);
        
        if (movementDirections != Vector3.zero)
        {
            //transform.forward = movementDirections;
            animator.SetBool("isWalking", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirections, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        powerupIndicator.transform.position = transform.position;


    }

    // Method Created to restrict the boundary
    public void PlayerConstrains()
    {
        // If position of player towards z Axis gets over the z positive point
        if (transform.position.z >= zBound)
        {
            // Setting transform.position as current position; setting z axis to positive bound 
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }

        // If position of player towards z Axis gets over the z negetive point
        if (transform.position.z <= -zBound)
        {
            // Setting transform.position as current position; setting z axis to negetive bound 
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isTriggered)
        {
            Destroy(gameObject);

            gameManager.isGameOver = true;
            Debug.Log("GAME OVER");
        }


        if (collision.gameObject.CompareTag("Electric Bar"))
        {
            Destroy(gameObject);

            powerupIndicator.SetActive(false);
            Debug.Log("GAME OVER");

            gameManager.isGameOver = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            isTriggered = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(Powerupactivity());
        }
    }

    IEnumerator Powerupactivity()
    {
        yield return new WaitForSeconds(5);
        isTriggered = false;
        powerupIndicator.SetActive(false);
    }

}