using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Application.Models;
using NPOI.XSSF.UserModel;
using System.Data;

namespace AttendanceSystem.Infrastructure.FileExport
{
    public class ExcelExporter : IExcelExporter
    {

        public byte[] ExportAttendanceReportsToExcel(List<AttendanceReportExportDto> attendanceRecords)
        {
            var workbook = new XSSFWorkbook();
            var groupedByMonth = attendanceRecords.GroupBy(r => r.WeekStart.ToString("MMMM yyyy"));

            foreach (var monthGroup in groupedByMonth)
            {
                var sheet = workbook.CreateSheet(monthGroup.Key);
                var titleRow = sheet.CreateRow(0); // 🔹 Date range row (NEW)
                var headerRow = sheet.CreateRow(1); // 🔹 Column headers (S/N, Member Name, Activities)
                var subHeaderRow = sheet.CreateRow(2); // 🔹 Activity names (under each week)

                int colIndex = 2; // Start after S/N and Member Name

                var weeks = monthGroup.GroupBy(r => new { r.WeekStart, r.WeekEnd });

                // 🟢 **Add "S/N" and "Member Name" headers**
                headerRow.CreateCell(0).SetCellValue("S/N");
                headerRow.CreateCell(1).SetCellValue("Member Name");
                subHeaderRow.CreateCell(0).SetCellValue("");
                subHeaderRow.CreateCell(1).SetCellValue("");

                foreach (var week in weeks)
                {
                    var activityNames = week.Select(a => a.ActivityName).Distinct().ToList();
                    int activityCount = activityNames.Count;

                    // 🟢 **Create merged title header for each week (above column headers)**
                    var titleCell = titleRow.CreateCell(colIndex);
                    titleCell.SetCellValue($"{week.Key.WeekStart:dd MMM} - {week.Key.WeekEnd:dd MMM yyyy}");

                    var mergedTitleRegion = new NPOI.SS.Util.CellRangeAddress(0, 0, colIndex, colIndex + activityCount - 1);
                    sheet.AddMergedRegion(mergedTitleRegion);

                    // 🟢 **Add column headers under the merged week title**
                    foreach (var activity in activityNames)
                    {
                        subHeaderRow.CreateCell(colIndex).SetCellValue(activity);
                        colIndex++;
                    }

                    colIndex++; // 🟢 **Leave space between week groups**
                }

                // 🟢 **Fill in member rows**
                var members = monthGroup.Select(r => r.MemberName).Distinct().ToList();
                for (int i = 0; i < members.Count; i++)
                {
                    var row = sheet.CreateRow(i + 3); // 🔹 Start from row index 3 (after headers)
                    row.CreateCell(0).SetCellValue(i + 1); // S/N
                    row.CreateCell(1).SetCellValue(members[i]); // Member Name

                    colIndex = 2;
                    foreach (var week in weeks)
                    {
                        var activities = week.Select(a => a.ActivityName).Distinct();
                        foreach (var activity in activities)
                        {
                            var record = monthGroup.FirstOrDefault(r => r.MemberName == members[i] && r.WeekStart == week.Key.WeekStart && r.ActivityName == activity);
                            row.CreateCell(colIndex++).SetCellValue(record?.Attendance ?? 0);
                        }

                        colIndex++; // 🟢 **Leave space between weeks**
                    }
                }

                // 🟢 **Auto-size columns for better readability**
                for (int i = 0; i < colIndex; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }

            // Save the Excel file
            /*using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }*/

            // save the workbook to a stream
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);

            // convert the stream to a byte array
            byte[] bytes = stream.ToArray();

            return bytes;
        }
    }
}