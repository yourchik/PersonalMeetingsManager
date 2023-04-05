using PersonalMeetingsManager.Models;

namespace PersonalMeetingsManager.Services.Interface;

public interface IMeetingManager
{
    void AddMeeting(Meeting meeting);
    
    void EditMeeting(int index, Meeting newMeeting);
    
    void DeleteMeeting(int index);
    
    IEnumerable<Meeting> GetMeetingsByDate(DateTime date);
}