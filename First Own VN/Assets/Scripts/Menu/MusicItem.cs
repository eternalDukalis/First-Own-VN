using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicItem : MonoBehaviour {

    public string Title; //Название трека
    public Text TargetText; //Текст
    bool Available; //Доступность
    Button button; //Кнопка
	void Start ()
    {
        button = GetComponent<Button>(); //Находим кнопку
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
            button.interactable = true; //Делаем кнопку кликабельной
        }
        else //Иначе
        {
            TargetText.gameObject.SetActive(false); //Делаем текст невидимым
            button.interactable = false; //Делаем кнопку некликабельной
        }
    }
}
