using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using PersonalMeetingsManager.Models;
using PersonalMeetingsManager.Services.Interface;

namespace PersonalMeetingsManager.Services.Implementation;

public class ExelExport : IExelExport
{
    public void ExportMeetings(IEnumerable<Meeting> meetings)
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads",
            "Meetings.xlsx");

        using (var spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            // Add a WorkbookPart to the document.
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add column names to the first row.
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            var headerRow = new Row();
            headerRow.AppendChild(new Cell(new InlineString(new Text("Время начала"))));
            headerRow.AppendChild(new Cell(new InlineString(new Text("Время окончания"))));
            headerRow.AppendChild(new Cell(new InlineString(new Text("Напоминание"))));
            sheetData.AppendChild(headerRow);

            // Add meetings to the spreadsheet.
            foreach (var meeting in meetings)
            {
                var row = new Row();
                row.AppendChild(new Cell(new InlineString(new Text(meeting.StartTime.ToString()))));
                row.AppendChild(new Cell(new InlineString(new Text(meeting.EndTime.ToString()))));
                row.AppendChild(new Cell(new InlineString(new Text(meeting.Reminder.ToString()))));
                sheetData.AppendChild(row);
            }

            // Add a Sheets collection to the WorkbookPart.
            var sheets = workbookPart.Workbook.AppendChild(new Sheets());

            // Add a new sheet and associate it with the WorksheetPart.
            var sheet = new Sheet()
                { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Meetings" };
            sheets.Append(sheet);

            // Save the changes.
            workbookPart.Workbook.Save();
        }
    }
}