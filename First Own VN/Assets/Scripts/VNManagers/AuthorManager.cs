using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AuthorManager : MonoBehaviour {

    Image tForm; //Задний фон текстовой формы
    Text mText; //Сам текст
    static string Author = ""; //Статическая переменная для хранения автора
	void Start () 
    {
        tForm = transform.parent.gameObject.GetComponent<Image>(); //Получем задний фон
        mText = GetComponent<Text>(); //Получаем текст
        TurnOff(); //Скрываем форму автора
	}
	
	void Update () 
    {
	
	}

    public void DeleteAuthor() //Метод для удаления автора
    {
        Author = ""; //Обнуляем статическую переменную
        TurnOff(); //Убираем текстовую форму
    }

    public void UpdateAuthor(string author) //Метод для создания или обновления автора
    {
        TurnOn(); //Открываем текстовую форму
        Author = author; //Обновляем статическую переменную
        mText.text = Author; //Меняем текст на сцене
    }

    void TurnOff() //Скрытие формы автора
    {
        tForm.enabled = false; //Скрываем задний фон
        mText.enabled = false; //Скрываем текст
    }

    void TurnOn() //Открытие формы автора
    {
        tForm.enabled = true; //Открываем задний фон
        mText.enabled = true; //Открываем текст
    }
}
