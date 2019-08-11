using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class LeftHand_BallManager : MonoBehaviour
{

    public static LeftHand_BallManager Instance;

    public SteamVR_TrackedObject trackedObj;

    public GameObject ballPrefab;

    public GameObject planePrefab;

    public GameObject spikeBallPrefab;

    public GameObject woodenBallPrefab;

    public GameObject bombBallPrefab;


    private GameObject[] ballTypes = new GameObject[BALL_TYPES_NUMBER];


    private const int BALL_TYPES_NUMBER = 3;

    private const int LIMIT = 500;

    private const float SPEED_MULTIPLIER = 75f;  

    private const float MIN_SPEED = 4.3f;

    private const float MAX_AVERAGE_VELOCITY = 3f;


    private static int counter = 0;

    private static int SPEED_COUNTER = 0;

    private static bool throwing = false;

    private static Vector3 pointA = Vector3.positiveInfinity;

    private static Vector3[] points = new Vector3[LIMIT];

    private static float[] speeds = new float[LIMIT];

    private static GameObject[] trail = new GameObject[LIMIT];

    private static Ball ball = new Ball();

    private static bool ball_exists = false;

    private static int frame_count = 0;

    private static bool wind_on = false;

    private static bool right_wind = false;

  //  private static float the_time;


    public class Ball
    {
        int damage;
        float speed;
        Vector3 scale;
        GameObject ball_object;

        public int Damage { get => damage; set => damage = value; }
        public float Speed { get => speed; set => speed = value; }
        public Vector3 Scale { get => scale; set => scale = value; }
        public GameObject Ball_Object { get => ball_object; set => ball_object = value; }
    }

    class SpikeBall : Ball
    {
        public SpikeBall(GameObject spikeBall)
        {
            Damage = 150;
            Speed = 3f;
            Scale = new Vector3(0.2f, 0.2f, 0.2f);
            Ball_Object = Instantiate(spikeBall);

            Ball_Object.transform.localScale = Scale;
            Ball_Object.transform.localRotation = Quaternion.identity;
        }

    }

    class WoodenBall : Ball
    {
        public WoodenBall(GameObject spikeBall)
        {
            Damage = 100;
            Speed = 3f;
            Scale = new Vector3(0.2f, 0.2f, 0.2f);
            Ball_Object = Instantiate(spikeBall);

            Ball_Object.transform.localScale = Scale;
            Ball_Object.transform.localRotation = Quaternion.identity;
        }

    }

    class BombBall : Ball
    {
        public BombBall(GameObject spikeBall)
        {
            Damage = 200;
            Speed = 3f;
            Scale = new Vector3(0.2f, 0.2f, 0.2f);
            Ball_Object = Instantiate(spikeBall);

            Ball_Object.transform.localScale = Scale;
            Ball_Object.transform.localRotation = Quaternion.identity;
        }

    }


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
        if (!ball.Ball_Object.GetComponent<Rigidbody>())
        {
            Fire();
        }
        else
        {
            if((Mathf.Floor(Time.time) % 25) == 0)
            {
                wind_on = !wind_on;
                if (wind_on)
                {
                    right_wind = !right_wind;
                }
            }
            if (wind_on)
            {
                MoveBall();
            }
        }                                                                                                                                                                                                                                                                                                                  
        // use the points array to display the points 
        if (ball.Ball_Object.transform.position.y < 0.08)
        {
            ball_exists = false;
            Destroy(ball.Ball_Object);
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

                if (ball.Ball_Object.GetComponent<Rigidbody>() == null)
                {
                    frame_count = 0;

                    ball.Ball_Object.AddComponent<Rigidbody>();

                    averageVelocity = (averageVelocity > 3) ? MAX_AVERAGE_VELOCITY : averageVelocity;
                    ball.Ball_Object.GetComponent<Rigidbody>().velocity = throwingDirection * averageVelocity * SPEED_MULTIPLIER; // maybe use SPEED_MULTIPLIER
                    ball.Ball_Object.GetComponent<Rigidbody>().useGravity = true;

                    ball.Ball_Object.GetComponent<Rigidbody>().AddTorque(Vector3.forward);

                    ball.Ball_Object.AddComponent<TrailRenderer>();
                    ball.Ball_Object.GetComponent<TrailRenderer>().startWidth = 0.20f;
                    ball.Ball_Object.GetComponent<TrailRenderer>().endWidth = 0.05f;
                    ball.Ball_Object.GetComponent<TrailRenderer>().time = 0.5f;
                    ball.Ball_Object.GetComponent<TrailRenderer>().material.color = new Color(1.8f,0,0);
                    ball.Ball_Object.GetComponent<TrailRenderer>().enabled = true;

                    pointA = Vector3.positiveInfinity;
                }

                ball.Ball_Object.transform.parent = null;
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
        if (!ball.Ball_Object || !ball_exists)
        {
            int rand = Random.Range(0, BALL_TYPES_NUMBER);
            switch (rand)
            {
                case 0:
                    ball = new SpikeBall(ballTypes[0]);
                    break;
                case 1:
                    ball = new WoodenBall(ballTypes[1]);
                    break;
                case 2:
                    ball = new BombBall(ballTypes[2]);
                    break;
                default:
                    ball = new WoodenBall(ballTypes[0]);
                    break;
            }

            ball.Ball_Object.transform.parent = trackedObj.transform;
            ball.Ball_Object.transform.position = trackedObj.transform.position;

            ball_exists = true;
        }
    }


    private void MoveBall()
    {
        frame_count++;
        if (right_wind)
        {
            ball.Ball_Object.transform.position = ball.Ball_Object.transform.position + new Vector3(0.001f, 0f, 0f) * frame_count;
        }
        else
        {
            ball.Ball_Object.transform.position = ball.Ball_Object.transform.position + new Vector3(-0.001f, 0f, 0f) * frame_count;
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
