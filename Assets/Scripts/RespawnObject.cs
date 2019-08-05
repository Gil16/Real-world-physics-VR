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
        //    currentObject.transform.parent = objectPrefab.transform;
            currentObject.transform.position = new Vector3(262.665f, 6.268f, 166.023f);
            currentObject.transform.localScale = new Vector3(3f, 3f, 1f);
            currentObject.transform.localRotation = Quaternion.identity;
        }
    }

}
