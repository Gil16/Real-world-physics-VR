/*using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class RightHand_BallManager : MonoBehaviour
{

    public static RightHand_BallManager Instance;

    public SteamVR_TrackedObject trackedObj;

    private GameObject currentBall;

    public GameObject ballPrefab;

    public GameObject planePrefab;

    private static bool Ball = true;

    private static int counter = 0;

    private static int SPEED_COUNTER = 0;

    private static int trailCounter = 0;

    private static bool throwing = false;

    private static bool trailBool = false;

    private static Vector3 pointA = Vector3.positiveInfinity;

    private const int LIMIT = 500;

    private const float SPEED_MULTIPLIER = 50f;  // magic number

    // private const float FPS = 0.012f;

    private const float MIN_SPEED = 2f;

    private const float MAX_AVERAGE_VELOCITY = 3f;

    private static Vector3[] points = new Vector3[LIMIT];

    private static float[] speeds = new float[LIMIT];

    private static GameObject[] trail = new GameObject[LIMIT];




    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        attachBall();
        Fire();
        // use the points array to display the points 
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
                return;
            }
            else
            {
                pointB = trackedObj.transform.position;
            }
            currentVelocity = Vector3.Distance(pointB, pointA) / Time.deltaTime;

            // Start throwing, add the points to the array
            if (currentVelocity >= MIN_SPEED)
            {
                throwing = true;
                points[counter++] = pointA;
                points[counter++] = pointB;
            }

        }
        else if (throwing && counter < 15)
        {
            pointB = trackedObj.transform.position;
            points[counter] = pointB;
            currentVelocity = Vector3.Distance(points[counter], points[counter - 1]) / Time.deltaTime;
            speeds[SPEED_COUNTER] = currentVelocity;
            counter++;
            SPEED_COUNTER++;
        }
        else
        {

            pointB = trackedObj.transform.position;
            points[counter] = pointB;
            currentVelocity = Vector3.Distance(points[counter], points[counter - 1]) / Time.deltaTime;
            speeds[SPEED_COUNTER] = currentVelocity;

            float averageVelocity = 0;
            for (int i = 0; i < SPEED_COUNTER; i++)
            {
                averageVelocity += speeds[i];
            }
            averageVelocity = averageVelocity / SPEED_COUNTER;

            if (speeds[SPEED_COUNTER] < averageVelocity && speeds[SPEED_COUNTER - 1] < averageVelocity)
            {
                removeTrailDots();

                Vector3 throwingDirection = Vector3.zero;
                for (int i = 1; i <= (counter / 2); i++)
                {
                    throwingDirection += ((points[i] - points[i - 1]) * i);
                    throwingDirection += ((points[counter - i] - points[counter - i - 1]) * (i));
                }

                float sum = 0f;
                if (counter % 2 == 1)
                {
                    sum = ((((counter - 1) * ((float)(counter - 1))) / 8) + (((float)(counter - 1)) / 4)) + (counter / 2) + 1; // odd
                }
                else
                {
                    sum = (((counter * ((float)counter)) / 8) + (((float)counter) / 4)) * 2; // even
                }
                throwingDirection = throwingDirection / sum;

                if (currentBall.GetComponent<Rigidbody>() == null)
                {
                    currentBall.AddComponent<Rigidbody>();
                    //      currentBall.GetComponent<Rigidbody>().AddTorque(Vector3.forward);
                    //        currentBall.GetComponent<Rigidbody>().mass = 20f;
                    averageVelocity = (averageVelocity > 3) ? MAX_AVERAGE_VELOCITY : averageVelocity;
                    currentBall.GetComponent<Rigidbody>().velocity = throwingDirection * averageVelocity * SPEED_MULTIPLIER; // maybe use SPEED_MULTIPLIER
                                                                                                                             //    Debug.Log(averageVelocity);
                    currentBall.GetComponent<Rigidbody>().useGravity = true;

                    trailCounter = counter;
                    // Points in space of the throw
                    for (int i = 0; i < trailCounter; i++)
                    {
                        trail[i] = Instantiate(ballPrefab);
                        trail[i].transform.localScale = new Vector3(0.01F, 0.01F, 0.01F);
                        trail[i].transform.position = points[i];
                        trail[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        trail[i].transform.localRotation = Quaternion.identity;
                    }
                    trailBool = true;
                    Ball = false;
                }

                currentBall.transform.parent = null;
                throwing = false;
                counter = 0;
                SPEED_COUNTER = 0;
            }
            else
            {
                counter++;
                SPEED_COUNTER++;
            }

        }
        pointA = pointB;
    }


    public void attachBall()
    {
        if (currentBall == null)
        {
            currentBall = Instantiate(ballPrefab);
            currentBall.transform.parent = trackedObj.transform;
            currentBall.transform.position = trackedObj.transform.position;
            //    currentBall.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            currentBall.transform.localRotation = Quaternion.identity;
            Ball = true;
        }
    }


    private void removeTrailDots()
    {
        if (!trailBool || !Ball)
            return;
        else
        {
            for (int i = 0; i <= trailCounter; i++)
            {
                Destroy(trail[i]);
            }
            trailCounter = 0;
            trailBool = false;
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
*/