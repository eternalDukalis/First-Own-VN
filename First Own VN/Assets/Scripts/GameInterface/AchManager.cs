using UnityEngine;
using System.Collections;

public class AchManager : MonoBehaviour {

    public GameObject AchObject; //Объект отображения достижения
    public GameObject Parent; //Родительский объект
	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void Push(string title, string description) //Функция пуша
    {
        if (!Achievments.Push(title, description)) //Если успешно не получено новое достижение
            return; //То выход
        GameObject obj = Instantiate(AchObject); //Добавляем объект на сцену
        obj.transform.SetParent(Parent.transform, false); //Помещаем в родительский объект
        obj.transform.SetAsLastSibling(); //Помещаем сверху
        obj.GetComponent<NewAch>().Init(title, description); //Запускаем его
    }
}
