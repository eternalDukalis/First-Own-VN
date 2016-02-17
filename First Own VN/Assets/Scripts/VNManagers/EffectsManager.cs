using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EffectsManager : MonoBehaviour {

    public GameObject PlainObject; //Объект с эффектами
    public float TimeToFade = 1; //Время появления/исчезновения одноцветного экрана
	void Start () 
    {
	
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

    public void PlainScreenOff()
    {
        State.CurrentState.PlainScreenOn = false; //Записываем состояние
        StartCoroutine(WorkingWithPlainScreen(false)); //Запускаем корутину исчезания
    }

    IEnumerator WorkingWithPlainScreen(bool inc) //Корутина работы с одноцветным экраном
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем сценарий
        if (inc) //Если экран появляется
            PlainObject.SetActive(true); //То делаяем объект активным
        Image img = PlainObject.GetComponent<Image>(); //Находим компонент Image
        if (inc) //Если экран должен появиться
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
        }
        yield return null; //Переход на новый кадр
        if (!inc) //Если экран убирался
            PlainObject.SetActive(false); //То делаем объект неактивным
        ScenarioManager.UnlockCoroutine(); //Возобновляем сценарий
    }

    Color StringToColor(string source) //Функция преобразования строки в цвет
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
}
