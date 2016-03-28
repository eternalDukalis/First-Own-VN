using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleItem : MonoBehaviour {

    public Vector2 StartPosition;
    public float Size = 0.1f;
    public bool Pressed
    {
        get
        {
            return pressed;
        }
    }
    bool pressed = false;
    Button button;
    void Start ()
    {
        Place();
    }
	
	void Update ()
    {
	
	}

    public void Init()
    {
        Vector2 sPos = new Vector2(Random.Range(Size / 2, 1 - Size / 2), Random.Range(Size / 2, 1 - Size / 2));
        Init(sPos);
    }

    public void Init(Vector2 startPosition)
    {
        StartPosition = startPosition;
        Place();
    }

    void Place()
    {
        button = GetComponent<Button>();
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchorMin = StartPosition - new Vector2(Size, Size) / 2;
        rect.anchorMax = StartPosition + new Vector2(Size, Size) / 2;
    }

    public virtual void Press()
    {
        pressed = true;
        button.interactable = false;
    }
}
