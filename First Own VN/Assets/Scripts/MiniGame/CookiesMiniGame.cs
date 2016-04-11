using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CookiesMiniGame : MonoBehaviour {

    class ItemSet
    {
        List<string> ilist;
        int _count = 0;
        public int Count
        {
            get
            {
                return _count;
            }
        }
        public ItemSet()
        {
            ilist = new List<string>();
        }
        public void Add(string item)
        {
            ilist.Add(item);
            _count++;
        }
        public string ElementAt(int index)
        {
            return ilist.ToArray()[index];
        }
        public string[] ToArray()
        {
            return ilist.ToArray();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            ItemSet iset = obj as ItemSet;
            if (iset.Count != Count)
                return false;
            foreach (string x in iset.ilist)
            {
                if (!ilist.Contains(x))
                    return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            int res = 0;
            foreach (string x in ilist)
            {
                res += x.GetHashCode();
            }
            return res;
        }
    }

    static List<string> Items;
    static Dictionary<string, string> PrepareRules;
    static Dictionary<string, string> BakeRules;
    static Dictionary<ItemSet, string> MixRules;
    public Button PrepareButton;
    public Button BakeButton;
    public Button MixButton;
    public GameObject WarningObject;
    public GameObject WinScreen;
    public Image CookiesPic;
    static ItemSet CurrentItemSet;
    int cnt = 0;
	void Start ()
    {
        CurrentItemSet = new ItemSet();
        Items = new List<string>();
        PrepareRules = new Dictionary<string, string>();
        BakeRules = new Dictionary<string, string>();
        MixRules = new Dictionary<ItemSet, string>();
        RecipesFilling();
    }
	
	void Update ()
    {
        if (cnt != CurrentItemSet.Count)
        {
            cnt = CurrentItemSet.Count;
            if (cnt == 0)
            {
                BakeButton.interactable = false;
                PrepareButton.interactable = false;
                MixButton.interactable = false;
            }
            else
            {
                if (cnt == 1)
                {
                    BakeButton.interactable = true;
                    PrepareButton.interactable = true;
                    MixButton.interactable = false;
                }
                else
                {
                    BakeButton.interactable = false;
                    PrepareButton.interactable = false;
                    MixButton.interactable = true;
                }
            }
        }
        if (Items.Count == 1)
        {
            CookiesPic.color = Color.green;
            WinScreen.SetActive(true);
        }
	}

    static public string GetItem(int num)
    {
        if ((Items == null) || (num >= Items.Count))
            return "";
        return Items.ToArray()[num];
    }

    static public void UpdateItemSet()
    {
        CurrentItemSet = new ItemSet();
        CookiesItem[] ci = FindObjectsOfType<CookiesItem>();
        foreach (CookiesItem x in ci)
        {
            if (x.Selected)
                CurrentItemSet.Add(GetItem(x.ItemNum));
        }
    }

    public virtual void Act(string action)
    {
        bool success = false;
        switch (action)
        {
            case "Prepare":
                if (PrepareRules.ContainsKey(CurrentItemSet.ElementAt(0)))
                {
                    success = true;
                    Items.Remove(CurrentItemSet.ElementAt(0));
                    Items.Add(PrepareRules[CurrentItemSet.ElementAt(0)]);
                }
                break;
            case "Bake":
                if (BakeRules.ContainsKey(CurrentItemSet.ElementAt(0)))
                {
                    success = true;
                    Items.Remove(CurrentItemSet.ElementAt(0));
                    Items.Add(BakeRules[CurrentItemSet.ElementAt(0)]);
                }
                break;
            case "Mix":
                if (MixRules.ContainsKey(CurrentItemSet))
                {
                    success = true;
                    foreach (string x in CurrentItemSet.ToArray())
                    {
                        Items.Remove(x);
                    }
                    Items.Add(MixRules[CurrentItemSet]);
                }
                break;
        }
        CookiesItem.UnselectAll();
        if (!success)
        {
            WarningObject.SetActive(true);
        }
    }

    public virtual void End()
    {
        gameObject.SetActive(false);
        MiniGamesManager.StopGame();
    }

    void RecipesFilling()
    {
        Items.Add("Мука");
        Items.Add("Молоко");
        Items.Add("Молоко");
        Items.Add("Яйца");
        Items.Add("Сахар");
        Items.Add("Сахар");
        Items.Add("Какао");

        PrepareRules.Add("Яйца", "Взбитые яйца");
        PrepareRules.Add("Шоколад", "Шоколадная крошка");

        BakeRules.Add("Тесто", "Печенье");
        BakeRules.Add("Основа для шоколада", "Шоколад");

        ItemSet set = new ItemSet();
        set.Add("Мука");
        set.Add("Молоко");
        set.Add("Сахар");
        set.Add("Взбитые яйца");
        MixRules.Add(set, "Тесто");
        set = new ItemSet();
        set.Add("Молоко");
        set.Add("Сахар");
        set.Add("Какао");
        MixRules.Add(set, "Основа для шоколада");
        set = new ItemSet();
        set.Add("Печенье");
        set.Add("Шоколадная крошка");
        MixRules.Add(set, "Шоколадное печенье");
    }
}
