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
        else if (collision.gameObject.name == "Cube")
        {

            Destroy(gameObject);
        }
    }
}
