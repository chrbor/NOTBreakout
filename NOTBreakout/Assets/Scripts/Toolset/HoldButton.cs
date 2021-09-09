using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool useHold = true;
    public UnityEvent OnTipped, OnHold, OnPressed, OnReleased;

    public static bool buttonPressed;
    public static bool buttonsLocked;

    protected Sprite normal;
    public Sprite selectedSprite;
    public Sprite clickedSprite;
    public Sprite lockedSprite;

    Image image;

    protected bool pointerDown;
    protected bool selected;
    
    void Awake()
    {

        image = GetComponent<Image>();
        normal = image.sprite;
        ResetButton();
    }

    public void ResetButton()
    {
        pointerDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;

        //switch(button_type)
        if (!(pointerDown || buttonsLocked))
        {
            pointerDown = true;

            //Do stuff on press:
            //image.color = new Color(1, 1, 1, 0.75f);
            image.sprite = clickedSprite;
            OnPressed.Invoke();
            if(useHold) StartCoroutine(HoldCounter());
        }
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        if (!buttonsLocked)
        {
            //pointerDown = false;
            buttonPressed = false;

            //Do stuff on release:
            image.sprite = normal;
            OnTipped.Invoke();
        }
        OnReleased.Invoke();
    }

    bool counting;
    IEnumerator HoldCounter()
    {
        if (counting) yield break;
        counting = true;

        for(float count = 0; count < 1 && pointerDown; count += Time.fixedDeltaTime) yield return new WaitForFixedUpdate();
        if (!pointerDown) { counting = false; yield break; }

        selected = true;
        OnHold.Invoke();

        counting = false;
        yield break;
    }
}
