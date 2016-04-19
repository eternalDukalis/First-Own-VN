using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sync : MonoBehaviour {

    public InputField RegName;
    public InputField RegPass;
    public InputField RegCheckPass;
    public InputField AuthName;
    public InputField AuthPass;
    public Text Message;
    public GameObject[] AuthObj;
    public GameObject[] UnauthObj;
    public GameObject RegScreen;
    public GameObject AuthScreen;
    GameObject MessageScreen;
    string[] DataSeparator = { "\n\n[nextdatablock]\n\n" }; //Разделитель данных
    string Login = ""; //Логин
    string Password = ""; //Пароль
    static public bool Authorized = false; //Авторизован ли
    bool _auth = false;
    string Key = "SyncData"; //Ключ для сохранения
    string[] Separator = { "\n[syncsep]\n" }; //Разделитель
    static string RegistrationUrl = "http://dukalisgds.xyz/registration.php";
    static string AuthFormat = "http://dukalisgds.xyz/authorization.php?username={0}&password={1}";
    static string DownloadFormat = "http://dukalisgds.xyz/load.php?username={0}&password={1}";
    static string UploadUrl = "http://dukalisgds.xyz/upload.php";
    static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
    static int MinLength = 5;
    static int MaxLength = 20;
    float WaitingTime = 5;
    bool locked = false;
    void Start ()
    {
        MessageScreen = Message.transform.parent.parent.gameObject;
        Load(); //Загружаем данные
        UpdateActivity();
    }

	void Update ()
    {
        if (_auth != Authorized)
        {
            _auth = Authorized;
            UpdateActivity();
        }
    }

    string Data() //Функция получения данных об игре
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

    public virtual void Register()
    {
        if (locked)
            return;
        if (!InAlpabet(RegName.text))
        {
            PushMessage("Некорректное имя пользователя. Допускаются только латинские буквы и цифры.");
            return;
        }
        if (!InAlpabet(RegPass.text))
        {
            PushMessage("Некорректный пароль. Допускаются только латинские буквы и цифры.");
            return;
        }
        if ((RegName.text.Length > MaxLength) || (RegName.text.Length < MinLength))
        {
            PushMessage("Некорректное имя пользователя. Длина должна составлять 5-20 символов.");
            return;
        }
        if ((RegPass.text.Length > MaxLength) || (RegPass.text.Length < MinLength))
        {
            PushMessage("Некорректный пароль. Длина должна составлять 5-20 символов.");
            return;
        }
        if (RegPass.text != RegCheckPass.text)
        {
            PushMessage("Пароли не совпадают.");
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("username", RegName.text);
        form.AddField("password", RegPass.text);
        form.AddField("data", Data());
        StartCoroutine(reg(form));
    }

    public virtual void Authorize()
    {
        if (locked)
            return;
        StartCoroutine(auth());
    }

    public virtual void LogOut()
    {
        if (locked)
            return;
        Login = "";
        Password = "";
        Authorized = false;
        Save();
    }

    public virtual void Download()
    {
        if (locked)
            return;
        StartCoroutine(download(string.Format(DownloadFormat, Login, Password)));
    }

    public virtual void Upload()
    {
        if (locked)
            return;
        WWWForm form = new WWWForm();
        form.AddField("username", Login);
        form.AddField("password", Password);
        form.AddField("data", Data());
        StartCoroutine(upload(form));
    }

    void Save() //Функция сохранения данных
    {
        string res = Login + Separator[0] + Password + Separator[0] + Authorized.ToString(); //Формируем строку
        PlayerPrefs.SetString(Key, res); //Сохраняем строку по ключу
    }

    void Load() //Функция загрузки данных
    {
        if (PlayerPrefs.HasKey(Key)) //Если есть данные по ключу
        {
            string[] data = PlayerPrefs.GetString(Key).Split(Separator, System.StringSplitOptions.None); //Получаем данные по ключу
            Login = data[0]; //Логин
            Password = data[1]; //Пароль
            Authorized = bool.Parse(data[2]); //Авторизован ли
        }
    }

    IEnumerator reg(WWWForm form)
    {
        WWW www = new WWW(RegistrationUrl, form);
        locked = true;
        float tm = 0;
        while (!www.isDone)
        {
            tm += Time.deltaTime;
            if (tm > WaitingTime)
                break;
            yield return null;
        }
        locked = false;
        if ((!www.isDone) || (www.text == ""))
        {
            if (!www.isDone)
                PushMessage("Истекло время ожидания.");
            else
                PushMessage("Отсутсвует подключение к интернету.");
        }
        else
        {
            switch (www.text[0])
            {
                case '0':
                    PushMessage("Ваш аккаунт успешно создан!");
                    RegScreen.SetActive(false);
                    break;
                case '1':
                    PushMessage("Пользователь с таким именем уже существует.");
                    break;
                default:
                    PushMessage("Неизвестная ошибка.");
                    break;
            }
        }
    }

    IEnumerator auth()
    {
        string login = AuthName.text;
        string pass = AuthPass.text;
        string s = string.Format(AuthFormat, login, pass);
        WWW www = new WWW(s);
        float tm = 0;
        while (!www.isDone)
        {
            tm += Time.deltaTime;
            if (tm > WaitingTime)
                break;
            yield return null;
        }
        locked = false;
        if ((!www.isDone) || (www.text == ""))
        {
            if (!www.isDone)
                PushMessage("Истекло время ожидания.");
            else
                PushMessage("Отсутсвует подключение к интернету.");
        }
        else
        {
            switch (www.text[0])
            {
                case '0':
                    PushMessage("Вы успешно вошли в систему!");
                    AuthScreen.SetActive(false);
                    Authorized = true;
                    Login = login;
                    Password = pass;
                    Save();
                    break;
                case '1':
                    PushMessage("Нет пользователя с таким именем.");
                    break;
                case '2':
                    PushMessage("Неверный пароль.");
                    break;
                default:
                    PushMessage("Неизвестная ошибка");
                    break;
            }
        }
    }

    IEnumerator download(string s)
    {
        WWW www = new WWW(s);
        float tm = 0;
        while (!www.isDone)
        {
            tm += Time.deltaTime;
            if (tm > WaitingTime)
                break;
            yield return null;
        }
        locked = false;
        if ((!www.isDone) || (www.text == ""))
        {
            if (!www.isDone)
                PushMessage("Истекло время ожидания.");
            else
                PushMessage("Отсутсвует подключение к интернету.");
        }
        else
        {
            if ((www.text[0] == '1') && (www.text.Length < 10))
            {
                PushMessage("Не удалось обнаружить данные.");
            }
            else
            {
                ApplyData(www.text);
                PushMessage("Данные успешно получены!");
            }
        }
    }

    IEnumerator upload(WWWForm form)
    {
        WWW www = new WWW(UploadUrl, form);
        float tm = 0;
        while (!www.isDone)
        {
            tm += Time.deltaTime;
            if (tm > WaitingTime)
                break;
            yield return null;
        }
        locked = false;
        if ((!www.isDone) || (www.text == ""))
        {
            if (!www.isDone)
                PushMessage("Истекло время ожидания.");
            else
                PushMessage("Отсутсвует подключение к интернету.");
        }
        else
        {
            switch (www.text[0])
            {
                case '0':
                    PushMessage("Данные успешно загружены на сервер!");
                    break;
                case '1':
                    PushMessage("Неверное имя пользователя. Пожалуйста, перезайдите в систему.");
                    break;
                case '2':
                    PushMessage("Неверный пароль. Пожалуйста, перезайдите в систему");
                    break;
            }
        }
    }

    void PushMessage(string s)
    {
        MessageScreen.SetActive(true);
        Message.text = s;
    }

    bool InAlpabet(string s)
    {
        foreach (char x in s)
        {
            if (!Alphabet.Contains(x.ToString()))
                return false;
        }
        return true;
    }

    void UpdateActivity()
    {
        foreach (GameObject x in AuthObj)
        {
            x.gameObject.SetActive(Authorized);
        }
        foreach (GameObject x in UnauthObj)
        {
            x.gameObject.SetActive(!Authorized);
        }
    }
}