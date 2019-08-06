using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour {

    protected class MovingObject
    {
        int hp;
        int score;
        float speed;
        Vector3 startingPosition;
        Vector3 scale;
        Quaternion rotation;
        GameObject currentObject;

        public int Hp
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        public Vector3 StartingPosition
        {
            get
            {
                return startingPosition;
            }

            set
            {
                startingPosition = value;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }

        public GameObject CurrentObject
        {
            get
            {
                return currentObject;
            }

            set
            {
                currentObject = value;
            }
        }

        public virtual bool offBounds()
        {
            return false;
        }
    }

    class MovingFence : MovingObject
    {
        public MovingFence(GameObject fence)
        {
            Hp = 100;
            Score = 5;
            Speed = 17f;
            StartingPosition = new Vector3(254.665f, 1.02f, 170.023f);
            Scale = new Vector3(0.8f, 0.8f, 0.8f);
            Rotation = Quaternion.identity;
            CurrentObject = Instantiate(fence);

            CurrentObject.transform.position = StartingPosition;
            CurrentObject.transform.localScale = Scale;
            CurrentObject.transform.localRotation = Rotation;
        }

        public override bool offBounds()
        {
            return (CurrentObject.transform.position.z < 80f) ? true : false;
        }
    }


    public GameObject fencePrefab;

    public GameObject wallEPrefab;

    public GameObject elephantPrefab;


    private GameObject[] movingItems = new GameObject[NUMBER_OF_ITEMS];

    private const int NUMBER_OF_ITEMS = 3;

    private static bool object_exists = false;

    private static MovingObject moving = new MovingObject();

    // Use this for initialization
    void Start () {
        movingItems[0] = fencePrefab;
        movingItems[1] = wallEPrefab;
        movingItems[2] = elephantPrefab;
    }
	
	// Update is called once per frameW
	void Update () {
        if (!object_exists)
        {
            int rand = 0; // Random.Range(0,NUMBER_OF_ITEMS);
            switch (rand)
            {
                case 0:
                    moving = new MovingFence(movingItems[0]);
                    break;
                case 1:
                    // moving = new MovingFence();
                    break;
                case 2:
                    // moving = new MovingFence();
                    break;
                default:
                    //moving = new MovingFence();
                    break;
            }
            object_exists = true;

        }

        if (object_exists && moving.offBounds())
        {
            moving.CurrentObject.transform.parent = null;
            Destroy(moving.CurrentObject);
            object_exists = false;
        }
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
            object_exists = false;
        }
    }

}
