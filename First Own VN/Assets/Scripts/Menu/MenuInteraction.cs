using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuInteraction : MonoBehaviour {

    public State NewGame;
    void Start ()
    {

    }
	
	void Update ()
    {
	
	}

    public virtual void BeginGame()
    {
        State.CurrentState = new State(NewGame);
        State.CurrentState.CurrentInstruction = -1;
        Settings.HasStarted = true;
        SceneManager.LoadScene("game");
    }

    public virtual void Continue()
    {
        Saves.Continue();
    }

    public virtual void Quit()
    {
        Application.Quit();
    }
}
