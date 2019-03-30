using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BallManager : MonoBehaviour {

    public static BallManager Instance;

    public SteamVR_TrackedObject trackedObj;

    private GameObject currentBall;

    public GameObject ballPrefab;

    public GameObject planePrefab;




    private static int counter = 0;

    private static bool throwing = false;

    private static Vector3 pointA = Vector3.positiveInfinity;

    private static float lastVelocity;

    private const int LIMIT = 5000;

    private const int SPEED_MULTIPLIER = 4750;

    private const float FPS = 0.012f;

    private const float MIN_SPEED = 1.8f;   //  ////////////////////////////////////////   NOT     FINISHED     !!!!!

    private static Vector3[] points = new Vector3[LIMIT];


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        attachBall();
        Fire();
        if (currentBall.transform.position.y < 1)
        {
            Destroy(currentBall);
        }
    }

    private void Fire()
    {
        Vector3 pointB;
        float currentVelocity;
        if (throwing == false)
        {
            // Initialazing coords
            if (pointA.Equals(Vector3.positiveInfinity))
            {
                pointA = trackedObj.transform.position;
                pointB = trackedObj.transform.position;
                lastVelocity = 0;
            } else {
                pointB = trackedObj.transform.position;
            }

            currentVelocity = Vector3.Distance(pointB, pointA) / FPS;
            // Starting throwing, adding the points to the array
            if (currentVelocity >= MIN_SPEED)
            {
                throwing = true;
                points[counter++] = pointA;
                points[counter++] = pointB;
            }

        } else {
            
            // Counter reached the points array limit, we copy 3 coords and continuing
            if (counter == LIMIT - 1)
            {
                points[0] = points[counter-2];
                points[1] = points[counter - 1];
                points[2] = points[counter];
                counter = 2;
            }
            pointB = trackedObj.transform.position;
            points[counter] = pointB;
            currentVelocity = Vector3.Distance(points[counter], points[counter-1]) / FPS;
            lastVelocity = Vector3.Distance(points[counter - 1], points[counter - 2]) / FPS;
            if (currentVelocity < (lastVelocity * 0.85))
            {

                Vector3 throwingDirection = points[counter/2] - points[counter];            // not finishedddddddddddddddddddddddddddd
              //  float throwingVelocity = currentVelocity * SPEED_MULTIPLIER;

                if (currentBall.GetComponent<Rigidbody>() == null)
                {
                    currentBall.AddComponent<Rigidbody>();
                    currentBall.GetComponent<Rigidbody>().AddRelativeForce(throwingDirection * SPEED_MULTIPLIER, ForceMode.Force);
                    currentBall.GetComponent<Rigidbody>().useGravity = true;
                }

                currentBall.transform.parent = null;
                throwing = false;
                counter = 0;

            } else {
                counter++;
            }
            
        }

        lastVelocity = currentVelocity;
        pointA = pointB;
    }


    public void attachBall()
    {
        if (currentBall == null)
        {
            currentBall = Instantiate(ballPrefab);
            currentBall.transform.parent = trackedObj.transform;
            currentBall.transform.position = trackedObj.transform.position;
            currentBall.transform.localRotation = Quaternion.identity;
        }
    }






    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Destroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

}
