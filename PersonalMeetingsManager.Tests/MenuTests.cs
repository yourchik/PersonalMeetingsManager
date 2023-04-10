using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using PersonalMeetingsManager.Services.Interface;
using PersonalMeetingsManager.Services.Implementation;
using PersonalMeetingsManager.Models;
using Menu = PersonalMeetingsManager.Services.Implementation.Menu;

namespace PersonalMeetingsManager.Tests;

[TestFixture]
public class MenuTests
{
    [Test]
    public void AddMeeting_ValidInput_CallsCreateMeeting()
    {
        // Arrange
        var meetingManagerMock = new Mock<IMeetingManager>();
        var exelExportMock = new Mock<IExelExport>();
            
        var input = new StringBuilder();
        input.AppendLine("1");
        input.AppendLine("2023-10-10 10:00");
        input.AppendLine("2023-10-10 11:00");
        input.AppendLine("00:15");
        input.AppendLine("exit");

        using (var stringReader = new StringReader(input.ToString()))
        {
            Console.SetIn(stringReader);

            var meetingManager = new Mock<IMeetingManager>();
            var exelExport = new Mock<IExelExport>();
            var menu = new Menu(meetingManager.Object, exelExport.Object);

            // Act
            menu.ShowMenu();

            // Assert
            meetingManager.Verify(x => x.CreateMeeting(It.IsAny<Meeting>()), Times.Once);
        }
    }
    
    [Test]
    public void EditMeeting_ValidInput_CallsCreateMeeting()
    {
        // Arrange
        var meetingManager = new Mock<IMeetingManager>();
        var exelExport = new Mock<IExelExport>();
        var menu = new Menu(meetingManager.Object, exelExport.Object);
        
        var meeting = new Meeting
        {
            StartTime = new DateTime(2023, 10, 10, 10, 0, 0),
            EndTime = new DateTime(2023, 10, 10, 11, 0, 0),
            Reminder = new TimeSpan(9, 15, 0)
        };
        
        meetingManager.Setup(x => x.GetMeetingsByDate(It.IsAny<DateTime>())).Returns(new List<Meeting> { meeting });
        
        var input = new StringBuilder();
        input.AppendLine("2");
        input.AppendLine("2023-10-10");
        input.AppendLine(" ");
        input.AppendLine("1");
        input.AppendLine("2023-10-10");
        input.AppendLine("2023-10-12 10:00");
        input.AppendLine("2023-10-12 11:00");
        input.AppendLine("12:30");
        input.AppendLine("exit");

        using (var stringReader = new StringReader(input.ToString()))
        {
            Console.SetIn(stringReader);
            
            // Act
            menu.ShowMenu();

            // Assert
            meetingManager.Verify(x => x.EditMeeting(It.IsAny<int>(), It.IsAny<Meeting>()), Times.Once);
        }
    }
    
    [Test]
    public void DeleteMeeting_ValidInput_CallsDeleteMeeting()
    {
        // Arrange
        var meetingManager = new Mock<IMeetingManager>();
        var exelExport = new Mock<IExelExport>();
        var menu = new Menu(meetingManager.Object, exelExport.Object);

        var meeting = new Meeting
        {
            StartTime = new DateTime(2023, 10, 10, 10, 0, 0),
            EndTime = new DateTime(2023, 10, 10, 11, 0, 0),
            Reminder = new TimeSpan(9, 15, 0)
        };

        meetingManager.Setup(x => x.GetMeetingsByDate(It.IsAny<DateTime>())).Returns(new List<Meeting> { meeting });

        var input = new StringBuilder();
        input.AppendLine("3"); 
        input.AppendLine("2023-10-10");
        input.AppendLine("2023-10-10");
        input.AppendLine(" ");
        input.AppendLine("1");
        input.AppendLine("exit");

        using (var stringReader = new StringReader(input.ToString()))
        {
            Console.SetIn(stringReader);

            // Act
            menu.ShowMenu();

            // Assert
            meetingManager.Verify(x => x.DeleteMeeting(It.IsAny<int>()), Times.Once);
        }
    }
    
