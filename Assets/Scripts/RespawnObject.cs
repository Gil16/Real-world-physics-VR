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

    public static bool tutorial = false;

    public static int demand_ball = RANDOM_BALL;

    

    public static float startTime;

    private static MovingObject tut1 = new MovingObject();

    private static MovingObject tut2 = new MovingObject();

    private static GameObject transparent_wall;

    private static MovingObject tut3 = new MovingObject();



    private static GameObject startText;

    private static GameObject startGameButton;

    private static GameObject tutorialText;

    private static GameObject tutorialGameButton;

    private static GameObject score_board;

    private static GameObject instruction1;

    private static GameObject Tut1_instruction;

    private static GameObject Tut2_instruction;

    private static GameObject Tut3_instruction;

    private static GameObject timer_text_count;

    private static GameObject score_text;

    private static GameObject time_text;

    private static GameObject timerObj;

    private static GameObject text_gameover;

    private static GameObject[] movingItems = new GameObject[NUMBER_OF_ITEMS];


    private static float current_time_tut;

    private const int RANDOM_BALL = 3;

    private const int NUMBER_OF_ITEMS = 3;

    private static int current_tut_num;

    private const int BOMB_BALL_DAMAGE = 200;

    private const int WOODEN_BALL_DAMAGE = 100;

    private const int SPIKE_BALL_DAMAGE = 150;

    private static bool tut_timer = false;

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

        public MovingFence(GameObject fence, Vector3 position) {

            Hp = 100;
            Score = 5;
            Speed = 0f;
            StartingPosition = position;
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
            StartingPosition = new Vector3(256.665f, 1f, 170.023f);
            Scale = new Vector3(0.2f, 0.2f, 0.2f);
            Rotation = Quaternion.Euler(0f,90f,0f);
            CurrentObject = Instantiate(wallE);

            CurrentObject.transform.position = StartingPosition;
            CurrentObject.transform.localScale = Scale;
            CurrentObject.transform.localRotation = Rotation;
        }

        public MovingWallE(GameObject wallE, Vector3 position)
        {
            Hp = 180;
            Score = 10;
            Speed = 0f;
            StartingPosition = position;
            Scale = new Vector3(0.2f, 0.2f, 0.2f);
            Rotation = Quaternion.Euler(0f, 90f, 0f);
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
            Hp = 100;
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

        public MovingElephant(GameObject elephant,Vector3 position)
        {
            Hp = 100;
            Score = 6;
            Speed = 4f;
            StartingPosition = position;
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
	
	// Update is called once per frame
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
                if (LeftHand_BallManager.ball.Ball_Object.GetComponent<Rigidbody>())
                {
                    Destroy(LeftHand_BallManager.ball.Ball_Object);
                }
                gameOver();
            }
        }

        if (tut_timer) {
            int curr_timer = (int)(3 - (Mathf.Floor(Time.realtimeSinceStartup - current_time_tut)));
            if (current_tut_num == 4)
            {
                timer_text_count.GetComponent<TextMesh>().text = "Good job tutorial is finished!";
            }
            else
            {
                timer_text_count.GetComponent<TextMesh>().text = "Good Job!, Next tutorial in : " + curr_timer;
            }
            if (current_time_tut + 3f < Time.realtimeSinceStartup) {                
                timer_text_count.GetComponent<TextMesh>().text = "";                
            }
            if (current_time_tut + 4f < Time.realtimeSinceStartup) {
                tut_timer = false;
                startTutorial(current_tut_num);
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "StartGame")
        {
            DestoryButtons();

            start_game = true;
            tutorial = false;
            startTimer();
            startScore();
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.name == "TutorialButton") {
            DestoryButtons();
            Destroy(gameObject);
            startTutorial(current_tut_num);
        }
        if (collision.gameObject.name == "Tutorial1")
        {
            Destroy(collision.gameObject);
            Tut1_instruction.GetComponent<TextMesh>().text = "";
            current_time_tut = Time.realtimeSinceStartup;
            tut_timer = true;
            Destroy(Tut1_instruction);
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.name == "Transparent_Tutorial2")
        {
            demand_ball = 2;
            Tut2_instruction.GetComponent<TextMesh>().text = "Good Job!\nThe bomb can destroy any object in one hit";
            Destroy(collision.gameObject);
            Destroy(gameObject);
            tut2.CurrentObject.GetComponent<Rigidbody>().detectCollisions = true;
            tut2.Hp = 1;
            return;
        }
        else if (collision.gameObject.name == "Tutorial2" && transparent_wall == null)
        {
            Destroy(collision.gameObject);
            Tut2_instruction.GetComponent<TextMesh>().text = "";
            Destroy(Tut2_instruction);
            current_time_tut = Time.realtimeSinceStartup;
            tut_timer = true;

            GameObject exp = Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
            Destroy(exp, 3);
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.name == "Tutorial3") {
            Tut3_instruction.GetComponent<TextMesh>().text = "";
            Destroy(Tut3_instruction);
            Destroy(gameObject);
            Destroy(collision.gameObject);
            current_time_tut = Time.realtimeSinceStartup;
            tut_timer = true;
            LeftHand_BallManager.wind_on = false;
            tutorial = false;
            return;
        }
        if (collision.gameObject.name == "Plane")
        {
            Destroy(gameObject);
            return;
        }
        if (!start_game)
        {
            return;
        } 
    
        if (!gameObject.GetComponent<Rigidbody>())
        {
            gameOver();
            return;
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

    private void DestoryButtons()
    {
        if (game_over)
        {
            text_gameover.GetComponent<TextMesh>().text = "";
            Destroy(text_gameover);
            game_over = false;
        }
        Destroy(startGameButton);
        Destroy(tutorialGameButton);

        startText.GetComponent<TextMesh>().text = "";
        Destroy(startText);

        tutorialText.GetComponent<TextMesh>().text = "";
        Destroy(tutorialText);

        if (instruction1)
        {
            instruction1.GetComponent<TextMesh>().text = "";
            Destroy(instruction1);
        }
    }

    private void initGame()
    {
        movingItems[0] = fencePrefab;
        movingItems[1] = wallEPrefab;
        movingItems[2] = elephantPrefab;
        current_tut_num = 1;
        LeftHand_BallManager.wind_on = false;        
        tutorial = false;

        timer_text_count = new GameObject();
        timer_text_count.AddComponent<TextMesh>();
        timer_text_count.GetComponent<TextMesh>().text = "";
        timer_text_count.GetComponent<TextMesh>().fontSize = 120;
        timer_text_count.GetComponent<Renderer>().material.color = Color.white;
        timer_text_count.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        timer_text_count.GetComponent<TextMesh>().transform.localPosition = new Vector3(248.92f, 16.08f, 147.36f);
        timer_text_count.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;

        startGameButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
        startGameButton.name = "StartGame";
        startGameButton.GetComponent<Renderer>().material.color = Color.black;
        startGameButton.GetComponent<Renderer>().shadowCastingMode = 0;
        startGameButton.transform.localScale = new Vector3(2.8f, 1f, 0.5f);
        startGameButton.transform.position = new Vector3(258.5f, 5.48f, 111.51f);

        startText = new GameObject();
        startText.AddComponent<TextMesh>();
        startText.GetComponent<TextMesh>().text = "Start";
        startText.GetComponent<TextMesh>().name = "start";
        startText.GetComponent<TextMesh>().fontSize = 70;
        startText.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        startText.GetComponent<TextMesh>().transform.localPosition = new Vector3(257.65f, 5.87f, 111.51f);

        tutorialGameButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tutorialGameButton.name = "TutorialButton";
        tutorialGameButton.GetComponent<Renderer>().material.color = Color.black;
        tutorialGameButton.GetComponent<Renderer>().shadowCastingMode = 0;
        tutorialGameButton.transform.localScale = new Vector3(2.8f, 1f, 0.5f);
        tutorialGameButton.transform.position = new Vector3(263.81f, 5.48f, 111.51f);

        tutorialText = new GameObject();
        tutorialText.AddComponent<TextMesh>();
        tutorialText.GetComponent<TextMesh>().text = "Tutorial";
        tutorialText.GetComponent<TextMesh>().name = "tutorial";
        tutorialText.GetComponent<TextMesh>().fontSize = 70;
        tutorialText.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        tutorialText.GetComponent<TextMesh>().transform.localPosition = new Vector3(262.71f, 5.87f, 111.51f);

        if (!game_over)
        {
            instruction1 = new GameObject();
            instruction1.AddComponent<TextMesh>();
            instruction1.GetComponent<TextMesh>().text = "Choose your option by throwing the ball";
            instruction1.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
            instruction1.GetComponent<TextMesh>().name = "instruction1";
            instruction1.GetComponent<TextMesh>().fontSize = 120;
            instruction1.GetComponent<Renderer>().material.color = Color.white;
            instruction1.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            instruction1.GetComponent<TextMesh>().transform.localPosition = new Vector3(248.92f, 16.08f, 147.36f);
        }
    }

    private void Tutorial1()
    {
        demand_ball = 1;
        tut1 = new MovingFence(movingItems[0], new Vector3(254.665f, 1.02f, 130f));
        tut1.CurrentObject.transform.name = "Tutorial1";

        Tut1_instruction = new GameObject();
        Tut1_instruction.AddComponent<TextMesh>();
        Tut1_instruction.GetComponent<TextMesh>().text = "Preform faster throwing movement for a higher velocity";
        Tut1_instruction.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
        Tut1_instruction.GetComponent<TextMesh>().name = "Tut1_instruction";
        Tut1_instruction.GetComponent<TextMesh>().fontSize = 120;
        Tut1_instruction.GetComponent<Renderer>().material.color = Color.white;
        Tut1_instruction.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Tut1_instruction.GetComponent<TextMesh>().transform.localPosition = new Vector3(248.92f, 16.08f, 147.36f);
    }


    private void Tutorial2()
    {
        tut2 = new MovingWallE(movingItems[1], new Vector3(254.665f, 1.02f, 130f));
        tut2.CurrentObject.transform.name = "Tutorial2";
        tut2.CurrentObject.GetComponent<Rigidbody>().useGravity = false;
        tut2.CurrentObject.GetComponent<Rigidbody>().detectCollisions = false;

        transparent_wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        transparent_wall.transform.name = "Transparent_Tutorial2";
        transparent_wall.transform.position = new Vector3(257.59f, 4.64f, 129.05f);
        transparent_wall.transform.localScale = new Vector3(5f, 5f, 0.2f);
        transparent_wall.GetComponent<BoxCollider>().center = new Vector3(-0.001132202f, -0.009615612f, 0);
        transparent_wall.GetComponent<BoxCollider>().size = new Vector3(1.186383f, 1.449679f, 1f);


        Tut2_instruction = new GameObject();
        Tut2_instruction.AddComponent<TextMesh>();
        Tut2_instruction.GetComponent<TextMesh>().text = "Some items require more than one hit";
        Tut2_instruction.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
        Tut2_instruction.GetComponent<TextMesh>().name = "Tut2_instruction";
        Tut2_instruction.GetComponent<TextMesh>().fontSize = 120;
        Tut2_instruction.GetComponent<Renderer>().material.color = Color.white;
        Tut2_instruction.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Tut2_instruction.GetComponent<TextMesh>().transform.localPosition = new Vector3(248.92f, 16.08f, 147.36f);
    }

    private void Tutorial3()
    {
        demand_ball = RANDOM_BALL;

        tut3 = new MovingElephant(movingItems[2], new Vector3(261.4f, 6.32f, 144.79f));
        tut3.CurrentObject.transform.name = "Tutorial3";
        tut3.CurrentObject.GetComponent<Rigidbody>().useGravity = true;
        tut3.CurrentObject.GetComponent<Rigidbody>().drag = 0;
        tut3.CurrentObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        tut3.CurrentObject.GetComponent<Rigidbody>().freezeRotation = true;

        Tut3_instruction = new GameObject();
        Tut3_instruction.AddComponent<TextMesh>();
        Tut3_instruction.GetComponent<TextMesh>().text = "The ball moves in the wind direction";
        Tut3_instruction.GetComponent<TextMesh>().fontStyle = FontStyle.Bold;
        Tut3_instruction.GetComponent<TextMesh>().name = "Tut3_instruction";
        Tut3_instruction.GetComponent<TextMesh>().fontSize = 120;
        Tut3_instruction.GetComponent<Renderer>().material.color = Color.white;
        Tut3_instruction.GetComponent<TextMesh>().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Tut3_instruction.GetComponent<TextMesh>().transform.localPosition = new Vector3(248.92f, 16.08f, 147.36f);

        LeftHand_BallManager.wind_on = true;
        LeftHand_BallManager.right_wind = true;

    }

    private void startTutorial(int num)
    {
        tutorial = true;
        switch (num)
        {
            case 1:
                Tutorial1();
                current_tut_num++;
                break;
            case 2:
                Tutorial2();
                current_tut_num++;
                break;
            case 3:
                Tutorial3();
                current_tut_num++;
                break;
            default:
                initGame(); ////////////////////////// MAYBE CAUSE PROBLEM!!
                break;
        }
        
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
