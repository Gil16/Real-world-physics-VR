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
        Debug.Log("Right side = " + windFromLeft.particleCount);
        Debug.Log("Left side = " + windFromRight.particleCount);
        Debug.Log("Wind is " + LeftHand_BallManager.wind_on);
        if (LeftHand_BallManager.wind_on && !LeftHand_BallManager.right_wind && RespawnObject.start_game && !windFromRight.isPlaying)
        {
            windFromRight.Play();
        }
        else if (LeftHand_BallManager.wind_on && LeftHand_BallManager.right_wind && RespawnObject.start_game && !windFromLeft.isPlaying)
        {
            windFromLeft.Play();
        }
        else if (!LeftHand_BallManager.wind_on && RespawnObject.start_game)
        {
            windFromRight.Clear();
            windFromLeft.Clear();
            windFromLeft.Stop();            
            windFromRight.Stop();
        }
        else if (LeftHand_BallManager.wind_on &&  RespawnObject.game_over)
        {
            windFromLeft.Clear();
            windFromRight.Clear();
            windFromLeft.Stop();
            windFromRight.Stop();
            
        }                 
        
    }
}
