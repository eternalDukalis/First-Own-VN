using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchItem : MonoBehaviour {

    public Text TargetText; //Компонент названия
    public Text Description; //Объект с описанием
    Toggle toggle; //Тоггл
    string Title; //Название
    bool Available; //Доступно ли
	void Start ()
    {
        toggle = GetComponent<Toggle>(); //Получаем тоггл
        Title = TargetText.text; //Получаем название
        Init(); //Инициализируем
	}
	
	void Update ()
    {
        if (Available != (Achievments.GetDescription(Title) != "")) //Если факт доступности изменился
            Init(); //Инициализируем
	}

    void Init() //Инициализация
    {
        Available = Achievments.GetDescription(Title) != ""; //Получаем доступность
        toggle.interactable = Available; //Изменяем интерактивность тоггла
        TargetText.gameObject.SetActive(Available); //Изменяем активность текста с навзанием
    }

    public virtual void PushDescription() //Функция показывания описания
    {
        if (toggle.isOn) //Если тоггл включён
            Description.text = Achievments.GetDescription(Title); //Помещаем описание в объект
    }
}
