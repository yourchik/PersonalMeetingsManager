using System.Text.RegularExpressions;
using PersonalMeetingsManager.Models;
using PersonalMeetingsManager.Services.Interface;

namespace PersonalMeetingsManager.Helpers;

public static class MenuHelper
{
    private static IMeetingManager _meetingManager;
    private static IExelExport _exelExport;
    private static IMenu _menu;
    
    private const string InvalidFormatMessage = "Неверный формат времени. Попробуйте еще раз или введите 0 для возврата в главное меню: ";
    private const string MainMenuKey = "0";
    
    private static string _erorrMessage;

    public static void Initialize(IMeetingManager meetingManager, IExelExport exelExport, IMenu menu, string erorrMessage)
    {
        _meetingManager = meetingManager;
        _exelExport = exelExport;
        _menu = menu;
        _erorrMessage = erorrMessage;
    }

    /// <summary>
    /// Метод проверки валидности пункта меню
    /// </summary>
    public static void MenuSwitch()
    {
        while (true)
        {
            switch (Console.ReadLine())
            {
                case "1":
                    AddMeeting();
                    break;
                case "2":
                    EditMeeting();
                    break;
                case "3":
                    DeleteMeeting();
                    break;
                case "4":
                    SeeMeetingsByDate();
                    break;
                case "5":
                    ExportMeetingsByDate();
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный пункт меню введите число от 1 до 6");
                    continue;
            }
            break;
        }
    }

