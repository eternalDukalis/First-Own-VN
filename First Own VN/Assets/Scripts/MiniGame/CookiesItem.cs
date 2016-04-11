using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CookiesItem : MonoBehaviour {

    public int ItemNum;
    public Text Title;
    public bool Selected = false;
    string itemname;
    Toggle toggle;
	void Start ()
    {
        toggle = GetComponent<Toggle>();
        GettingItem();
	}
	
	void Update ()
    {
        if (itemname != CookiesMiniGame.GetItem(ItemNum))
            GettingItem();
        if (Selected != toggle.isOn)
            Selected = toggle.isOn;
	}

    void GettingItem()
    {
        itemname = CookiesMiniGame.GetItem(ItemNum);
        Title.text = itemname;
        if (itemname == "")
            toggle.interactable = false;
        else
            toggle.interactable = true;
    }

    public virtual void ChangeValue()
    {
        Selected = toggle.isOn;
        CookiesMiniGame.UpdateItemSet();
    }

    static public void UnselectAll()
    {
        CookiesItem[] ci = FindObjectsOfType<CookiesItem>();
        foreach (CookiesItem x in ci)
        {
            x.toggle.isOn = false;
        }
    }
}
