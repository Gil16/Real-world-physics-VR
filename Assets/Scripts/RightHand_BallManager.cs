/*using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class RightHand_BallManager : MonoBehaviour
{

    public static RightHand_BallManager Instance;

    public SteamVR_TrackedObject trackedObj;

    public GameObject ballPrefab;

    public GameObject planePrefab;

    public GameObject spikeBallPrefab;

    public GameObject woodenBallPrefab;

    public GameObject bombBallPrefab;


    private GameObject[] ballTypes = new GameObject[BALL_TYPES_NUMBER];

    private GameObject currentBall;


    private const int BALL_TYPES_NUMBER = 3;

    private const int LIMIT = 500;

    private const float SPEED_MULTIPLIER = 50f;

    private const float MIN_SPEED = 2.3f;

    private const float MAX_AVERAGE_VELOCITY = 3f;


    private static int counter = 0;

    private static int SPEED_COUNTER = 0;

    private static bool throwing = false;

    private static Vector3 pointA = Vector3.positiveInfinity;

    private static Vector3[] points = new Vector3[LIMIT];

    private static float[] speeds = new float[LIMIT];

    private static GameObject[] trail = new GameObject[LIMIT];

    // Use this for initialization
    void Start()
    {
        ballTypes[0] = spikeBallPrefab;
        ballTypes[1] = woodenBallPrefab;
        ballTypes[2] = bombBallPrefab;
    }

    // Update is called once per frame
    void Update()
    {

        attachBall();
        if (!currentBall.GetComponent<Rigidbody>())
        {
            Fire();
        }
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

        if (!throwing)
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
            pointA = pointB;
        }
        else if (throwing && counter > 7)
        {
            pointB = trackedObj.transform.position;
            points[counter] = pointB;
            currentVelocity = Vector3.Distance(pointB, points[counter - 1]) / Time.deltaTime;
            speeds[SPEED_COUNTER] = currentVelocity;

            float averageVelocity = 0;
            for (int i = 0; i < SPEED_COUNTER; i++)
            {
                averageVelocity += speeds[i];
            }
            averageVelocity = averageVelocity / SPEED_COUNTER;

            if (currentVelocity < averageVelocity && speeds[SPEED_COUNTER - 1] < averageVelocity)
            {
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
                pointA = pointB;

                if (currentBall.GetComponent<Rigidbody>() == null)
                {
                    currentBall.AddComponent<Rigidbody>();
                    currentBall.GetComponent<Rigidbody>().AddTorque(Vector3.forward);
                    //        currentBall.GetComponent<Rigidbody>().mass = 20f;
                    averageVelocity = (averageVelocity > 3) ? MAX_AVERAGE_VELOCITY : averageVelocity;
                    currentBall.GetComponent<Rigidbody>().velocity = throwingDirection * averageVelocity * SPEED_MULTIPLIER; // maybe use SPEED_MULTIPLIER
                                                                                                                             //    Debug.Log(averageVelocity);
                    currentBall.GetComponent<Rigidbody>().useGravity = true;

                    currentBall.AddComponent<TrailRenderer>();
                    currentBall.GetComponent<TrailRenderer>().enabled = true;
                    currentBall.GetComponent<TrailRenderer>().startWidth = 0.50f;
                    currentBall.GetComponent<TrailRenderer>().endWidth = 0.25f;
                    currentBall.GetComponent<TrailRenderer>().time = 0.5f;

                    pointA = Vector3.positiveInfinity;
                }

                currentBall.transform.parent = null;
                throwing = false;
                counter = 0;
                SPEED_COUNTER = 0;
                averageVelocity = 0;

            }
            else
            {
                counter++;
                SPEED_COUNTER++;
            }

        }
        else
        {
            pointB = trackedObj.transform.position;
            points[counter] = pointB;
            currentVelocity = Vector3.Distance(pointB, points[counter - 1]) / Time.deltaTime;  // The distance between the last two points in the array
            speeds[SPEED_COUNTER++] = currentVelocity;
            counter++;
            pointA = pointB;
        }

    }


    public void attachBall()
    {
        if (currentBall == null)
        {
            int rand = Random.Range(0, BALL_TYPES_NUMBER);
            currentBall = Instantiate(ballTypes[rand]);
            currentBall.transform.parent = trackedObj.transform;
            currentBall.transform.position = trackedObj.transform.position;
            currentBall.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
*/