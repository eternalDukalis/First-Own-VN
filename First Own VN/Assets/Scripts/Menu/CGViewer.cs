using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CGViewer : MonoBehaviour {

    static Sprite SpriteToView; //Спрайт, который нужно показать
    static Image Viewer; //Компонент Image
	void Start ()
    {
        Viewer = GetComponent<Image>(); //Получаем компонент
	}
	
	void Update ()
    {
        if ((Viewer != null) && (SpriteToView != Viewer.sprite)) //Если компонент получен и спрайт для показывания изменился
            Viewer.sprite = SpriteToView; //То меняем спрайт
	}

    static public void View(Sprite spriteToView) //Функция показывания
    {
        SpriteToView = spriteToView; //Записываем спрайт
    }
}
