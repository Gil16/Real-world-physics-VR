using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroyOnPlane : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "SpikeBall(Clone)" 
            || collision.gameObject.name == "WoodenBall(Clone)" 
            || collision.gameObject.name == "bombBall(Clone)"
            || collision.gameObject.name == "Cube(Clone)"
            || collision.gameObject.name == "Fence(Clone)"
            || collision.gameObject.name == "WallE(Clone)"
            || collision.gameObject.name == "Elephant(Clone)")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}