using PersonalMeetingsManager.Models;

namespace PersonalMeetingsManager.Services.Interface;

public interface IMeetingManager
{
    /// <summary>
    /// Добавляет встречу в список встреч
    /// </summary>
    /// <param name="meeting"></param>
    void CreateMeeting(Meeting meeting);
    
    /// <summary>
    /// Редактирует встречу в списке встреч
    /// </summary>
    /// <param name="index"></param>
    /// <param name="newMeeting"></param>
    void EditMeeting(int index, Meeting newMeeting);
    
    /// <summary>
    /// Удаляет встречу из списка встреч по индексу
    /// </summary>
    /// <param name="index"></param>
    void DeleteMeeting(int index);
    
    /// <summary>
    /// Возвращает список встреч на определенную дату
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    IEnumerable<Meeting> GetMeetingsByDate(DateTime date);
    
    IEnumerable<Meeting> GetAllMeetings();
}