using UnityEngine;
using System.Collections;

public class SelectButtonData : MonoBehaviour {

    public int SelectionNum; //Номер выбора
    SelectionManager SManager; //Менеджер выборов
	void Start () 
    {
        SManager = GameObject.Find("CommonObject").GetComponent<SelectionManager>(); //Находим компонент на сцене
	}
	
	void Update () 
    {
	
	}

    public virtual void Select() //Функция выбора данного варианта
    {
        SManager.Select(SelectionNum); //Вызываем функцию выбора
    }
}