using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FitText : MonoBehaviour {

    public float SizeKoef; //Коэффициент размера шрифта
	void Start () 
    {
        SetSize(); //Вызываем метод применения нового размера
	}
	
	void Update () 
    {
	
	}

    public void SetSize() //Функция применения нового размера шрифта
    {
        GetComponent<Text>().fontSize = (int)(Screen.height * SizeKoef); //Получаем новый размер шрифта, умножая высоту экрана на коэффициент
    }
}
