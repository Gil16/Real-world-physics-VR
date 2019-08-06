using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour {

  //  public GameObject objectPrefab;

    public GameObject fencePrefab;

    public GameObject wallEPrefab;

    public GameObject elephantPrefab;


    private GameObject currentObject;

    private GameObject[] movingItems = new GameObject[NUMBER_OF_ITEMS];

    private const int NUMBER_OF_ITEMS = 3;

	// Use this for initialization
	void Start () {
        movingItems[0] = fencePrefab;
        movingItems[1] = wallEPrefab;
        movingItems[2] = elephantPrefab;
    }
	
	// Update is called once per frameW
	void Update () {
        createObject();
        if (currentObject.transform.position.z < 80f)
        {
            currentObject.transform.parent = null;
            Destroy(currentObject);
        }
    }

    private void createObject()
    {
        if (currentObject == null)
        {
            int rand = 0; // Random.Range(0,NUMBER_OF_ITEMS);
            currentObject = Instantiate(movingItems[rand]);
            //currentObject.transform.parent = objectPrefab.transform;
            currentObject.transform.position = new Vector3(262.665f, 6.268f, 166.023f);
            currentObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            currentObject.transform.localRotation = Quaternion.identity;
        }
    }

}
