using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Navigation : MonoBehaviour {

    public GameObject CurrentScreen; //Текущий экран
    public GameObject PreviousScreen; //Предыдущий экран
    public GameObject PlayingInterface; //Экран игрового режима
    public GameObject MenuObject; //Объект меню
    public float FadeTime = 1; //Время появления/исчезновения
    bool cdn = true; //Сверяемая переменная
    bool fast = false; //Нужно ли быстро закончить
    bool locked = false; //Корутина заблокирована
	void Start () 
    {
	
	}
	
	void Update () 
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetMouseButtonDown(1)))
            GoBack();
	}

    public virtual void GoTo(GameObject newObject) //Переход на другой экран
    {
        if (locked) //Если корутина заблокирована
            return; //То прерываем
        if (newObject == null) //Если нового объекта нет
            return; //То прерываем
        if (CurrentScreen == PlayingInterface) //Если текущий экран - игровой
            ScenarioManager.PlayingMode = false; //То больше не в режиме проигрывания
        fast = true; //Нужно быстро закончить текущую корутину
        StartCoroutine(goTo(newObject)); //Запускаем корутину перехода
    }

    public virtual void GoBack() //Переход на предыдущий экран
    {
        if (CurrentScreen == PlayingInterface) //Если текущий экран - игровой
            GoTo(MenuObject); //То переходим на меню
        else //Иначе
        {
            if (CurrentScreen == MenuObject) //Если текущий экран - меню
                GoTo(PlayingInterface); //То переходим на игровой экран
            else //Иначе
                GoTo(PreviousScreen); //Перехождим на предыдущий экран
        }
    }

    IEnumerator goTo(GameObject newObj) //Корутина перехода
    {
        locked = true; //Блокируем
        yield return null; //Новый кадр
        yield return null; //Новый кадр
        GameObject prs = CurrentScreen; //Сохраняем текущий экран
        fast = false; //Не нужно быстро заканчивать
        StartCoroutine(fade(CurrentScreen, false)); //Убираем текущий экран
        yield return StartCoroutine(WaitNext()); //Ждём
        CurrentScreen.SetActive(false); //Делаем текущий экран неактивным
        CurrentScreen = newObj; //Текущий экран теперь новый
        CurrentScreen.SetActive(true); //Делаем его активным
        StartCoroutine(fade(CurrentScreen, true)); //Показываем его
        yield return StartCoroutine(WaitNext()); //Ждём
        PreviousScreen = prs; //Обновляем предыдущий экран
        if (CurrentScreen == PlayingInterface) //Если текущий экран - игровой
            ScenarioManager.PlayingMode = true; //То переходим в режим проигрывания
        locked = false; //Разблокируем
    }

    IEnumerator fade(GameObject obj, bool inc) //Корутина изменения прозрачности
    {
        cdn = false; //Приостанавливаем проигрывание
        float amount = (!inc).GetHashCode(); //Получаем текущее значение
        Image[] imgs = obj.GetComponentsInChildren<Image>(); //Находим компоненты Image
        Text[] txts = obj.GetComponentsInChildren<Text>(); //Находим компоненты Text
        SetAlpha(imgs, txts, (!inc).GetHashCode()); //Устанавливаем изначальное значение альфы
        while (((inc) && (amount < 1)) || ((!inc) && (amount > 0))) //Пока не достигли нужного значениея
        {
            if (fast) //Если нужно быстро закончить
            {
                SetAlpha(imgs, txts, inc.GetHashCode()); //Устанавливаем конечно значение
                break;//Выходим из цикла
            }
            float delta = (2 * inc.GetHashCode() - 1) * Time.deltaTime / FadeTime; //Рассчитываем изменение на текущей итерации
            amount += delta; //Прибавляем его к значению
            AddAlpha(imgs, txts, delta); //Прибавляем его к альфе объектов
            yield return null; //Новый кадр
        }
        cdn = true; //Возобновляем проигрывание
    }

    void AddAlpha(Image[] imgs, Text[] txts, float value) //Функция добавления к альфе
    {
        for (int i = 0; i < imgs.Length; i++) 
            imgs[i].color += new Color(0, 0, 0, value); //Меняем альфу у всех объектов Image
        for (int i = 0; i < txts.Length; i++)
            txts[i].color += new Color(0, 0, 0, value); //Меняем альфу у всех объектов Text
    }

    void SetAlpha(Image[] imgs, Text[] txts, float value) //функция установки значения альфы
    {
        for (int i = 0; i < imgs.Length; i++)
            imgs[i].color = new Color(imgs[i].color.r, imgs[i].color.g, imgs[i].color.b, value); //Меняем альфу у всех объектов Image
        for (int i = 0; i < txts.Length; i++)
            txts[i].color = new Color(txts[i].color.r, txts[i].color.g, txts[i].color.b, value); //Меняем альфу у всех объектов Text
    }

    IEnumerator WaitNext() //Корутина ожидания
    {
        while (!cdn) //Пока нельзя проигрывать
            yield return null; //Новый кадр
    }
}
