namespace PersonalMeetingsManager.Models;

public class Meeting
{
    public Meeting() { }

    public Meeting(DateTime newStart, DateTime newEnd, TimeSpan reminder)
    {
        StartTime = newStart;
        EndTime = newEnd;
        Reminder = reminder;
    }

    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public TimeSpan Reminder { get; set; }
    
}