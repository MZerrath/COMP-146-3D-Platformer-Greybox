using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemRotation : MonoBehaviour {

    public float speed = 10;
    public int randomNumber;

    // Start is called before the first frame update
    void Start()
    {
        randomNumber = Random.Range(1, (int)speed);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(1,1,0) * randomNumber);
    }
}
