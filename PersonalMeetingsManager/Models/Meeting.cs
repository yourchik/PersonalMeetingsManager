namespace PersonalMeetingsManager.Models;

public class Meeting
{
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public TimeSpan Reminder { get; set; }
    
    public bool Validate()
    {
        if (StartTime < DateTime.Now)
        {
            Console.WriteLine("Время начала встречи не может быть меньше текущего времени.");
            return false;
        }
        
        if (EndTime < DateTime.Now)
        {
            Console.WriteLine("Время окончания встречи не может быть меньше текущего времени.");
            return false;
        }
        
        if (StartTime > EndTime)
        {
            Console.WriteLine("Время начала встречи не может быть больше времени окончания встречи.");
            return false;
        }
        
        if (Reminder > StartTime.TimeOfDay)
        {
            Console.WriteLine("Напоминание должно быть раньше времени начала встречи.");
            return false;
        }
        
        return true;
    }
}