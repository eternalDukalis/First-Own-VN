using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour {

    public State NewGame;
    public float FadeTime = 1;
    public AudioSource Source;
    public GameObject Scr;
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
        LoadGame();
    }

    public virtual void Continue()
    {
        Saves.Continue();
    }

    public virtual void Quit()
    {
        Application.Quit();
    }

    public static void LoadGame()
    {
        MenuInteraction obj = FindObjectOfType<MenuInteraction>();
        if (obj != null)
        {
            obj.Scr.SetActive(true);
            obj.StartCoroutine(obj.fade(obj.Scr.GetComponent<Image>(), obj.Source));
        }
        else
            SceneManager.LoadScene("game"); //Загружаем уровень
    }

    IEnumerator fade(Image img, AudioSource src)
    {
        float val = 0;
        while (val < 1)
        {
            val += Time.deltaTime / FadeTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, val);
            src.volume = 1 - val;
            yield return null;
        }
        src.volume = 0;
        SceneManager.LoadScene("game"); //Загружаем уровень
    }
}
