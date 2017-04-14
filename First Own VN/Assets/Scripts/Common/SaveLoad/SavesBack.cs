using UnityEngine;
using System.Collections;

public class SavesBack : MonoBehaviour {

    public float MoveTime = 0.5f; //Время перемещения
    float xpos = 0.5f; //Текущая позиция
    float firstpagepos = 0.5f; //Позиция первой страницы
    float lastpagepos = 5.5f; //Позиция последней страницы
    RectTransform rTrans; //Компонент RectTransform
    int Page = 0; //Текущий номер страницы
	void Start () 
    {
	    rTrans = GetComponent<RectTransform>(); //Находим компонент
	}
	
	void Update () 
    {
	
	}

    public virtual void ChangePage(int page) //Функция смены страницы
    {
        if (page == Page) //Если страница та же
            return; //То выход
        StartCoroutine(moving(firstpagepos + page, page)); //Начинаем корутину перемещения
    }

    IEnumerator moving(float targetpos, int newpage) //Корутина перемещения
    {
        bool right = targetpos > xpos; //Вправо ли нужно двигаться
        float oldpos = xpos; //сохраняем изначальную позицию
        while (right == (targetpos > xpos)) //Пока нужно двигаться в ту же сторону, что и с начала
        {
            float delta = (targetpos - oldpos) * Time.deltaTime / MoveTime; //рассчитываем длину шага
            xpos += delta; //прибавляем к текущей позиции
            if (right == (targetpos - xpos < delta))
                break;
            SetPosition(); //применяем изменения
            yield return null; //новый кадр
        }
        xpos = targetpos; //окончательно применяем позицию
        SetPosition(); //применяем изменения
        Page = newpage; //меняем значение страницы
    }

    void SetPosition() //Функция применения изменений
    {
        rTrans.anchorMin = new Vector2(firstpagepos - xpos, rTrans.anchorMin.y); //Устанавливаем anchorMin
        rTrans.anchorMax = new Vector2(lastpagepos - xpos, rTrans.anchorMax.y); //Устанавливаем anchorMax
    }
}
