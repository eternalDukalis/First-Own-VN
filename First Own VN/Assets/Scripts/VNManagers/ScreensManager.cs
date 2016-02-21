using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour {

    public GameObject ScreensObject; //Родительский объект для экранов
    public Image NewDayObject; //Компонент Image вставки "новый день"
    public float FadeTime = 1; //Время появления/исчезновения
    public float FillingTime = 2; //Время выезда
    public float PauseTime = 1; //Время паузы
    bool cdn = true; //Сверяемая булева переменная
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void NewDay(string background) //Функция запуска вставки "новый день"
    {
        Texture2D bck = Resources.Load<Texture2D>(BackgroundManager.BackPath + background); //Загружаем текстуру
        ScreensObject.SetActive(true); //Делаем родительский объект активным
        NewDayObject.sprite = Sprite.Create(bck, new Rect(0, 0, bck.width, bck.height), new Vector2(0, 0)); //Вставляем текстуру в компонент
        StartCoroutine(newDay()); //Начинаем корутину
    }

    IEnumerator newDay() //Корутина вставки "новый день"
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем сценарий
        StartCoroutine(FadeObject(NewDayObject, true)); //Показываем фон
        yield return StartCoroutine(WaitNext()); //Ждём, пока фон полностью покажется
        Image logo = NewDayObject.transform.GetChild(0).GetComponent<Image>(); //Находим компонент Image логотипа
        logo.fillOrigin = 1; //Появление снизу вверх
        while (logo.fillAmount < 1) //Пока логотипа полностью не появится
        {
            logo.fillAmount += Time.deltaTime / FadeTime; //Увеличить заполнение логотипа
            yield return null; //Новый кадр
        }
        logo.fillOrigin = 0; //Скрытие снизу вверх
        float tm = 0; //Текущее время паузы
        while (tm < PauseTime) //Пока время паузы не истекло
        {
            tm += Time.deltaTime; //Увеличиваем счётчик паузы
            yield return null; //Новый кадр
        }
        while (logo.fillAmount > 0) //Пока логотип полностью не скроется
        {
            logo.fillAmount -= Time.deltaTime / FadeTime; //Уменьшаем заполнение логотипа
            yield return null; //Новый кадр
        }
        StartCoroutine(FadeObject(NewDayObject, false)); //Убираем фон
        yield return StartCoroutine(WaitNext()); //Ждём, пока фон полностью не уберётся
        ScenarioManager.UnlockCoroutine(); //Возобновляем сценарий
        ScreensObject.SetActive(false); //Делаем родительский объект неактивным
    }

    IEnumerator FadeObject(Image img, bool inc) //Корутина работы с альфой компонентов Image
    {
        cdn = false; //Приостанавливаем локальную корутину
        while (((img.color.a < 1) && (inc)) || ((img.color.a > 0) && (!inc))) //Пока объект полностью не появится или полностью не исчезнет
        {
            img.color += new Color(0, 0, 0, (2 * inc.GetHashCode() - 1) * Time.deltaTime / FadeTime); //Увеличиваем или уменьшаем альфу
            yield return null; //Новый кадр
        }
        cdn = true; //Возобновляем локальную корутину
    }

    IEnumerator WaitNext() //Корутина ожидания возобновления корутин
    {
        while (!cdn) //Пока сверяемая булева переменная равна false
            yield return null; //Ждём
    }
}
