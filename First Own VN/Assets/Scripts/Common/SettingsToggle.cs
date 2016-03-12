using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour {

    public string FieldName;
    Toggle toggle;
	void Start ()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = Settings.GetField(FieldName, true);
	}
	
	void Update ()
    {
        if (toggle.isOn != Settings.GetField(FieldName, true))
            toggle.isOn = Settings.GetField(FieldName, true);
	}

    public virtual void SetValue()
    {
        Settings.SetField(FieldName, toggle.isOn);
    }
}
