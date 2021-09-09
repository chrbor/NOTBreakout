using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HoldButton;

public class TouchScript : MonoBehaviour
{
    public static TouchScript touchScript;

    public float tappTime = .1f;
    public float doubleTappTime = .1f;
    bool testDoubleTouch;

    [HideInInspector]
    public bool run;
    [HideInInspector]
    public bool tapp;
    [HideInInspector]
    public bool doubleTapp;

    int touchcount_old = 0;

    private void Awake()
    {
        touchScript = this;
    }

    private void FixedUpdate()
    {
        if (!run || buttonPressed) return;
        buttonsLocked |= Input.touchCount > 0;

        //reagiere auf touch:
        if(Input.touchCount > 0)
        {
            buttonsLocked = true;

            if (testDoubleTouch)
                testDoubleTouch = false;
            else StartCoroutine(TestDoubleTapp());
        }
        
        touchcount_old = Input.touchCount;
    }

    IEnumerator TestDoubleTapp()
    {
        testDoubleTouch = true;
        for(float count = 0; count < doubleTappTime; count += Time.fixedDeltaTime)
        {
            if (!testDoubleTouch) break;
            yield return new WaitForFixedUpdate();
        }
        if (testDoubleTouch)
        {
            testDoubleTouch = false;
            yield break;
        }

        doubleTapp = true;
        yield return new WaitForEndOfFrame();
        doubleTapp = false;
    }

    IEnumerator SetTapp()
    {
        tapp = true;
        yield return new WaitForEndOfFrame();
        tapp = false;
        yield break;
    }
}
