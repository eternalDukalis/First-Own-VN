using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptAnalizer : MonoBehaviour {

    class CurrentState
    {
        string Name;
        string Clothes;
        string Emotion;
        SortedList<string, int> Attributes;
        public CurrentState(string name)
        {
            Name = name;
            Clothes = "";
            Emotion = "";
            Attributes = new SortedList<string, int>();
        }
        public void SetClothes(string clothes)
        {
            Clothes = clothes;
        }
        public void SetEmotion(string emotion)
        {
            Emotion = emotion;
        }
        public void AddAttribute(string attr)
        {
            if (Attributes == null)
                Attributes = new SortedList<string, int>();
            if (!Attributes.ContainsKey(attr))
                Attributes.Add(attr, 1);
        }
        public void DeleteAttribute(string attr)
        {
            if (Attributes == null)
                return;
            if (Attributes.ContainsKey(attr))
                Attributes.Remove(attr);
        }
        public void DeleteAllAttributes()
        {
            Attributes.Clear();
        }
        public override string ToString()
        {
            string s = Clothes + "/" + Emotion;
            if (Attributes == null)
                return s;
            foreach (KeyValuePair<string, int> x in Attributes)
                s += "_" + x.Key;
            return s;
        }
        public string GetEmotion()
        {
            return Emotion;
        }
    }
    static Dictionary<string, CurrentState> States;
    static List<string> Commands;
    string Path = "Commands";

	void Start ()
    {
        States = new Dictionary<string, CurrentState>();
        string[] com = Resources.Load<TextAsset>(Path).text.Split('\n');
        for (int i = 0; i < com.Length; i++)
        {
            com[i] = ScenarioManager.DeleteSpacesAtTheEnd(com[i]);
        }
        Commands = new List<string>(com);
    }

    public static string AllSprites(string startPoint, bool lacking)
    {
        string s = "";
        SortedList<string, int> lst = new SortedList<string, int>(SpritesList(startPoint));
        foreach (KeyValuePair<string, int> x in lst)
        {
            if ((!lacking) || (Resources.Load("Graphics/Sprites/" + x.Key) == null))
                s += x.Key + " - " + x.Value.ToString() + ";\n";
        }
        return s;
    }

    public static Dictionary<string, int> SpritesList(string startPoint)
    {
        Dictionary<string, int> s = new Dictionary<string, int>();
        string[] operations = Resources.Load<TextAsset>(ScenarioManager.ScenarioPath + startPoint).text.Split('\n');
        for (int i = 0; i < operations.Length; i++)
        {
            operations[i] = ScenarioManager.DeleteSpacesAtTheEnd(operations[i]);
            string[] op = operations[i].Split('|');
            if ((op.Length > 1))
            {
                switch (op[0])
                {
                    case "goto":
                        s = DictAssociation(s, SpritesList(op[1]));
                        break;
                    case "select":
                        for (int k = 2; k < op.Length; k += 2)
                            s = DictAssociation(s, SpritesList(op[k]));
                        break;
                    case "ifgoto":
                        s = DictAssociation(s, SpritesList(op[3]));
                        break;
                    default:
                        string name = "", cl = "", em = "", attr = "";
                        bool delall = false;
                        switch (op[0])
                        {
                            case "setactor":
                                name = op[1];
                                em = op[op.Length - 1];
                                break;
                            case "changeemo":
                                name = op[1];
                                em = op[2];
                                break;
                            case "attribute":
                                name = op[1];
                                attr = "+" + op[2];
                                break;
                            case "delattribute":
                                name = op[1];
                                attr = "-" + op[2];
                                break;
                            case "delactor":
                                name = op[1];
                                delall = true;
                                break;
                            case "changeclothes":
                                name = op[1];
                                cl = op[2];
                                em = "---";
                                break;
                        }
                        if (name == "")
                            continue;
                        if (!States.ContainsKey(name))
                            States.Add(name, new CurrentState(name));
                        if (cl != "")
                            States[name].SetClothes(cl);
                        if (em != "")
                        {
                            if (em == "---")
                                States[name].SetEmotion("");
                            else
                                States[name].SetEmotion(em);
                        }
                        if (attr != "")
                        {
                            if (attr[0] == '+')
                                States[name].AddAttribute(attr.Substring(1));
                            else
                                States[name].DeleteAttribute(attr.Substring(1));
                        }
                        if (delall)
                        {
                            States[name].DeleteAllAttributes();
                        }
                        if (States[name].GetEmotion() == "")
                            break;
                        string key = name + "/" + States[name].ToString();
                        if (s.ContainsKey(key))
                            s[key]++;
                        else
                            s.Add(key, 1);
                        break;
                }
            }
        }
        return s;
    }

    public static IndicatorManager.Indicator GetIndicator(string[] operations, int index)
    {
        for (int i = index + 1; i < operations.Length; i++)
        {
            string[] op = operations[i].Split('|');
            if ((op.Length == 1) || (!Commands.Contains(op[0])))
                return IndicatorManager.Indicator.Push;
            if (op[0] == "add")
                return IndicatorManager.Indicator.Add;
            if (((op[0] == "switchtextform") || (op[0] == "clearform")) && (!IndicatorManager.BottomTF))
                return IndicatorManager.Indicator.Page;
        }
        return IndicatorManager.Indicator.Push;
    }

    static Dictionary<string, int> DictAssociation(Dictionary<string, int> a, Dictionary<string, int> b)
    {
        Dictionary<string, int> s = new Dictionary<string, int>(a);
        foreach (KeyValuePair<string, int> x in b)
        {
            if (s.ContainsKey(x.Key))
                s[x.Key] += x.Value;
            else
                s.Add(x.Key, x.Value);
        }
        return s;
    }
}
