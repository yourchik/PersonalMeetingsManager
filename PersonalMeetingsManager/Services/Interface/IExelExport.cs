using PersonalMeetingsManager.Models;

namespace PersonalMeetingsManager.Services.Interface;

public interface IExelExport
{
    /// <summary>
    /// Экспортирует встречи в Excel и сохраняет в папку загрузок
    /// </summary>
    /// <param name="meetings"></param>
    void ExportMeetings(IEnumerable<Meeting> meetings);
}