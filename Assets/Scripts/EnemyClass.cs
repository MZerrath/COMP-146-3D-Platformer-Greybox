using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour {

    public enum State {
        LOOKFOR,
        GOTO,
        ATTACK,
    }
    public State curState;

    public float speed = .5f;
    public float goToDistance = 13;
    public float attackDistance = 4;
    public Transform target;
    public string playerTag = "Player";
    public float attackTimer = 2;
    // Note: I've added this damage modifier to the enemy class to allow for the player's ChangePlayerHealth() method to work
    [SerializeField] private int damage = -1;

    // Two extra variables have been added to ensure functionality
    public float curTime;
    Player playerScript;

    // Start is called before the first frame update
    IEnumerator Start() {
        curTime = attackTimer;
        target = GameObject.FindGameObjectWithTag(playerTag).transform;
        if (target != null) {
            playerScript = target.GetComponent<Player>();
        }
        while (true){
            // This is our update, gets called each frame
            switch (curState)
            {
                case State.LOOKFOR:
                    LookFor();
                    break;
                case State.GOTO:
                    GoTo();
                    break;
                case State.ATTACK:
                    Attack();
                    break;
            }
            yield return 0;
        }
    }

    void LookFor() {
        if (Vector3.Distance(target.position, transform.position) < goToDistance) {
            curState = State.GOTO;
        }
    }
    void GoTo() {
        transform.LookAt(target);
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit something; // used to signify what we are looking at
        if(Physics.Raycast(transform.position, fwd, out something)) {
            if(something.transform.tag != playerTag)
            {
                // not our Player need to keep looking
                curState = State.LOOKFOR;
                return; // no need to continue, exit the function
            }
        }

        if (Vector3.Distance(target.position, transform.position) > attackDistance) {
            // Set our position towards Player
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } else {
            // means we are close enough to attack
            curState = State.ATTACK;
        }
    }
    void Attack() {
        curTime = curTime - Time.deltaTime;
        if (curTime < 0) {
            // playerScript.health-- changed to playerScript.ChangePlayerHealth(damage);
            playerScript.ChangePlayerHealth(damage);
            curTime = attackTimer;
        }
        if (Vector3.Distance(target.position, transform.position) > attackDistance) {
            // set our position towards Player
            curState = State.GOTO;
        }
    }
      
}
