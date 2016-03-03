using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FitText : MonoBehaviour {

    public float SizeKoef; //Коэффициент размера шрифта
    int height; //Высота экрана
	void Start () 
    {
        height = Screen.height; //Берём значение из настроек
        SetSize(); //Вызываем метод применения нового размера
	}
	
	void Update () 
    {
        if (height != Screen.height) //Если значение не совпадает
        {
            SetSize(); //Пересчитываем размер шрифта
        }
	}

    public void SetSize() //Функция применения нового размера шрифта
    {
        height = Screen.height; //Берём значение из настроек
        GetComponent<Text>().fontSize = (int)(height * SizeKoef); //Получаем новый размер шрифта, умножая высоту экрана на коэффициент
    }
}
