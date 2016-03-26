using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewAch : MonoBehaviour {

    public float FlyingTime = 1; //Время полёта
    public float FadeTime = 0.5f; //Время работы с альфой
    public float PauseTime = 4; //Время паузы
    public Text AchText; //Компонент текста надписи "Новое достижение"
    public Text AchTitle; //Компонент текста названия
    public Text AchDesc; //Компонент текста описания
    bool cdn = true; //Можно двигаться дальше
    Vector2 center = new Vector2(0.3f, 0.7f); //Центр
	void Start ()
    {
        
	}
	
	void Update ()
    {
	
	}

    public void Init(string title, string description) //Инициализация
    {
        AchTitle.text = title; //Помещаем название
        AchDesc.text = description; //Помещаем описание
        StartCoroutine(ach()); //Запускаем корутину
    }

    IEnumerator ach() //Основная корутина
    {
        StartCoroutine(fly(center)); //Летим в центр
        yield return StartCoroutine(WaitNext()); //Ждём
        StartCoroutine(wait(PauseTime / 4)); //Пауза
        yield return StartCoroutine(WaitNext()); //Ждём
        StartCoroutine(fade(AchText, false)); //Убираем надпись "Новое достижение"
        yield return StartCoroutine(WaitNext()); //Ждём
        StartCoroutine(fade(AchTitle, true)); //Показываем название
        yield return StartCoroutine(WaitNext()); //Ждём
        StartCoroutine(fade(AchDesc, true)); //Показываем описание
        yield return StartCoroutine(WaitNext()); //Ждём
        StartCoroutine(wait(PauseTime)); //Пауза
        yield return StartCoroutine(WaitNext()); //Ждём
        StartCoroutine(fly(new Vector2(-1, 0))); //Летим за край экрана
        yield return StartCoroutine(WaitNext()); //Ждём
        Destroy(gameObject); //Разрушаем объект
    }

    IEnumerator wait(float time) //Корутина паузы
    {
        cdn = false; //Нельзя продолжать
        float tm = 0; //Счётчик времени
        while (tm < time) //Пока время не истечёт
        {
            tm += Time.deltaTime; //Увеличиваем счётчик
            yield return null; //Новый кадр
        }
        cdn = true; //Можно продолжать
    }

    IEnumerator fade(Text obj, bool on) //Кортуина работы с альфой
    {
        cdn = false; //Нельзя продолжать
        while (((on) && (obj.color.a < 1)) || ((!on) && (obj.color.a > 0))) //Пока не закончили
        {
            obj.color += new Color(0, 0, 0, (2 * on.GetHashCode() - 1) * Time.deltaTime / FadeTime); //Изменяем альфу
            yield return null; //Новый кадр
        }
        cdn = true; //Можно продолжать
    }

    IEnumerator fly(Vector2 target) //Корутина полёта
    {
        cdn = false; //Нельзя продолжать
        RectTransform rect = GetComponent<RectTransform>(); //Получаем RectTransform
        while (rect.anchorMax.x - target.y > 0) //Пока нужно двигаться
        {
            rect.anchorMin -= new Vector2(Time.deltaTime / FlyingTime, 0); //Изменяем anchorMin
            rect.anchorMax -= new Vector2(Time.deltaTime / FlyingTime, 0); //Изменяем anchorMax
            yield return null;//Новый кадр
        }
        cdn = true; //Можно продолжать
    }

    IEnumerator WaitNext() //Корутина ожидания
    {
        while (!cdn) //Пока нельзя продолжать
            yield return null; //Новый кадр
    }
}