    [Test]
    public void SeeMeetingsByDate_ShouldListMeetings()
    {
        // Arrange
        var meetingManager = new Mock<IMeetingManager>();
        var exelExport = new Mock<IExelExport>();
        var menu = new Menu(meetingManager.Object, exelExport.Object);
    
        var meeting1 = new Meeting
        {
            StartTime = new DateTime(2023, 10, 10, 10, 0, 0),
            EndTime = new DateTime(2023, 10, 10, 11, 0, 0),
            Reminder = new TimeSpan(9, 15, 0)
        };
    
        var meeting2 = new Meeting
        {
            StartTime = new DateTime(2023, 10, 10, 12, 0, 0),
            EndTime = new DateTime(2023, 10, 10, 13, 0, 0),
            Reminder = new TimeSpan(11, 30, 0)
        };
    
        var meetings = new List<Meeting> { meeting1, meeting2 };
    
        meetingManager.Setup(x => x.GetMeetingsByDate(It.IsAny<DateTime>())).Returns(meetings);
    
        var expectedOutput = "Меню управления персональными встречами\r\n" +
                             "1. Добавить встречу\r\n" +
                             "2. Изменить встречу\r\n" +
                             "3. Удалить встречу\r\n" +
                             "4. Просмотр встреч на дату\r\n" +
                             "5. Экспорт встреч на дату в Excel\r\n" +
                             "6. Выход\r\n" +
                             "Выберите пункт меню: Введите дату в формате (yyyy-mm-dd): Список встреч:\r\n" +
                             $"Встреча начинается в {meeting1.StartTime:dd.MM.yyyy HH:mm:ss} и заканчивается в {meeting1.EndTime:dd.MM.yyyy HH:mm:ss}\r\n" +
                             $"Встреча начинается в {meeting2.StartTime:dd.MM.yyyy HH:mm:ss} и заканчивается в {meeting2.EndTime:dd.MM.yyyy HH:mm:ss}\r\n" +
                             "Нажмите любую клавишу для продолжения...";
    
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        
        var input = new StringBuilder();
        input.AppendLine("4"); 
        input.AppendLine("2023-10-10");
        input.AppendLine("exit");

        using (var stringReader = new StringReader(input.ToString()))
        {
            Console.SetIn(stringReader);

            // Act
            menu.ShowMenu();

            // Assert
            var output = stringWriter.ToString();
            Assert.AreEqual(expectedOutput, output);
            meetingManager.Verify(x => x.GetMeetingsByDate(It.IsAny<DateTime>()), Times.Once);
        }
    }
    
    [Test]
    public void ExportMeetings_ShouldExportMeetingsToExcel()
    {
        // Arrange
        var meetingManager = new Mock<IMeetingManager>();
        var exelExport = new Mock<IExelExport>();
        var menu = new Menu(meetingManager.Object, exelExport.Object);

        var meeting1 = new Meeting
        {
            StartTime = new DateTime(2023, 10, 10, 10, 0, 0),
            EndTime = new DateTime(2023, 10, 10, 11, 0, 0),
            Reminder = new TimeSpan(9, 15, 0)
        };

        var meeting2 = new Meeting
        {
            StartTime = new DateTime(2023, 10, 10, 12, 0, 0),
            EndTime = new DateTime(2023, 10, 10, 13, 0, 0),
            Reminder = new TimeSpan(11, 30, 0)
        };

        var meetings = new List<Meeting> { meeting1, meeting2 };
        meetingManager.Setup(x => x.GetMeetingsByDate(It.IsAny<DateTime>())).Returns(meetings);
        
        var input = new StringBuilder();
        input.AppendLine("5");
        input.AppendLine("2023-10-10");
        input.AppendLine(" ");
        input.AppendLine("exit");

        using (var stringReader = new StringReader(input.ToString()))
        {
            Console.SetIn(stringReader);

            // Act
            menu.ShowMenu();

            // Assert
            var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            var filePath = Path.Combine(downloadsPath, "Meetings.xlsx");
            Assert.IsTrue(File.Exists(filePath), "Файл не найден в папке: " + filePath);
            exelExport.Verify(x => x.ExportMeetings(It.IsAny<IEnumerable<Meeting>>()), Times.Once);
        }
    }
}

