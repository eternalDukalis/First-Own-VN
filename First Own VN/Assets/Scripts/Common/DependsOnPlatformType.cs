using UnityEngine;
using System.Collections;

public class DependsOnPlatformType : MonoBehaviour {

    public bool DesktopOnly = true;
    public bool MobileOnly = false;
	void Start ()
    {
        if ((DesktopOnly) && (Application.isMobilePlatform))
            gameObject.SetActive(false);
        if ((MobileOnly) && (!Application.isMobilePlatform))
            gameObject.SetActive(false);
	}
	
	void Update ()
    {
	
	}
}
