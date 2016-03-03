using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveSlotData : MonoBehaviour {

    public Text SaveTime; //Компонент - время сохранения
    public Text SaveAuthor; //Компонент - автор текста
    public Text SaveText; //Компонент - текст
    public Button SaveButton; //Кнопка сохранения
    public Button DeleteButton; //Кнопка удаления
    public GameObject ConfirmScreen; //Экран подтверждения
    System.DateTime time; //Время сохранения
    string author; //автор текста
    string text; //Текст
    bool interacted = false; //Должны ли быть кнопки включены
    string action = ""; //Нужно действие для подтверждения
	void Start () 
    {
        if (SaveSlot.SelectedData != null) //Если выбран какой-то слот
            GettingData(); //Получаем данные
        SaveButton.interactable = interacted; //Включаем или выключаем кнопки по умолчанию
        DeleteButton.interactable = interacted;
	}
	
	void Update () 
    {
        if ((SaveSlot.SelectedData != null) && (time != SaveSlot.SelectedData.SaveTime)) //Если выбран какой-то слот и поменялось время сохранения
            GettingData(); //Получаем данные
        if ((SaveSlot.SelectedData != null) != interacted) //Если поменялась нужда во включении кнопок
        {
            interacted = SaveSlot.SelectedData != null; //Меняем переменную
            SaveButton.interactable = interacted; //Включаем или выключаем кнопки
            DeleteButton.interactable = interacted;
        }
	}

    void GettingData() //Получение данных
    {
        if (SaveSlot.SelectedData.SaveData != null) //Если данные существуют
        {
            time = SaveSlot.SelectedData.SaveTime; //Получаем время
            author = SaveSlot.SelectedData.SaveData.Author; //Получаем автора
            text = SaveSlot.SelectedData.SaveData.MainText; //Получаем текст
            SaveTime.text = time.ToString(); //Выводим время
            SaveAuthor.text = author; //Выводим автора
            SaveText.text = text; //Выводим текст
        }
        else //Иначе
        {
            time = System.DateTime.MinValue; //Устанавливаем минимальное время
            SaveTime.text = ""; //Удаляем вывод всех данных
            SaveAuthor.text = "";
            SaveText.text = "";
        }
    }

    public virtual void Save() //Функция сохранения
    {
        if (SaveSlot.SelectedData.SaveData != null) //Если данные уже есть в слоте
        {
            action = "save"; //Текущее действие - сохранение
            ShowConfirmScreen(); //вызываем экран подтверждения
            return; //конец
        }
        _save(); //сохраняем
    }

    public virtual void Delete() //Функция удаления
    {
        action = "delete"; //Текущее действие - удаление
        ShowConfirmScreen(); //Вызываем экран подтверждения
    }

    void _save() //Функция сохранения
    {
        Saves.Save(SaveSlot.SelectedData.SaveSlotPosition); //Сохраняем
    }

    void _delete() //Функция удаления
    {
        Saves.Delete(SaveSlot.SelectedData.SaveSlotPosition); //удаляем
    }

    public virtual void ShowConfirmScreen() //Вызов экрана подтверждения
    {
        ConfirmScreen.SetActive(true); //Делаем экран активным
    }

    public virtual void HideConfirmScreen(bool cont) //Скрытие экрана подтверждения
    {
        ConfirmScreen.SetActive(false); //Делаем экран неактивным
        if (cont) //Если действие было подтверждено
        {
            switch (action) //В зависимости от действия
            {
                case "save": //Если сохранение
                    _save(); //То сохраняем
                    break;
                case "delete": //Если удаление
                    _delete(); //То удаляем
                    break;
            }
        }
    }
}
