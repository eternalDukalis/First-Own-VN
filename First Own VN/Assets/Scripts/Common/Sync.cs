using UnityEngine;
using System.Collections;

public class Sync : MonoBehaviour {

    string[] DataSeparator = { "\n\n[nextdatablock]\n\n" }; //Разделитель данных
	void Start ()
    {
        
    }

	void Update ()
    {
       
    }

    string Data()
    {
        string res = Saves.GetSavesListData() + DataSeparator[0]; //Получаем данные о ключах сохранений
        res += Saves.GetSavesData() + DataSeparator[0]; //Получаем данные о сохранениях
        res += Settings.GetData() + DataSeparator[0]; //Получаем данные и настройках
        res += Skip.GetData() + DataSeparator[0]; //Получаем данные о пройденных участках
        res += Achievments.GetData() + DataSeparator[0]; //Получаем данные о достижениях
        res += CGGallery.GetData() + DataSeparator[0]; //Получаем данные о графической галерее
        res += MusicGallery.GetData(); //Получаем данные о музыкальной галерее
        return res; //Возвращаем результат
    }

    void ApplyData(string s) //Функция установки данных игры
    {
        PlayerPrefs.DeleteAll(); //Удаляем все сохранённые данные
        string[] blocks = s.Split(DataSeparator, System.StringSplitOptions.None); //Разделяем блоки данных
        Saves.SetSavesData(blocks[0], blocks[1]); //Устанавливаем данные о сохранениях
        Settings.SetData(blocks[2]); //Устанавливаем данные о настройках
        Skip.SetData(blocks[3]); //Устанавливаем данные о пройденных участках
        Achievments.SetData(blocks[4]); //Устанавливаем данные о достижениях
        CGGallery.SetData(blocks[5]); //Устанавливаем данные о графической галерее
        MusicGallery.SetData(blocks[6]); //Устанавливаем данные о музыкальной галерее
    }
}
