using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static LoadSave;
using static TouchScript;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public static bool gameRun;
    public static bool gamePause;

    public static int activeBalls;
    public static int boxCount;

    public Vector2 ballGravity;

    public UnityAction getActiveBalls;

    [SerializeField]
    public GameAttributes attributes;
    [System.Serializable]
    public class GameAttributes
    {
        public bool tiltOnly;
        public float ballGravity;
        public float objGravity;
        public bool lock_ballGravity;
        public bool lock_objGravity;


        public GameAttributes()
        {
            tiltOnly = false;
            ballGravity = 1;
            objGravity = 1;
            lock_ballGravity = false;
            lock_objGravity = false;
        }
    }

    public Text text;

    private void Awake()
    {
        manager = this;

        gameRun = false;
        gamePause = false;

        LoadProgress();
        Input.gyro.enabled = true;
        TiltScript.SetTiltSensitivity(4);

        StartCoroutine(RunGame());
    }

    IEnumerator RunGame()
    {
        //Aktualisiere Ball-counter:
        activeBalls = 0;
        yield return new WaitForFixedUpdate();
        if(getActiveBalls != null) getActiveBalls.Invoke();
        if (activeBalls < 1) { Debug.Log("Error: no balls found!"); yield break; }

        boxCount = 0;

        /*
        //Preparationsphase, in der der Spieler das Level betrachten kann:
        Debug.Log("wait for start");
        touchScript.run = true;
        yield return new WaitUntil(() => Input.touchCount == 0);
        while (!touchScript.tapp)
        {
            //Bewege die camera entsprechend der Streich-geste:

            yield return new WaitForFixedUpdate();
        }
        */
        //Game-Loop:
        Debug.Log("run game");
        gameRun = true;
        while (activeBalls > 0 && gameRun)
        {
            //Stoppe, wenn das Spiel pausiert:
            if (gamePause) yield return new WaitWhile(() => gamePause);

            //Aktualisiere Gravitation:
            if (!attributes.lock_ballGravity)
            {
                ballGravity = TiltScript.Get2DTilt() * attributes.ballGravity;
            }

            //Aktualisiere Camerapoisiton:


            yield return new WaitForEndOfFrame();
        }
        Debug.Log("game over");

        if (gameRun)
        {
            text.text = "You lose!";
        }
        else
        {
            text.text = "You win!";
        }

        yield break;
    }

    public void Calibrate()
    {
        TiltScript.SetTiltOffset(Input.gyro.gravity);
    }
}
