using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class LeftHand_BallManager : MonoBehaviour
{

    public static LeftHand_BallManager Instance;

    public SteamVR_TrackedObject trackedObj;

    private GameObject currentBall;

    public GameObject ballPrefab;

    public GameObject planePrefab;


    private static int counter = 0;

    private static int SPEED_COUNTER = 0;

    private static bool throwing = false;

    private static Vector3 pointA = Vector3.positiveInfinity;

    private const int LIMIT = 5000;

    private const float SPEED_MULTIPLIER = 50f; // magic number

    // private const float FPS = 0.012f;

    private const float MIN_SPEED = 1.8f;

    private static Vector3[] points = new Vector3[LIMIT];

    private static float[] speeds = new float[LIMIT];


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

            // Starting throwing, adding the points to the array
            if (currentVelocity >= MIN_SPEED)
            {
                throwing = true;
                points[counter++] = pointA;
                points[counter++] = pointB;             
            }
        
        }
        else if (throwing && counter < 8)
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
            // Counter reached the points array limit, we copy 3 coords and continuing
   /*         if (counter == LIMIT - 1)
            {
                points[0] = points[counter - 2];
                points[1] = points[counter - 1];
                points[2] = points[counter];
                counter = 2;
                return;
            }
    */
            

            pointB = trackedObj.transform.position;
            points[counter] = pointB;
            currentVelocity = Vector3.Distance(points[counter], points[counter - 1]) / Time.deltaTime;
            speeds[SPEED_COUNTER] = currentVelocity;

            float averageVelocity = 0;
            for (int i = 0 ; i < SPEED_COUNTER ; i++)
            {
                averageVelocity += speeds[i];
            }
            averageVelocity = averageVelocity / SPEED_COUNTER;

            // Debug.Log("averageVelocity = " + averageVelocity);
            //  Debug.Log("currentVelocity = " + currentVelocity);
            //if (currentVelocity < (averageVelocity))  // (lastVelocity * 0.85)
            
                if (speeds[SPEED_COUNTER] < averageVelocity && speeds[SPEED_COUNTER-1] < averageVelocity && speeds[SPEED_COUNTER-2] < averageVelocity
                   && speeds[SPEED_COUNTER - 3] < averageVelocity && speeds[SPEED_COUNTER - 4] < averageVelocity && speeds[SPEED_COUNTER - 5] < averageVelocity)
                {
                Vector3 throwingDirection = Vector3.zero;
                float sum = 0f;
                for (int i = 0; i <= (counter / 2); i++)
                {
                    throwingDirection = throwingDirection + (points[i] * i);

                    throwingDirection = throwingDirection + (points[counter-i] * (i));
                }
 /*               for (int i = counter; i > (counter / 2); i--)
                {
                    throwingDirection = throwingDirection + (points[i] * (counter - i));
                }  */
                
                if (counter % 2 == 1)
                {
                    sum = ((((counter - 1) * ((float)(counter-1))) / 8) + (((float)(counter-1)) / 4)) + (counter/2) + 1; // odd
                }
                else
                {
                    sum = (((counter * ((float)counter)) / 8) + (((float)counter) / 4)) * 2; // even
                }
                throwingDirection = throwingDirection / sum;
               // throwingDirection = throwingDirection * (-1);

                if (currentBall.GetComponent<Rigidbody>() == null)
                {
                    currentBall.AddComponent<Rigidbody>();                    
                    currentBall.GetComponent<Rigidbody>().AddRelativeForce(SPEED_MULTIPLIER * throwingDirection * averageVelocity, ForceMode.Force); // maybe use SPEED_MULTIPLIER
                    currentBall.GetComponent<Rigidbody>().useGravity = true;

                }
                currentBall.transform.parent = null;
                throwing = false;

                              // Points in space of the throw
                              for (int i = 0; i < counter; i++)
                              {
                                  GameObject ballPerPoint;
                                  ballPerPoint = Instantiate(ballPrefab);
                                  ballPerPoint.transform.localScale = new Vector3(0.01F, 0.01F, 0.01F);
                                  //    ballPerPoint.transform.parent = trackedObj.transform;
                                  ballPerPoint.transform.position = points[i];
                                  ballPerPoint.transform.localRotation = Quaternion.identity;
                              }  

                Debug.Log(counter); //ADDED TO CODE
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
