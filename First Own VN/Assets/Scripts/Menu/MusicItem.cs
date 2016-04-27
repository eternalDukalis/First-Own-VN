using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicItem : MonoBehaviour {

    public string Title; //Название трека
    public Text TargetText; //Текст
    public string Author; //Исполнитель
    string LabelFormal = "<b>{0}</b> - {1}"; //Формат текста
    bool Available; //Доступность
    Toggle toggle; //Кнопка
	void Start ()
    {
        toggle = GetComponent<Toggle>(); //Находим кнопку
        Init(); //Инициализируем
	}
	
	void Update ()
    {
        if (Available != MusicGallery.Exists(Title)) //Если доступность изменилась
            Init(); //Тогда инициализируем
	}

    void Init() //Инициализация
    {
        Available = MusicGallery.Exists(Title); //Получаем доступность
        if (Available) //Если доступно
        {
            TargetText.gameObject.SetActive(true); //Делаем текст видимым
            TargetText.text = string.Format(LabelFormal, Author, Title); //Применяем текст
            toggle.interactable = true; //Делаем кнопку кликабельной
        }
        else //Иначе
        {
            TargetText.gameObject.SetActive(false); //Делаем текст невидимым
            toggle.interactable = false; //Делаем кнопку некликабельной
        }
    }
}
