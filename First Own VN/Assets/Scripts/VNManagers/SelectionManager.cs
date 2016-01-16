using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour {

    public GameObject SelectObject; //Объект с вариантами выбора
    public GameObject SelectPrefab; //Кнопка
    public float ButtonYSize; //Высота кнопки
    static public string NextTargert; //Следующий источник инструкций
    GameObject[] btns; //Массив кнопок
    string[] PotentialTargets; //Потеницальные источники инструкций
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void SetSelection(string[] texts, string[] targets) //Функция помещения выбора
    {
        if (texts.Length != targets.Length) //Если размер входных массивов не равен
            return; //Выходим из метода
        ScenarioManager.LockCoroutine(); //Приостанавливаем сценарий
        SelectObject.SetActive(true); //Делаем родительский объект активным
        btns = new GameObject[texts.Length]; //Инициализируем массив кнопок
        PotentialTargets = targets; //Сохраняем источники инструкций
        for (int i = 0; i < texts.Length; i++) //Для всех вариантов
        {
            btns[i] = Instantiate(SelectPrefab); //Помещаем кнопку на сцену
            btns[i].transform.SetParent(SelectObject.transform, false); //Устанавливаем родительский объект
            btns[i].GetComponentInChildren<Text>().text = texts[i]; //Меняем текст в кнопке
            RectTransform rt = btns[i].GetComponent<RectTransform>(); //Находим компонент RectTransform у кнопки
            rt.anchorMin = new Vector2(rt.anchorMin.x, 0.5f + (texts.Length - i - (texts.Length + 1) / 2 - 1) * ButtonYSize); //Устанавливаем  anchorMin
            rt.anchorMax = new Vector2(rt.anchorMax.x, 0.5f + (texts.Length - i - (texts.Length + 1) / 2) * ButtonYSize); //Устанавливаем anchorMax
            btns[i].GetComponent<SelectButtonData>().SelectionNum = i; //Устанавливаем кнопке номер выбора
        }
    }

    public void Select(int num) //Функция выбора
    {
        NextTargert = PotentialTargets[num]; //Следующим источником становится выбранный вариант
        for (int i = 0; i < btns.Length; i++)
            Destroy(btns[i]); //удаляем все кнопки
        SelectObject.SetActive(false); //Делаем родительский объект неактивным
        ScenarioManager.UnlockCoroutine(); //Возобновляем сценарий
    }
}
