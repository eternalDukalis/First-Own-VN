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
    public bool isLoading = false; //Загрузка ли
    System.DateTime time; //Время сохранения
    string author; //автор текста
    string text; //Текст
    bool interacted = false; //Должны ли быть кнопки включены
    string action = ""; //Нужно действие для подтверждения
    SaveSlot selData
    {
        get
        {
            if (isLoading)
                return SaveSlot.SelectedLoadData;
            return SaveSlot.SelectedData;
        }
        set
        {
            if (isLoading)
                SaveSlot.SelectedLoadData = value;
            else
                SaveSlot.SelectedData = value;
        }
    }
	void Start () 
    {
        if (selData != null) //Если выбран какой-то слот
            GettingData(); //Получаем данные
        SaveButton.interactable = interacted; //Включаем или выключаем кнопки по умолчанию
        DeleteButton.interactable = interacted;
	}
	
	void Update () 
    {
        if ((selData == null) || (time != selData.SaveTime)) //Если выбран какой-то слот и поменялось время сохранения
            GettingData(); //Получаем данные
        if ((selData != null) != interacted) //Если поменялась нужда во включении кнопок
        {
            interacted = (((!isLoading) && (selData != null)) || ((isLoading) && (selData != null) && (selData.SaveData != null))); //Меняем переменную
            SaveButton.interactable = interacted; //Включаем или выключаем кнопки
            DeleteButton.interactable = interacted;
        }
	}

    void GettingData() //Получение данных
    {
        if ((selData != null) && (selData.SaveData != null)) //Если данные существуют
        {
            time = selData.SaveTime; //Получаем время
            author = selData.SaveData.Author; //Получаем автора
            text = selData.SaveData.MainText; //Получаем текст
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
        if (selData.SaveData != null) //Если данные уже есть в слоте
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

    public virtual void Load() //Функция загрузки
    {
        Saves.Load(selData.SaveSlotPosition); //Загружаем
    }

    void _save() //Функция сохранения
    {
        Saves.Save(selData.SaveSlotPosition); //Сохраняем
    }

    void _delete() //Функция удаления
    {
        Saves.Delete(selData.SaveSlotPosition); //удаляем
        //interacted = false; //Выключаем кнопки
        selData = null; //Теперь ничего не выделено
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
