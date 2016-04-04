using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniGameCharacter : MonoBehaviour {

    public int Sex;
    public int Race;
    public int Hair;
    public int RightSex;
    public int RightRace;
    public int RightHair;
    public Button button;
    int SexMultiplier = 9;
    int RaceMultiplier = 3;
    int HairMultiplier = 1;
    Image img;
    string SpritesPath = "Graphics/MiniGames/sprite";
	void Start ()
    {
        img = GetComponent<Image>();
	}
	
	void Update ()
    {
	
	}

    public virtual void ChangeParam(string par)
    {
        int val = int.Parse(par.Substring(par.Length - 1));
        string nam = par.Substring(0, par.Length - 1);
        switch (nam)
        {
            case "Sex":
                Sex = val;
                break;
            case "Race":
                Race = val;
                break;
            case "Hair":
                Hair = val;
                break;
            default:
                Debug.LogError("Wrong minigame param");
                return;
        }
        int spriteNum = Sex * SexMultiplier + Race * RaceMultiplier + Hair * HairMultiplier + 1;
        Texture2D tex = Resources.Load<Texture2D>(SpritesPath + spriteNum.ToString());
        img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        if ((Sex == RightSex) && (Race == RightRace) && (Hair == RightHair))
            button.interactable = true;
        else
            button.interactable = false;
    }

    public virtual void End()
    {
        MiniGamesManager.StopGame();
    }
}
