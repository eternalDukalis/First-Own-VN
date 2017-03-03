using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour {

    static public string BackPath = "Graphics/Backgrounds/"; //Директория с задними фонами
    static public float BackgroundChangeTime = 1; //Стандартное время смены заднего фона
    Image img; //Компонент Image текущего объекта
	void Start () 
    {
        img = GetComponent<Image>(); //Поиск компонента
    }
	
	void Update () 
    {
	
	}

    public void ChangeBackground(string title) //Стандартная функция смены заднего фона
    {
        ChangeBackground(title, BackgroundChangeTime); //Смена фона с передачей стандартных параметров
    }

    public void ChangeBackground(string title, float time) //Функция смены заднего фона с дополнительным параметров времени
    {
        State.CurrentState.Background = title; //Записываем текущий фон
        Texture2D target = Resources.Load<Texture2D>(BackPath + title); //Загружаем текстуру
        StartCoroutine(cBackground(target, time)); //Запускаем корутину смены фона
    }

    IEnumerator cBackground(Texture2D newtext, float time) //Корутина смены фона
    {
        ScenarioManager.LockCoroutine(); //Приостанавливаем основную сценарную корутину
        GameObject OldBackground = GameObject.Instantiate(this.gameObject); //Создаём новый объект со старым фоном
        OldBackground.transform.SetParent(this.transform, false); //Родительским объектом назначаем текущий объект
        Image oldimg = OldBackground.GetComponent<Image>(); //Компонент Image старого фона
        img.sprite = Sprite.Create(newtext, new Rect(0, 0, newtext.width, newtext.height), new Vector2(0, 0)); //Сменяем спрайт на новый
        yield return null;
        while (oldimg.color.a > 0) //Пока старый фон не станет прозрачным
        {
            if ((ControlManager.Next()) || (Skip.isSkipping)) //Если нажата кнопка продолжения
            {
                yield return new WaitForSeconds(Settings.SkipInterval); //Новый кадр
                DeleteObject(OldBackground); //Удаляем старый фон
                break; //Прерываем цикл
            }
            oldimg.color -= new Color(0, 0, 0, Time.deltaTime / time); //Уменьшаем непрозрачность старого фона
            yield return null; //Переход на новый кадр
        }
        DeleteObject(OldBackground); //Удаляем старый фон
        ScenarioManager.UnlockCoroutine(); //Возобновляем основную сценарную корутину
    }

    void DeleteObject(GameObject obj) //Функция удаления объекта
    {
        Destroy(obj.gameObject); //Удаляем объект
        Resources.UnloadUnusedAssets(); //Освобождаем неиспользуемые ресурсы
    }
}
