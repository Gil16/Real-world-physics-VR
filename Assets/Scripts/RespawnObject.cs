using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour {

    public GameObject objectPrefab;

    private GameObject currentObject;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        createObject();
        if (currentObject.transform.position.z < -30f)
        {
            currentObject.transform.parent = null;
            Destroy(currentObject);
        }
    }

    private void createObject()
    {
        if (currentObject == null)
        {
            currentObject = Instantiate(objectPrefab);
            currentObject.transform.parent = objectPrefab.transform;
            currentObject.transform.position = new Vector3(-0.05f, 2.98f, 30f);
            currentObject.transform.localRotation = Quaternion.identity;
        }
    }

}