    /// <summary>
    /// Метод для добавления встречи
    /// </summary>
    public static void AddMeeting()
    {
        var newStart = PromptForValue("Введите время начала в формате (yyyy-mm-dd hh-mm): ", DateTime.Parse, ValidateDateTimeString );

        while (ValidateMeetingTime(DateTime.Now, newStart))
        {
            Console.WriteLine("Начальное время не может быть меньше текущего времени");
            newStart = PromptForValue("Введите время окончания в формате(yyyy-mm-dd hh-mm): ", DateTime.Parse, ValidateDateTimeString);
        }
        
        var newEnd = PromptForValue("Введите время окончания в формате(yyyy-mm-dd hh-mm): ", DateTime.Parse, ValidateDateTimeString);
        var reminder = PromptForValue("Введите время напоминания в формате(hh-mm): ", TimeSpan.Parse, ValidateTimeString);

        while (ValidateMeetingTime(newStart, newEnd))
        {
            Console.WriteLine("Конечное время не может быть меньше начального времени");
            newEnd = PromptForValue("Введите время окончания в формате(yyyy-mm-dd hh-mm): ", DateTime.Parse, ValidateDateTimeString);
        }

        Meeting newMeeting = new(newStart, newEnd, reminder);

        ValidateMeeting(newMeeting);
        
        _meetingManager.CreateMeeting(newMeeting);
        Console.WriteLine("Встреча успешно добавлена.");
        Console.Write("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
        BackToMenu();
    }
    
    /// <summary>
    /// Метод для изменения встречи
    /// </summary>
    public static void EditMeeting()
    {
        var meetings = GetMeetingsByDate();

        int num = 1;
        foreach (var meeting in meetings)
        {
            Console.WriteLine("Список встреч:");
            Console.WriteLine($"Встреча № {num} начинается в {meeting.StartTime} и заканчивается в {meeting.EndTime}");
        }
        
        Console.WriteLine("Введите любую клавишу для продолжения...");
        Console.ReadKey();
        int index;

        do
        {
            Console.Write("Введите номер встречи: ");
        } 
        while (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > meetings.Count);

        var meetingToEdit = meetings[index - 1];
    
        var newStart = PromptForValue("Введите новое время начала в формате (yyyy-mm-dd hh-mm): ", DateTime.Parse, ValidateDateTimeString);
        
        while (ValidateMeetingTime(DateTime.Now, newStart))
        {
            Console.WriteLine("Начальное время не может быть меньше текущего времени");
            newStart = PromptForValue("Введите время окончания в формате(yyyy-mm-dd hh-mm): ", DateTime.Parse, ValidateDateTimeString);
        }
        
        var newEnd = PromptForValue("Введите новое время окончания в формате (yyyy-mm-dd hh-mm): ", DateTime.Parse, ValidateDateTimeString);
        var reminder = PromptForValue("Введите время напоминания в формате (hh-mm): ", TimeSpan.Parse, ValidateTimeString);
    
        while (ValidateMeetingTime(newStart, newEnd))
        {
            Console.WriteLine("Конечное время не может быть меньше начального времени.");
            newEnd = PromptForValue("Введите новое время окончания в формате (yyyy-mm-dd hh-mm): ", DateTime.Parse, ValidateDateTimeString);
        }
    
        meetingToEdit.StartTime = newStart;
        meetingToEdit.EndTime = newEnd;
        meetingToEdit.Reminder = reminder;

        ValidateMeeting(meetingToEdit);

        Console.WriteLine("Встреча успешно отредактирована.");
     
        BackToMenu();
    }
    
    /// <summary>
    /// Метод для удаления встречи
    /// </summary>
    public static void DeleteMeeting()
    {
        var date = PromptForValue("Введите дату встречи в формате (yyyy-mm-dd): ", DateTime.Parse, ValidateDate);

        var meetings = GetMeetingsByDate();
        
        var num = 1;
        foreach (var meeting in meetings)
        {
            Console.WriteLine("Список встреч:");
            Console.WriteLine($"Встреча № {num} начинается в {meeting.StartTime} и заканчивается в {meeting.EndTime}");
        }

        Console.WriteLine("Введите любую клавишу для продолжения...");
        Console.ReadKey();
        
        int index;

        do
        {
            Console.Write("Введите номер встречи: ");
        } 
        while (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > meetings.Count);

        var meetingToDelete = meetings[index - 1];

        _meetingManager.DeleteMeeting(1);

        Console.WriteLine("Встреча успешно удалена.");
        BackToMenu();
    }
    
    /// <summary>
    /// Метод для просмотра встреч на дату
    /// </summary>
    public static void SeeMeetingsByDate()
    {
        var meetings = GetMeetingsByDate();
        
        Console.WriteLine("Список встреч:");
        foreach (var meeting in meetings)
        {
            Console.WriteLine($"Встреча начинается в {meeting.StartTime} и заканчивается в {meeting.EndTime}");
        }

        BackToMenu();
    }

    /// <summary>
    /// Метод для экспорта встреч на дату в Excel
    /// </summary>
    public static void ExportMeetingsByDate()
    {
        var meetings = GetMeetingsByDate();
        
        foreach (var meeting in meetings)
        {
            Console.WriteLine("Список встреч:");
            Console.WriteLine(meeting);
        }
        
        Console.WriteLine("Для экспорта встреч в Excel нажмите любую клавишу или 0 для возврата в меню");

        if (Console.ReadLine() == MainMenuKey)
        {
            Console.Clear();
            _menu.ShowMenu();
        }

        _exelExport.ExportMeetings(meetings);

        Console.WriteLine("Встречи успешно экспортированы в Excel.");
        BackToMenu();
    }
    
    /// <summary>
    /// Метод для получения встреч на дату
    /// </summary>
    /// <returns></returns>
    public static List<Meeting> GetMeetingsByDate()
    {
        var date = PromptForValue("Введите дату в формате (yyyy-mm-dd): ", DateTime.Parse, ValidateDate);

        var meetings = _meetingManager.GetMeetingsByDate(date).ToList();
        
        if (!meetings.Any())
        {
            _erorrMessage = "Встреч на эту дату нет.";
            BackToMenu(_erorrMessage);
        }

        return meetings;
    }

    /// <summary>
    /// Метод для валидации даты
    /// </summary>
    /// <param name="prompt">Строка для вывода в консоль</param>
    /// <param name="dateType">Формат даты или времени</param>
    /// <param name="validateInput">Подходящий метод валидации даты или времени</param>
    /// <returns>Дата/Время</returns>
    private static T PromptForValue<T>(string prompt, Func<string, T> dateType, Func<string, bool> validateInput)
    {
        while (true)
        {
            Console.Write(prompt);
            var meetingTime = Console.ReadLine();

            if (validateInput(meetingTime))
            {
                return dateType(meetingTime);
            }
            
            else if (prompt == InvalidFormatMessage && meetingTime == MainMenuKey)
            {
                BackToMenu();
            }

            else
            {
                prompt = InvalidFormatMessage;
                continue;
            }
        }
    }
    
    /// <summary>
    /// Метод для возрата в меню
    /// </summary>
    public static void BackToMenu()
    {
        Console.Write("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
        Console.Clear();
        _menu.ShowMenu();
    }
    
    /// <summary>
    /// Метод для возврата в меню с сообщением об ошибке
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public static void BackToMenu(string message)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        _menu.ShowMenu();
    }
    
    public static void ValidateMeeting(Meeting meeting)
    {
        var metings = _meetingManager.GetAllMeetings().ToList();
            
        foreach (var mg in metings)
        {
            if (mg.StartTime == meeting.StartTime || mg.EndTime == meeting.EndTime || mg.StartTime == meeting.EndTime || mg.EndTime == meeting.StartTime)
            {
                _erorrMessage = "Встреча на это время уже существует";
                BackToMenu(_erorrMessage);
            }
        }
    }
    
    public static bool ValidateTimeString(string time)
    {
        var patternTime = @"^([01]\d|2[0-3]):([0-5]\d)$";
        return Regex.IsMatch(time, patternTime);
    }

    public static bool ValidateDateTimeString(string dataTime)
    {
        var patternTime = @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])\s([01]\d|2[0-3]):([0-5]\d)$";
        return Regex.IsMatch(dataTime, patternTime);
    }
    
    public static bool ValidateDate(string date)
    {
        var patternTime = @"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$";
        return Regex.IsMatch(date, patternTime);
    }
    
    public static bool ValidateMeetingTime(DateTime startTime, DateTime endTime)
    {
        return startTime > endTime;
    }
}