using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class RespawnObject : MonoBehaviour {

    public GameObject fencePrefab;

    public GameObject wallEPrefab;

    public GameObject elephantPrefab;

    public GameObject explosionPrefab;


    public static bool start_game = false;

    public static bool game_over = false;

    public static float startTime;


    private static GameObject text;

    private static GameObject startGameButton;

    private static GameObject score_board;

    private static GameObject score_text;

    private static GameObject time_text;

    private static GameObject timerObj;

    private static GameObject text_gameover;

    private GameObject[] movingItems = new GameObject[NUMBER_OF_ITEMS];


    private const int NUMBER_OF_ITEMS = 3;

    private const int BOMB_BALL_DAMAGE = 200;

    private const int WOODEN_BALL_DAMAGE = 100;

    private const int SPIKE_BALL_DAMAGE = 150;


    private static bool init_flag = false;

    private static bool score_board_flag = false;

    private static bool object_exists = false;

    private static MovingObject moving = new MovingObject();

    private static int current_score = 0;



    public class MovingObject
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

        public bool offBounds()
        {
            return (CurrentObject.transform.position.z < 80f) ? true : false;
        }
    }

    class MovingFence : MovingObject
    {
        public MovingFence(GameObject fence)
        {
            Hp = 100;
            Score = 5;
            Speed = 5f;
            StartingPosition = new Vector3(254.665f, 1.02f, 170.023f);
            Scale = new Vector3(0.8f, 0.8f, 0.8f);
            Rotation = Quaternion.identity;
            CurrentObject = Instantiate(fence);

            CurrentObject.transform.position = StartingPosition;
            CurrentObject.transform.localScale = Scale;
            CurrentObject.transform.localRotation = Rotation;
        }

    }

    class MovingWallE : MovingObject
    {
        public MovingWallE(GameObject wallE) {
            Hp = 180;
            Score = 10;
            Speed = 3f;
            StartingPosition = new Vector3(256.665f, 1.02f, 170.023f);
            Scale = new Vector3(0.2f, 0.2f, 0.2f);
            Rotation = Quaternion.Euler(0f,90f,0f);
            CurrentObject = Instantiate(wallE);

            CurrentObject.transform.position = StartingPosition;
            CurrentObject.transform.localScale = Scale;
            CurrentObject.transform.localRotation = Rotation;
        }

    }

    class MovingElephant : MovingObject
    {
        public MovingElephant(GameObject elephant)
        {
            Hp = 150;
            Score = 6;
            Speed = 4f;
            StartingPosition = new Vector3(260f, 0.6f, 170.023f);
            Scale = new Vector3(100f, 100f, 100f);
            Rotation = Quaternion.Euler(0f, 180f, 0f);
            CurrentObject = Instantiate(elephant);

            CurrentObject.transform.position = StartingPosition;
            CurrentObject.transform.localScale = Scale;
            CurrentObject.transform.localRotation = Rotation;
        }

    }


    // Use this for initialization
    void Start () {
        if (!init_flag)
        {
            init_flag = true;
            initGame();   
        } 
    }    
	
	// Update is called once per frameW
	void Update () {
        if (start_game && !game_over)
        {
            updateTime();
            if (!object_exists)
            {
                int rand = UnityEngine.Random.Range(0, NUMBER_OF_ITEMS);
                switch (rand)
                {
                    case 0:
                        moving = new MovingFence(movingItems[0]);
                        break;
                    case 1:
                        moving = new MovingWallE(movingItems[1]);
                        break;
                    case 2:
                        moving = new MovingElephant(movingItems[2]);
                        break;
                    default:
                        moving = new MovingFence(movingItems[0]);
                        break;
                }
                object_exists = true;
            }

            moving.CurrentObject.transform.Translate(Vector3.back * Time.deltaTime * moving.Speed, Space.World);

            if (object_exists && moving.offBounds())
            {
                moving.CurrentObject.transform.parent = null;
                Destroy(moving.CurrentObject);
                object_exists = false;
            }
        }

        if (object_exists) {            
            if (moving.CurrentObject.transform.position.z <= LeftHand_BallManager.points[0].z) {         //// need right hand too ////////////////
                gameOver();
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "StartGame")
        {
            Destroy(collision.gameObject);
            text.GetComponent<TextMesh>().text = "";
            Destroy(text);

            if (game_over)
            {
                text_gameover.GetComponent<TextMesh>().text = "";
                Destroy(text_gameover);
                game_over = false;
            }

            start_game = true;
            startTimer();
            startScore();      
            return;
        }
        if(!start_game)
        {
            return;
        } 
    
        if (!gameObject.GetComponent<Rigidbody>())
        {
            gameOver();
            return;
        } 
        if (collision.gameObject.name == "Plane")
        {
            Destroy(gameObject);
        }
        else if (gameObject.name == "BombBall(Clone)")
        {
            GameObject exp = Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
            moving.Hp = moving.Hp - BOMB_BALL_DAMAGE;
            Destroy(exp, 3);
        }
        else if (gameObject.name == "WoodenBall(Clone)")
        {
            moving.Hp = moving.Hp - WOODEN_BALL_DAMAGE;
        }
        else if (gameObject.name == "SpikeBall(Clone)")
        {
            moving.Hp = moving.Hp - SPIKE_BALL_DAMAGE;
        }
        Destroy(gameObject);
        if (moving.Hp <= 0)
        {
            Destroy(collision.gameObject);
            current_score = current_score + moving.Score;
            score_board.GetComponent<TextMesh>().text = (current_score).ToString();
            object_exists = false;
        }

    }


    private void initGame()
    {
        movingItems[0] = fencePrefab;
        movingItems[1] = wallEPrefab;
        movingItems[2] = elephantPrefab;

        startGameButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
        startGameButton.name = "StartGame";
        startGameButton.GetComponent<Renderer>().material.color = Color.black;
        startGameButton.GetComponent<Renderer>().shadowCastingMode = 0;
        startGameButton.transform.localScale = new Vector3(2f, 1f, 0.5f);
        startGameButton.transform.position = new Vector3(261.312f, 5.26f, 111.51f);

        text = new GameObject();
        text.AddComponent<TextMesh>();
        text.GetComponent<TextMesh>().text = "Start";
        text.GetComponent<TextMesh>().name = "start";
        text.GetComponent<TextMesh>().fontSize = 70;       
        text.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        text.GetComponent<TextMesh>().transform.localPosition = new Vector3(260.602f, 5.646f, 111.51f); 
    }

    private void gameOver()
    {
        game_over = true;
        Destroy(moving.CurrentObject);
        object_exists = false;

        text_gameover = new GameObject();
        text_gameover.AddComponent<TextMesh>();
        text_gameover.GetComponent<TextMesh>().text = "Game over \nScore : " + current_score + "\nTime : " + timerObj.GetComponent<TextMesh>().text;
        text_gameover.GetComponent<Renderer>().material.color = Color.red;
        text_gameover.GetComponent<TextMesh>().fontSize = 70;
        text_gameover.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        text_gameover.GetComponent<TextMesh>().transform.localPosition = new Vector3(259.7f, 8.92f, 112.55f);

        score_text.GetComponent<TextMesh>().text = "";
        Destroy(score_text);
        score_board.GetComponent<TextMesh>().text = "";
        Destroy(score_board);
        time_text.GetComponent<TextMesh>().text = "";
        Destroy(time_text);
        timerObj.GetComponent<TextMesh>().text = "";
        Destroy(timerObj);

        initGame();
        start_game = false;
    }

    private void startScore()
    {
        score_text = new GameObject();
        score_text.AddComponent<TextMesh>();
        score_text.transform.position = new Vector3(246.28f, 22f, 175.723f);
        score_text.GetComponent<TextMesh>().text = "Score:";
        score_text.GetComponent<TextMesh>().fontSize = 20;

        score_board = new GameObject();
        score_board.AddComponent<TextMesh>();
        score_board.transform.position = new Vector3(252.8f, 21.9f, 175.723f);
        score_board.GetComponent<TextMesh>().text = "0";
        score_board.GetComponent<TextMesh>().fontSize = 20;

        current_score = 0;
    }

    private void startTimer()
    {
        time_text = new GameObject();
        time_text.AddComponent<TextMesh>();
        time_text.transform.position = new Vector3(263.48f, 22f, 175.723f);
        time_text.GetComponent<TextMesh>().text = "Time:";
        time_text.GetComponent<TextMesh>().fontSize = 20;

        timerObj = new GameObject();
        timerObj.AddComponent<TextMesh>();
        timerObj.transform.position = new Vector3(269.48f,21.9f, 175.723f);
        timerObj.GetComponent<TextMesh>().fontSize = 20;

        startTime = Time.time;
    }

    private void updateTime()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString("f0");
        string seconds = (t % 60).ToString("f0");

        timerObj.GetComponent<TextMesh>().text = minutes + ":" + seconds;
    }



}
