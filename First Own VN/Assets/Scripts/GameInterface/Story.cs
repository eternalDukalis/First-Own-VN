using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Story : MonoBehaviour {

    public GameObject StoryForm; //Форма
    public float FadeTime; //Время работы с прозрачностью объектов
    public Text StoryContent; //Компонент Text, отображающий историю
    TextManager TManager; //Компонент для управления текстовой формой
    int TextLimit = 10000; //Предел текста
	void Start () 
    {
        TManager = GameObject.Find("TEXTMANAGER").GetComponent<TextManager>(); //Находим компонент на сцене
	}
	
	void Update () 
    {
        if ((StoryForm.activeSelf) && (Skip.isSkipping))
            Skip.SetMode(false);
        if (StoryContent.text.Length > TextLimit) //Если текст слишком большой
            DeleteTop(); //Удаляем верхний абзац
	}

    public void AddText(string s) //Функция добавления текста
    {
        StoryContent.text = StoryContent.text.Insert(StoryContent.text.LastIndexOf('\n'), s); //Добавление текста
    }

    void DeleteTop() //Функция удаления верхнего абзаца
    {
        StoryContent.text = StoryContent.text.Remove(0, StoryContent.text.IndexOf('\n') + 1); //Удаляем до первого символа перевода строки
    }

    public virtual void OpenStory() //Фунция открытия истории
    {
        StartCoroutine(opening()); //стартуем соответствуюшую корутину
    }

    public virtual void CloseStory() //Функция закрытия истории
    {
        StartCoroutine(closing()); //Стартуем соответствуюшую корутину
    }

    IEnumerator opening() //Корутина открытия истории
    {
        ScenarioManager.StopScenario(); //Останавливаем проигрывание сценария
        TManager.TakeOffTextForm(); //Убираем текстовую форму
        yield return StartCoroutine(WaitNext()); //Ждём окончания действия
        StoryForm.SetActive(true); //Делаем форму активной
        StartCoroutine(workWithForm(true)); //Показываем форму истории
        yield return StartCoroutine(WaitNext()); //Ждём окончания действия
    }

    IEnumerator closing() //Корутина закрытия истории
    {
        StartCoroutine(workWithForm(false)); //Скрываем форму истории
        yield return StartCoroutine(WaitNext()); //Ждём окончания действия
        StoryForm.SetActive(false); //Делаем форму неактивной
        TManager.ShowTextForm(); //Показываем текстовую форму
        yield return StartCoroutine(WaitNext()); //Ждём окончания действия
        ScenarioManager.StartScenario(); //Проигрываем сценарий
    }

    IEnumerator workWithForm(bool isShowing) //Корутина показывания/скрытия формы истории
    {
        ScenarioManager.LockCoroutine(); //Отмечаем приостанавливаение
        Image[] imgs = StoryForm.GetComponentsInChildren<Image>(); //Ищем все компоненты Image
        Text[] txts = StoryForm.GetComponentsInChildren<Text>(); //Ищем все компоненты Text
        float counter = (!isShowing).GetHashCode(); //Определяем счётчик
        while (((counter < 1) && (isShowing)) || ((counter > 0) && (!isShowing))) //Если нужно ещё работать с цветом
        {
            counter += (isShowing.GetHashCode() * 2 - 1) * Time.deltaTime / FadeTime; //Изменяем счётчик
            for (int i = 0; i < imgs.Length; i++)
                imgs[i].color += new Color(0, 0, 0, (isShowing.GetHashCode() * 2 - 1) * Time.deltaTime / FadeTime); //Работаем с цветом всех Image
            for (int i = 0; i < txts.Length; i++)
                txts[i].color += new Color(0, 0, 0, (isShowing.GetHashCode() * 2 - 1) * Time.deltaTime / FadeTime); //Работаем с цветом всех Text
            yield return null; //Новый кадр
        }
        ScenarioManager.UnlockCoroutine(); //Отмечаем возобновление
    }

    IEnumerator WaitNext() //Корутина ожидания возобновления корутины
    {
        while (!ScenarioManager.GetCanDoNext()) //Пока сверяемая булева переменная равна false
            yield return null; //Ждём
    }
}
