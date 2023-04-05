namespace PersonalMeetingsManager.Services.Interface;

public interface IMenu
{
    void ShowMenu();
    
    public void AddMeeting();

    public void EditMeeting();
    
    public void DeleteMeeting();
    
    public void GetMeetingsByDate();

    public void ExportMeetingsByDate();
}