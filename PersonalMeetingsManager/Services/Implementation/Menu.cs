using PersonalMeetingsManager.Models;
using PersonalMeetingsManager.Services.Interface;

namespace PersonalMeetingsManager.Services.Implementation;

public class Menu : IMenu
{
    private readonly IMeetingManager _meetingManager;
    private readonly IExelExport _exelExport;
    
    public Menu(IMeetingManager meetingManager, IExelExport exelExport)
    {
        _meetingManager = meetingManager;
        _exelExport = exelExport;
    }
    
    public void ShowMenu()
    {
        Console.WriteLine("Меню управления персональными встречами");
        
        Console.WriteLine("1. Добавить встречу");
        Console.WriteLine("2. Изменить встречу");
        Console.WriteLine("3. Удалить встречу");
        Console.WriteLine("4. Просмотр встреч на дату");
        Console.WriteLine("5. Экспорт встреч на дату в Excel");
        Console.WriteLine("6. Выход");
        
        Console.Write("Выберите пункт меню: ");
        
        MenuSwitch();
    }

    private void MenuSwitch()
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
                    GetMeetingsByDate();
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

    public void AddMeeting()
    {
        Console.Write("Введите время начала в формате (yyyy-mm-dd hh-mm): ");
        var newStart = Console.ReadLine();
        
        DateTime newStart1 = DateTime.Parse(Console.ReadLine());
        
        Console.Write("Введите время окончания в формате(yyyy-mm-dd hh-mm): ");
        DateTime newEnd = DateTime.Parse(Console.ReadLine());
        
        Console.Write("Введите время напоминания в формате(hh-mm): ");
        TimeSpan reminder = TimeSpan.Parse(Console.ReadLine());
        
        BackToMenu();
        
    }
    
    public void EditMeeting()
    {
        Console.Write("Введите номер встречи для редактирования: ");
        int editIndex = int.Parse(Console.ReadLine());
        
        Console.Write("Введите время начала в формате (yyyy-mm-dd hh-mm): ");
        DateTime newStart = DateTime.Parse(Console.ReadLine());
        
        Console.Write("Введите время окончания в формате(yyyy-mm-dd hh-mm): ");
        DateTime newEnd = DateTime.Parse(Console.ReadLine());
        
        Console.Write(" ");
        TimeSpan newReminder = TimeSpan.Parse(Console.ReadLine());
        
        BackToMenu();
    }
    
    public void DeleteMeeting()
    {
        Console.Write("Введите номер встречи для удаления: ");
        int deleteIndex = int.Parse(Console.ReadLine());
        
        BackToMenu();
    }
    
    public void GetMeetingsByDate()
    {
        Console.Write("Введите дату встречи в формате (yyyy-mm-dd): ");
        DateTime date = DateTime.Parse(Console.ReadLine());
        
        BackToMenu();
    }
    
    public void ExportMeetingsByDate()
    {
        Console.Write("Введите дату встречи в формате (yyyy-mm-dd): ");
        DateTime date = DateTime.Parse(Console.ReadLine());
        
        BackToMenu();
    }

    private void BackToMenu()
    {
        Console.WriteLine("Нажмите 0 для возврата в главное меню");
        Console.ReadLine();
        if (Console.ReadLine() == "0")
            ShowMenu();
    }

    private void SetMeetingTime()
    {
        Console.Write("Введите время начала в формате (yyyy-mm-dd hh-mm): ");
        DateTime newStart = DateTime.Parse(Console.ReadLine());
        Console.Write("Введите время окончания в формате(yyyy-mm-dd hh-mm): ");
        DateTime newEnd = DateTime.Parse(Console.ReadLine());
    }
    
    private bool ValidateMenu()
    {
        return true;
    }
    
    private bool ValidateTime()
    {
        return true;
    }
}