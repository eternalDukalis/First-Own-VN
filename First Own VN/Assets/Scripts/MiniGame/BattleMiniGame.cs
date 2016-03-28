using UnityEngine;
using System.Collections;

public class BattleMiniGame : MonoBehaviour {

    public float TimeToPlay = 5;
    public float TimeLeft
    {
        get
        {
            return timeLeft / TimeToPlay;
        }
    }
    public int ItemsCount = 5;
    public GameObject BItem;
    public GameObject LoseScreen;
    public GameObject WinScreen;
    public GameObject ObjWithItems;
    float timeLeft;
	void Start ()
    {
        //Init();
	}
	
	void Update ()
    {
	
	}

    public virtual void Init()
    {
        timeLeft = TimeToPlay;
        foreach (BattleItem x in FindObjectsOfType<BattleItem>())
            Destroy(x.gameObject);
        StartCoroutine(play());
    }

    IEnumerator play()
    {
        bool success = false;
        int items = 0;
        GameObject obj = SetItem();
        while (timeLeft > 0)
        {
            if (obj.GetComponent<BattleItem>().Pressed)
            {
                items++;
                if (items >= ItemsCount)
                {
                    success = true;
                    break;
                }
                obj = SetItem();
            }
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        if (success)
            WinScreen.SetActive(true);
        else
            LoseScreen.SetActive(true);
    }

    GameObject SetItem()
    {
        GameObject res = Instantiate(BItem);
        res.transform.SetParent(ObjWithItems.transform, false);
        res.GetComponent<BattleItem>().Init();
        return res;
    }

    public virtual void End()
    {
        MiniGamesManager.StopGame();
    }
}
