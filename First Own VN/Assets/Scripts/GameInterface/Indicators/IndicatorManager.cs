using UnityEngine;
using System.Collections;

public class IndicatorManager : MonoBehaviour {

    public enum Indicator { Add, Push, Page };
    [SerializeField]
    GameObject BottomParent, FullParent, AddIndicator, PushIndicator, PageIndicator;
    Indicator CurrentIndicator;
    GameObject CurObj;

    public static bool BottomTF
    {
        get
        {
            return Instance.BottomParent.activeInHierarchy;
        }
    }
    public static IndicatorManager Instance
    {
        get
        {
            return FindObjectOfType<IndicatorManager>();
        }
    }
	void Start ()
    {
	
	}

    public void SetIndicatorType(Indicator ind)
    {
        CurrentIndicator = ind;
    }

    public void SetIndicator()
    {
        GameObject obj = AddIndicator;
        switch (CurrentIndicator)
        {
            case Indicator.Page:
                obj = PageIndicator;
                break;
            case Indicator.Push:
                obj = PushIndicator;
                break;
        }
        CurObj = Instantiate(obj);
        if (BottomTF)
        {
            CurObj.transform.SetParent(BottomParent.transform, false);
        }
        else
        {
            CurObj.transform.SetParent(FullParent.transform, false);
        }
    }

    public void DeleteIndicator()
    {
        Destroy(CurObj);
    }
}
