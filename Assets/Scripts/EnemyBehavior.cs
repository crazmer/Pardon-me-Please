using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class EnemyBehavior : MonoBehaviour
{
    public float speed;
    [SerializeField]private float zBound = 20f;
    private Rigidbody enemyRb;
    private GameObject player;

    public int scoreToAdd = 1;
    private GameManager gameManager;

    private bool isprop = true;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.isGameOn)
        {
            EnemyControl();
        }
    }

    public void EnemyControl()
    {
        if (isprop == true)
        {
            if (!gameManager.isGameOver)
            {
                Vector3 lookDirection = (player.transform.position - transform.position).normalized;
                enemyRb.AddForce(lookDirection * speed);

                if (lookDirection != Vector3.zero)
                {

                    Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
                }

            }
        }

        if (transform.position.z <= -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }

        else if (transform.position.z >= zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }

    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Electric Bar"))
        {
            Destroy(gameObject);

            gameManager.UpdateScore(scoreToAdd);

        }

        if (collision.gameObject.CompareTag("Prop"))
        {
            
          //  isprop = false;
            
           
        }
    }


}