using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AttendanceSystem.Infrastructure.FileExport
{
    public class WordExporter : IWordExporter
    {
        public async Task<byte[]> ExportAttendanceReportToWordAsync(DateTime startDate, DateTime endDate, string fellowshipName, string period, List<MonthlyAttendanceReportViewModel> reportData)
        {
            using (var memoryStream = new MemoryStream()) // ✅ Use MemoryStream instead of saving to a file
            {
                using (var document = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document, true))
                {
                    var mainPart = document.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    // 🟢 **Title Section**
                    body.Append(CreateParagraph($"{period.ToUpper()} ANALYSIS REPORT OF CELL/FELLOWSHIP MEMBERSHIP ATTENDANCE TRACKER", true, JustificationValues.Center));
                    body.Append(CreateParagraph($"DATE: {DateTime.UtcNow:dd MMMM yyyy}", false, JustificationValues.Left));
                    body.Append(CreateParagraph($"{ConvertToSingular(period).ToUpper()} UNDER REVIEW: {startDate:MMMM}", false, JustificationValues.Left));
                    body.Append(CreateParagraph($"Name of Fellowship/Cell: {fellowshipName}", false, JustificationValues.Left));
                    body.Append(new Paragraph()); // Add spacing before table

                    // 🟢 **Table Creation**
                    var table = new Table();
                    var tableProperties = new TableProperties(new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }));
                    table.AppendChild(tableProperties);

                    // 🟢 **Table Header**
                    var headerRow = new TableRow();
                    foreach (var header in new[] { "S/N", "Type of Meeting", "Frequency", "Total Attendees", "100% Attendance", "75% Attendance", "50% Attendance", "Below 50% Attendance" })
                    {
                        headerRow.AppendChild(CreateTableCell(header, true));
                    }
                    table.AppendChild(headerRow);

                    // 🟢 **Table Data**
                    int serialNumber = 1;
                    foreach (var data in reportData)
                    {
                        var row = new TableRow();
                        row.Append(
                            CreateTableCell(serialNumber.ToString()), // S/N
                            CreateTableCell(data.Activity), // Type of Meeting
                            CreateTableCell(data.Frequency.ToString()), // Frequency
                            CreateTableCell(data.TotalAttendees.ToString()), // Total Attendees
                            CreateTableCell(data.Count100.ToString()), // 100% Attendance
                            CreateTableCell(data.Count75.ToString()), // 75% Attendance
                            CreateTableCell(data.Count50.ToString()), // 50% Attendance
                            CreateTableCell(data.CountBelow50.ToString()) // Below 50% Attendance
                        );
                        table.AppendChild(row);
                        serialNumber++;
                    }

                    body.AppendChild(table);
                    body.Append(new Paragraph()); // Add spacing

                    // 🟢 **Footer Notes**
                    body.Append(CreateParagraph("*Note: Frequency of prayer meeting will be the number of weeks in a month prayer group meeting held (this applies to all listed meetings).", false, JustificationValues.Left));

                    //document.Save();
                }
                return memoryStream.ToArray();
            }
        }

        // **Helper Methods**
        private Paragraph CreateParagraph(string text, bool bold = false, JustificationValues? alignment = null)
        {
            var runProperties = new RunProperties();
            if (bold) runProperties.Append(new Bold());

            var run = new Run();
            run.Append(runProperties);
            run.Append(new Text(text));

            var paragraph = new Paragraph(run);

            // 🟢 Set default alignment if null
            paragraph.ParagraphProperties = new ParagraphProperties(
                new Justification { Val = alignment ?? JustificationValues.Left }
            );

            return paragraph;
        }


        private TableCell CreateTableCell(string text, bool isHeader = false)
        {
            var cell = new TableCell(new Paragraph(new Run(new Text(text))));
            if (isHeader)
            {
                cell.TableCellProperties = new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2400" },
                    new Shading { Val = ShadingPatternValues.Clear, Fill = "D9D9D9" }); // Light gray background
            }
            else
            {
                cell.TableCellProperties = new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = "2400" });
            }
            return cell;
        }

        private string ConvertToSingular(string input)
        {
            var conversionMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Monthly", "Month" },
                { "Yearly", "Year" },
                { "Daily", "Day" },
                { "Weekly", "Week" },
            };

            return conversionMap.TryGetValue(input, out var singularForm) ? singularForm : input;
        }

    }
}