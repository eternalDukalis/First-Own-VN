using UnityEngine;
using System.Collections;

public class MiniGamesManager : MonoBehaviour {

    public GameObject[] Games;
    public GameObject Parent;
    GameObject currentGame;
	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void PlayGame(int num)
    {
        currentGame = Instantiate(Games[num]);
        currentGame.transform.SetParent(Parent.transform, false);
        ScenarioManager.LockCoroutine();
    }

    static public void StopGame()
    {
        MiniGamesManager mgm = FindObjectOfType<MiniGamesManager>();
        Destroy(mgm.currentGame);
        ScenarioManager.UnlockCoroutine();
    }
}
