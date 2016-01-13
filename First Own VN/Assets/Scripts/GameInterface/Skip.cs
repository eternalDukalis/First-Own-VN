using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Skip : MonoBehaviour {

    static public bool isSkipping; //Переменная, хранящая, нужно ли пропускать сценарий
	void Start () 
    {
        isSkipping = false; //По умолчанию false
	}
	
	void Update () 
    {
        if ((Input.GetKey(KeyCode.LeftControl)) || (Input.GetKey(KeyCode.RightControl))) //Если зажат ctrl
        {
            isSkipping = true; //То нужно пропускать
        }
        else //Иначе
        {
            if (Input.GetKeyDown(KeyCode.Tab)) //Если нажат Tab
            {
                isSkipping = !isSkipping; //Переключаем режим
            }
        }
        if ((Input.GetKeyUp(KeyCode.LeftControl)) || (Input.GetKeyUp(KeyCode.RightControl))) //Если отпускается ctrl
        {
            isSkipping = false; //То не нужно пропускать
        }
	}

    public virtual void Switch() //Функция переключения режима
    {
        isSkipping = !isSkipping; //Переключаем режим
    }
}
