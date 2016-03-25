using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CGItem : MonoBehaviour {

    public string Title; //Название картинки
    public Image TargetGraphics; //Компонент Image
    bool Available; //Открыта ли картинка
    Button button; //Компонент Button
	void Start ()
    {
        button = GetComponent<Button>(); //Получаем компонент
        Init(); //Инициализируем
	}
	
	void Update ()
    {
        if (Available != CGGallery.Exists(Title)) //Если изменился факт открытости
            Init(); //Инициализируем
	}

    void Init() //Инициализация
    {
        Available = CGGallery.Exists(Title); //Получаем факт открытости
        if (Available) //Если картинка открыта
        {
            TargetGraphics.gameObject.SetActive(true); //Открываем компонент для картинки
            Texture2D tex = Resources.Load<Texture2D>(BackgroundManager.BackPath + Title); //Загружаем текстуру
            TargetGraphics.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2()); //Устанавливаем спрайт
            button.interactable = true; //Делаем кнопки нажимабельной
        }
        else //Иначе
        {
            TargetGraphics.gameObject.SetActive(false); //Закрываем компонент для картинки
            button.interactable = false; //Делаем кнопку ненажимабельной
        }
    }

    public virtual void View()
    {
        CGViewer.View(TargetGraphics.sprite);
    }
}
