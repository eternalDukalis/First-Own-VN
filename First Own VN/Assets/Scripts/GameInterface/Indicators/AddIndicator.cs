using UnityEngine;
using System.Collections;

public class AddIndicator : MonoBehaviour {

    [SerializeField]
    float TurnTime = 1;
    int HalfCircle = 360;
	void Start ()
    {
        StartCoroutine(turning());
	}

    IEnumerator turning()
    {
        float tm = 0;
        while (true)
        {
            tm += Time.deltaTime;
            if (tm > TurnTime)
                tm -= TurnTime;
            GetComponent<RectTransform>().localRotation = Quaternion.Euler(tm * (float)HalfCircle / TurnTime, 0, 0);
            yield return null;
        }
    }
}