using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleTimeLine : MonoBehaviour {

    public BattleMiniGame game;
    public Color TimeIsOutColor;
    Color FullTimeColor;
    Image img;
    float val;
	void Start ()
    {
        img = GetComponent<Image>();
        FullTimeColor = img.color;
        val = game.TimeLeft;
	}
	
	void Update ()
    {
        if (val != game.TimeLeft)
            SetTime();
	}

    void SetTime()
    {
        img.rectTransform.anchorMax = new Vector2(img.rectTransform.anchorMax.x, game.TimeLeft);
        img.color = FullTimeColor * game.TimeLeft + TimeIsOutColor * (1 - game.TimeLeft);
    }
}
