using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public float gravity = 10;
    public float maxVelocityChange = 10;
    private bool dead;
    public int health = 3;
    public float jumpHeight = 2;
    private bool grounded;
    public int points = 0;
    private Transform playerTransform;
    private Rigidbody _rigidbody;

    // LevelManager added in so that the player doesn't do anything involving Scene Management
    [SerializeField] LevelManager levelManager;

    public Text pointsText;
    public Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
        RigidbodyConstraints.FreezeRotationY |
        RigidbodyConstraints.FreezeRotationZ;
        SetPointsText();
        SetHealthText();
    }

    void FixedUpdate()
    {
        playerTransform.Rotate(0,Input.GetAxis("Horizontal"), 0);
        Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Vertical"));
        targetVelocity = playerTransform.TransformDirection(targetVelocity);
        targetVelocity = targetVelocity * 10;

        Vector3 velocity = _rigidbody.velocity;
        Vector3 velocityChange = targetVelocity - velocity;
        // Clamping-grabs the current value and keeps it within min/max values
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0; // We don't move up/down
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        //print(_rigidbody.velocity);

        // Note: I deviated from the instructions by adding the grounded variable as part of the if statement.
        // If I did not, the player would be able to jump infinitely. Doing this will allow
        // for only one jump whenever the player presses the Jump button.
        if(Input.GetButton("Jump") && grounded)
        {
            _rigidbody.velocity = new Vector3(velocity.x, CalculateJump(), velocity.z);
            // Grounded turns to false when the player is in the air.
            grounded = false;
        }
    }

     void Update()
    {
        // If the player's health is below 1, the player dies.
        if (health < 1)
        {
            Destroy(this); // Disables player control until player restarts the level.
            levelManager.ShowGameOver();
        }
    }

    void OnCollisionStay() // if there is an object underneath. such as when the player is on the ground.
    {
        grounded = true;
    }

    float CalculateJump()
    {
        float jump = Mathf.Sqrt(2 * jumpHeight * gravity);
        return jump;
    }

    // Another change from the instructions: because the algorithm was being repeated, I have refactored 
    // the code for collectables into its own generalized method: void CollectCollectable().
    // The method is called whenever the player touches a coin, gem, or finish pole.
    void OnTriggerEnter(Collider theCoin)
    {
        if (theCoin.tag == "Coin")
        {
            CollectCollectable(5, theCoin.gameObject);
        }
        else if (theCoin.tag == "Gem")
        {
            CollectCollectable(20, theCoin.gameObject);
        }
        else if (theCoin.tag == "Finish")
        {
            CollectCollectable(50, theCoin.gameObject);
            levelManager.ShowLevelComplete();
        }
    }

    // This method is called when the player collects a coin, collects a gem, or hits the finish pole.
    void CollectCollectable(int prize, GameObject theCoin)
    {
        points = points + prize;
            // Collectable has been colleced, so destroy the collectable and update the score counter
            Destroy(theCoin.gameObject);
            SetPointsText();
    }

    // This method is called when the player gets hit by an enemy or when the player picks up a health power-up.
    // Health power-up not implemented at this time.
    public void ChangePlayerHealth(int change)
    {
        // If change is negative, it's damage. Otherwise, it's healing.
        health += change;
        SetHealthText();
    }

    // Called in Start() and CollectCollectable().
    public void SetPointsText()
    {
        pointsText.text = "Points: " + points.ToString();
    }

    // Called in Start() and ChangePlayerHealth().
    public void SetHealthText()
    {
        healthText.text = "Health: " + health.ToString();
    }
}
