using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Saves : MonoBehaviour {

    Dictionary<string, State> AllSaves;
    int saveNum
    {
        get
        {
            return _saveNum;
        }
        set
        {
            _saveNum = value;
            PlayerPrefs.SetInt("saveNum", value);
        }
    }
    string nameFormat = "save{0}";
    int _saveNum = 0;
	void Start () 
    {
        AllSaves = new Dictionary<string, State>();
        if (PlayerPrefs.HasKey("saveNum"))
            _saveNum = PlayerPrefs.GetInt("saveNum");
	}
	
	void Update () 
    {
	
	}

    public virtual void Save()
    {
        string filename = string.Format(nameFormat, saveNum);
        AllSaves.Add(filename, new State(State.CurrentState.PreviousState));
        saveNum++;
        PlayerPrefs.SetString(filename, AllSaves[filename].ToString());
    }

    public virtual void Load()
    {
        
    }

    void LoadInfo()
    {
 
    }
}
