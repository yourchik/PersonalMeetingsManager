using PersonalMeetingsManager.Models;
using PersonalMeetingsManager.Services.Interface;

namespace PersonalMeetingsManager.Services.Implementation;

public class MeetingManager : IMeetingManager
{
    private readonly List<Meeting> _meetings; 

    public MeetingManager(List<Meeting> meetings)
    {
        _meetings = meetings;
    }

    public void CreateMeeting(Meeting meeting)
    {
        _meetings.Add(meeting);
    }

    public void EditMeeting(int index, Meeting newMeeting)
    {
        _meetings[index] = newMeeting;
    }

    public void DeleteMeeting(int index)
    {
        _meetings.RemoveAt(index);
    }

    public IEnumerable<Meeting> GetMeetingsByDate(DateTime date)
    {
        return _meetings.Where(meeting => meeting.StartTime.Date == date.Date);
    }
    
    public IEnumerable<Meeting> GetAllMeetings()
    {
        return _meetings;
    }
    
}
