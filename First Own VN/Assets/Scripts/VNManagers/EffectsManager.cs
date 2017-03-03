using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EffectsManager : MonoBehaviour {

    public GameObject PlainObject; //Объект с эффектами
    public RectTransform InterfaceObject; //Родительский объект для интерфейса
    public float TimeToFade = 1; //Время появления/исчезновения одноцветного экрана
    public float JoltAmplitude = 0.1f; //Амплитуда тряски
    public int ImpulseSteps = 8; //Время одного толчка
    public float ImpulseCount = 8; //Количество толчков
    Vector2 TargetDeviation; //Целевое отклонение
    Vector2 Deviation; //Текущее отклонение
	void Start () 
    {
        if (State.CurrentState.PlainScreenOn) //Если включён одноцветный экран
        {
            QuickShow(PlainObject.GetComponent<Image>(), State.CurrentState.PlainScreenColor); //Показываем одноцветный экран
        }
	}
	
	void Update () 
    {
	
	}

    public void PlainScreenOn(string color) //Функция включения одноцветного экрана
    {
        State.CurrentState.PlainScreenOn = true; //Записываем состояние
        State.CurrentState.PlainScreenColor = color; //Записываем цвет
        StartCoroutine(WorkingWithPlainScreen(true)); //Запускаем корутину появления
    }

    public void PlainScreenOff() //Функция выключения одноцветного экрана
    {
        State.CurrentState.PlainScreenOn = false; //Записываем состояние
        StartCoroutine(WorkingWithPlainScreen(false)); //Запускаем корутину исчезания
    }

    public void Jolt() //Функция тряски
    {
        StartCoroutine(Jolting()); //Запускаем корутину
    }

    IEnumerator WorkingWithPlainScreen(bool inc) //Корутина работы с одноцветным экраном
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем сценарий
        if (inc) //Если экран появляется
            PlainObject.SetActive(true); //То делаяем объект активным
        Image img = PlainObject.GetComponent<Image>(); //Находим компонент Image
        float val = 0;
        Color begin = img.color;
        Color end = StringToColor(State.CurrentState.PlainScreenColor);
        end = new Color(end.r, end.g, end.b, inc.GetHashCode());
        while (val < 1)
        {
            if ((ControlManager.Next()) || (Skip.isSkipping)) //Если нажата кнопка продолжения
            {
                yield return new WaitForSeconds(Settings.SkipInterval); //Новый кадр
                img.color = end; //Окончательно показываем или убираем экран
                break; //Прерываем цикл
            }
            val += Time.deltaTime / TimeToFade;
            img.color = begin + (end - begin) * val;
            yield return null;
        }
        img.color = end;
        /*if (inc) //Если экран должен появиться
        {
            img.color = StringToColor(State.CurrentState.PlainScreenColor); //Красим экран
            img.color -= new Color(0, 0, 0, 1); //То делаем его прозрачным
        }
        while (((inc) && (img.color.a < 1)) || ((!inc) && (img.color.a > 0))) //Пока мы не убрали или показали полностью экран
        {
            if ((ControlManager.Next()) || (Skip.isSkipping)) //Если нажата кнопка продолжения
            {
                yield return new WaitForSeconds(Settings.SkipInterval); //Новый кадр
                img.color = new Color(img.color.r, img.color.g, img.color.b, inc.GetHashCode()); //Окончательно показываем или убираем экран
                break; //Прерываем цикл
            }
            img.color += new Color(0, 0, 0, (2 * inc.GetHashCode() - 1) * Time.deltaTime / TimeToFade); //Добавляем/уменьшаем альфу
            yield return null; //Переход на другой кадр
        }*/
        yield return null; //Переход на новый кадр
        if (!inc) //Если экран убирался
            PlainObject.SetActive(false); //То делаем объект неактивным
        ScenarioManager.UnlockCoroutine(); //Возобновляем сценарий
    }

    IEnumerator Jolting() //Корутина тряски
    {
        for (int i = 0; i < ImpulseCount; i++) //Выполняем определённое количество толчков
        {
            TargetDeviation = CalcaulateDeviation(); //Рассчитываем целевое отклонение
            Vector2 dist = TargetDeviation - Deviation; //Разница между целевым отклонением и текущим
            for (int j = 0; j < ImpulseSteps; j++) //Выполняем определённое количество шагов
            {
                if ((ControlManager.Next()) || (Skip.isSkipping)) //Если нажата кнопка продолжения
                {
                    j = ImpulseSteps; //Прерываем цикл
                    break; //Прерываем цикл
                }
                Deviation += dist / ImpulseSteps; //Изменяем текущее отклонение
                InterfaceObject.anchorMin = Deviation; //Изменяем расположение интерфейса
                InterfaceObject.anchorMax = new Vector2(1, 1) + Deviation;
                yield return null; //Новый кадр
            }
        }
        Deviation = new Vector2(0, 0); //Обнуляем отклонение
        InterfaceObject.anchorMin = Deviation; //Ставим интерфейс на место
        InterfaceObject.anchorMax = new Vector2(1, 1) + Deviation;
    }

    Vector2 CalcaulateDeviation() //Расчёт отклонения
    {
        float x = Random.Range(-JoltAmplitude, JoltAmplitude); //Координата x
        float y = Random.Range(-JoltAmplitude, JoltAmplitude); //Координата y
        return new Vector2(x, y); //Возвращаем результат
    }

    public static Color StringToColor(string source) //Функция преобразования строки в цвет
    {
        switch (source)
        {
            case "white":
                return Color.white;
            case "red":
                return Color.red;
            case "blue":
                return Color.blue;
            case "clear":
                return Color.clear;
            case "cyan":
                return Color.cyan;
            case "gray":
                return Color.gray;
            case "green":
                return Color.green;
            case "grey":
                return Color.grey;
            case "magenta":
                return Color.magenta;
            case "yellow":
                return Color.yellow;
        }
        return Color.black;
    }

    void QuickShow(Image img, string color) //Функция быстрого показа одноцветного экрана
    {
        img.gameObject.SetActive(true); //Делаем объект активным
        img.color = StringToColor(color); //Применяем цвет
    }
}
