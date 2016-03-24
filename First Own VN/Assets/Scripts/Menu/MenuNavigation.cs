using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour {

    public GameObject CurrentScreen;
    public float FadeTime = 0.25f;
    bool cdn = true;
    bool locked = false;
    Stack st;
	void Start ()
    {
        st = new Stack();
	}
	
	void Update ()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetMouseButtonDown(1)))
            GoBack();
    }

    public virtual void GoTo(GameObject obj)
    {
        if (Going(obj) == 0)
            st.Push(CurrentScreen);
    }

    int Going(GameObject obj)
    {
        if (locked)
            return -1;
        if (obj == null)
            return -2;
        StartCoroutine(goTo(obj));
        return 0;
    }

    public virtual void GoBack()
    {
        if (st.Count == 0)
            return;
        Going(st.Pop() as GameObject);
    }

    IEnumerator goTo(GameObject newObj) //Корутина перехода
    {
        locked = true; //Блокируем
        yield return null; //Новый кадр
        yield return null; //Новый кадр
        StartCoroutine(fade(CurrentScreen, false)); //Убираем текущий экран
        yield return StartCoroutine(WaitNext()); //Ждём
        CurrentScreen.SetActive(false); //Делаем текущий экран неактивным
        CurrentScreen = newObj; //Текущий экран теперь новый
        CurrentScreen.SetActive(true); //Делаем его активным
        StartCoroutine(fade(CurrentScreen, true)); //Показываем его
        yield return StartCoroutine(WaitNext()); //Ждём
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
            float delta = (2 * inc.GetHashCode() - 1) * Time.deltaTime / FadeTime; //Рассчитываем изменение на текущей итерации
            amount += delta; //Прибавляем его к значению
            AddAlpha(imgs, txts, delta); //Прибавляем его к альфе объектов
            yield return null; //Новый кадр
        }
        cdn = true; //Возобновляем проигрывание
    }

    void SetAlpha(Image[] imgs, Text[] txts, float value) //функция установки значения альфы
    {
        for (int i = 0; i < imgs.Length; i++)
            imgs[i].color = new Color(imgs[i].color.r, imgs[i].color.g, imgs[i].color.b, value); //Меняем альфу у всех объектов Image
        for (int i = 0; i < txts.Length; i++)
            txts[i].color = new Color(txts[i].color.r, txts[i].color.g, txts[i].color.b, value); //Меняем альфу у всех объектов Text
    }

    void AddAlpha(Image[] imgs, Text[] txts, float value) //Функция добавления к альфе
    {
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
