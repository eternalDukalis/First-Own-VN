using UnityEngine;
using System.Collections;

public class ControlManager : MonoBehaviour {

    static bool next = false; //Переменная, хранящая, была ли нажата клавиша продолжения
	void Start () 
    {
	
	}
	
	void Update () 
    {
        if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Return))) //Если нажат пробел или Enter
            next = true; //То клавиша продолжения нажата
	}

    static public bool Next() //Функция для определения, была ли нажата клавиша продолжения
    {
        if (next) //Если переменная равна true
        {
            next = false; //То присваеваем переменной false
            return true; //Возвращаем true
        }
        return false; //Иначе возвращаем false
    }

    public virtual void PressAll() //Функция, запускаемая при нажатии кнопки на весь экран
    {
        next = true; //Клавиша продолжения нажата
    }
}
