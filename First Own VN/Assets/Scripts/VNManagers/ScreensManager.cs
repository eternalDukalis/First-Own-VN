using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour {

    public GameObject ScreensObject; //Родительский объект для экранов
    public Image NewDayObject; //Компонент Image вставки "новый день"
    public Image OpeningObject; //Компонент Image псевдоопенинга
    public GameObject EpisodesObject; //Родительский объект для вставок начала эпизода
    public float FadeTime = 1; //Время появления/исчезновения
    public float MiniFadeTime = 0.5f; //Уменьшенное время появления/исчезновения
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

    public void Opening() //Функция запуска замены опенинга
    {
        ScreensObject.SetActive(true); //Делаем родительский объект активным
        StartCoroutine(opening()); //Начинаем корутину
    }

    public void NewEpisode(int epNum) //Функция запуска заставки начала эпизода
    {
        ScreensObject.SetActive(true); //Делаем родительский объект активным
        StartCoroutine(newEpisode(EpisodesObject.transform.GetChild(epNum - 1).GetComponent<Image>())); //Показываем заставку нужного эпизода
    }

    IEnumerator newEpisode(Image epParent) //Корутина начала эпизода
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем сценарий
        StartCoroutine(FadeObject(epParent, true, FadeTime)); //Показываем фон
        yield return StartCoroutine(WaitNext()); //Ждём
        Text[] txts = epParent.GetComponentsInChildren<Text>(); //Находим текстовые компоненты
        for (int i = 0; i < txts.Length; i++) //Каждый текстовый компонент
        {
            StartCoroutine(FadeObject(txts[i], true, MiniFadeTime)); //Выводим на экран
            yield return StartCoroutine(WaitNext()); //Ждём отдельный компонент
        }
        float tm = 0; //Счётчик паузы
        while (tm < PauseTime) //Пока время паузы не вышло
        {
            tm += Time.deltaTime; //Увеличиваем счётчик паузы
            yield return null; //Новый кадр
        }
        for (int i = 0; i < txts.Length; i++) //Каждый текстовый компонент
        {
            StartCoroutine(FadeObject(txts[i], false, FadeTime)); //Убираем со сцены
        }
        yield return StartCoroutine(WaitNext()); //Ждём все компоненты
        yield return null; //Новый кадр
        StartCoroutine(FadeObject(epParent, false, FadeTime)); //Убираем фон
        yield return StartCoroutine(WaitNext()); //Ждём
        ScenarioManager.UnlockCoroutine(); //Возобновляем сценарий
        ScreensObject.SetActive(false); //Делаем родительский объект неактивным
    }

    IEnumerator opening()
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем сценарий
        Image logo = OpeningObject.transform.GetChild(0).GetComponent<Image>(); //Находим лого
        Text title = OpeningObject.transform.GetChild(1).GetComponent<Text>(); //Нахоим название
        StartCoroutine(FadeObject(logo, true, FadeTime)); //Выводим на экран лого
        yield return StartCoroutine(WaitNext()); //Ждём
        StartCoroutine(FadeObject(title, true, FadeTime)); //Выводим на экран название
        yield return StartCoroutine(WaitNext()); //Ждём
        float tm = 0; //Счётчик паузы
        while (tm < PauseTime) //Пока время паузы не вышло
        {
            tm += Time.deltaTime; //Увеличиваем счётчик паузы
            yield return null; //Новый кадр
        }
        StartCoroutine(FadeObject(logo, false, MiniFadeTime)); //Убираем лого
        yield return StartCoroutine(WaitNext()); //Ждём
        StartCoroutine(FadeObject(title, false, MiniFadeTime)); //Убираем название
        yield return StartCoroutine(WaitNext()); //Ждём
        ScenarioManager.UnlockCoroutine(); //Возобновляем сценарий
        ScreensObject.SetActive(false); //Делаем родительский объект неактивным
    }

    IEnumerator newDay() //Корутина вставки "новый день"
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем сценарий
        StartCoroutine(FadeObject(NewDayObject, true, FadeTime)); //Показываем фон
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
        StartCoroutine(FadeObject(NewDayObject, false, FadeTime)); //Убираем фон
        yield return StartCoroutine(WaitNext()); //Ждём, пока фон полностью не уберётся
        ScenarioManager.UnlockCoroutine(); //Возобновляем сценарий
        ScreensObject.SetActive(false); //Делаем родительский объект неактивным
    }

    IEnumerator FadeObject(Image img, bool inc, float time) //Корутина работы с альфой компонентов Image
    {
        cdn = false; //Приостанавливаем локальную корутину
        while (((img.color.a < 1) && (inc)) || ((img.color.a > 0) && (!inc))) //Пока объект полностью не появится или полностью не исчезнет
        {
            img.color += new Color(0, 0, 0, (2 * inc.GetHashCode() - 1) * Time.deltaTime / time); //Увеличиваем или уменьшаем альфу
            yield return null; //Новый кадр
        }
        cdn = true; //Возобновляем локальную корутину
    }

    IEnumerator FadeObject(Text txt, bool inc, float time) //Корутина работы с альфой компоненов Text
    {
        cdn = false; //Приостанавливаем локальную корутину
        while (((txt.color.a < 1) && (inc)) || ((txt.color.a > 0) && (!inc))) //Пока текст полностью не появится или полностью не исчезнет
        {
            txt.color += new Color(0, 0, 0, (2 * inc.GetHashCode() - 1) * Time.deltaTime / time); //Увеличиваем или уменьшаем альфу
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
