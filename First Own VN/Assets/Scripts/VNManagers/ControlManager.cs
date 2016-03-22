using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour {

    static bool next = false; //Переменная, хранящая, была ли нажата клавиша продолжения
    public GameObject StoryObject; //Экран истории
    public GameObject LoadScreen; //Экран загрузки
    public GameObject SaveScreen; //Экран сохранения
    public Navigation NavObject; //Компонент навигации
	void Start () 
    {
	
	}
	
	void Update () 
    {
        if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.RightArrow)) || (Input.mouseScrollDelta.y < 0)) //Если нажат пробел или Enter или стрелка вправо
            next = true; //То клавиша продолжения нажата
        if ((ScenarioManager.PlayingMode)) //Если в режиме проигрывания
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow)) || (Input.mouseScrollDelta.y > 0)) //Если нажата стрелка влево
            {
                NavObject.GoTo(StoryObject); //Переходим на экран истории
                StoryObject.GetComponentInChildren<Scrollbar>().Select();
            }
            if (Input.GetKeyDown(KeyCode.S)) //Если нажата клавиша S
            {
                NavObject.GoTo(SaveScreen); //Переходим на экран сохранения
            }
            if (Input.GetKeyDown(KeyCode.L)) //Если нажата клавиша L
            {
                NavObject.GoTo(LoadScreen); //Переходим на экран загрузки
            }
        }
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
        if (!Input.GetMouseButtonUp(0)) //Если не кликнута левая кнопка мыши
            return; //То отмена
        next = true; //Клавиша продолжения нажата
    }
}
