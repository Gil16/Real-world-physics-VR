using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    public ParticleSystem windFromLeft;

    public ParticleSystem windFromRight;


    private static bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!start)
        {
            start = true;
            windFromLeft.Stop();
            windFromRight.Stop();
            windFromLeft.Clear();
            windFromRight.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftHand_BallManager.wind_on && LeftHand_BallManager.right_wind && RespawnObject.start_game && !windFromRight.isPlaying)
        {
            windFromLeft.Play();
        }
        else if (LeftHand_BallManager.wind_on && !LeftHand_BallManager.right_wind && RespawnObject.start_game && !windFromLeft.isPlaying)
        {
            windFromRight.Play();
            
        }
        else if (!LeftHand_BallManager.wind_on && RespawnObject.start_game)
        {
            windFromLeft.Stop();
            windFromRight.Stop();
            windFromRight.Clear();
            windFromLeft.Clear();
           
        }
        else if (LeftHand_BallManager.wind_on &&  RespawnObject.game_over)
        {
            windFromLeft.Stop();
            windFromRight.Stop();
            windFromLeft.Clear();
            windFromRight.Clear();
        }    
              
    }
}
