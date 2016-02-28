using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Navigation : MonoBehaviour {

    public GameObject CurrentScreen; //Текущий экран
    public GameObject PreviousScreen; //Предыдущий экран
    public GameObject PlayingInterface; //Экран игрового режима
    public float FadeTime = 1; //Время появления/исчезновения
    bool cdn = true; //Сверяемая переменная
	void Start () 
    {
	
	}
	
	void Update () 
    {
	    
	}

    public virtual void GoTo(GameObject newObject) //Переход на другой экран
    {
        if (newObject == null) //Если нового объекта нет
            return; //То прерываем
        if (CurrentScreen == PlayingInterface) //Если текущий экран - игровой
            ScenarioManager.PlayingMode = false; //То больше не в режиме проигрывания
        StartCoroutine(goTo(newObject)); //Запускаем корутину перехода
    }

    IEnumerator goTo(GameObject newObj) //Корутина перехода
    {
        PreviousScreen = CurrentScreen; //Обновляем предыдущий экран
        StartCoroutine(fade(CurrentScreen, false)); //Убираем текущий экран
        yield return StartCoroutine(WaitNext()); //Ждём
        CurrentScreen.SetActive(false); //Делаем текущий экран неактивным
        CurrentScreen = newObj; //Текущий экран теперь новый
        CurrentScreen.SetActive(true); //Делаем его активным
        StartCoroutine(fade(CurrentScreen, true)); //Показываем его
        yield return StartCoroutine(WaitNext()); //Ждём
        if (CurrentScreen == PlayingInterface) //Если текущий экран - игровой
            ScenarioManager.PlayingMode = true; //То переходим в режим проигрывания
    }

    IEnumerator fade(GameObject obj, bool inc) //Корутина изменения прозрачности
    {
        cdn = false; //Приостанавливаем проигрывание
        float amount = (!inc).GetHashCode(); //Получаем текущее значение
        while (((inc) && (amount < 1)) || ((!inc) && (amount > 0))) //Пока не достигли нужного значениея
        {
            float delta = (2 * inc.GetHashCode() - 1) * Time.deltaTime / FadeTime; //Рассчитываем изменение на текущей итерации
            amount += delta; //Прибавляем его к значению
            AddAlpha(obj, delta); //Прибавляем его к альфе объектов
            yield return null; //Новый кадр
        }
        cdn = true; //Возобновляем проигрывание
    }

    void AddAlpha(GameObject obj, float value) //Функция добавления к альфе
    {
        Image[] imgs = obj.GetComponentsInChildren<Image>(); //Находим компоненты Image
        Text[] txts = obj.GetComponentsInChildren<Text>(); //Находим компоненты Text
        for (int i = 0; i < imgs.Length; i++) 
            imgs[i].color += new Color(0, 0, 0, value); //Меняем альфу у всех объектов Image
        for (int i = 0; i < txts.Length; i++)
            txts[i].color += new Color(0, 0, 0, value); //Меняем альфу у всех объектов Text
    }

    IEnumerator WaitNext() //Корутина ожидания
    {
        while (!cdn) //Пока нельзя проигрывать
            yield return null; //Новый кадр
    }
}
