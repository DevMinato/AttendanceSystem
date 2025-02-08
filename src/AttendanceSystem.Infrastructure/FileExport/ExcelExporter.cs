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
                var activityNames = monthGroup.Select(r => r.ActivityName).Distinct().ToList();

                // 🟢 **Add "S/N" and "Member Name" headers**
                headerRow.CreateCell(0).SetCellValue("S/N");
                headerRow.CreateCell(1).SetCellValue("Member Name");
                subHeaderRow.CreateCell(0).SetCellValue("");
                subHeaderRow.CreateCell(1).SetCellValue("");

                /*titleRow.CreateCell(0).SetCellValue("S/N");
                titleRow.CreateCell(1).SetCellValue("Member Name");
                subHeaderRow.CreateCell(0).SetCellValue("");
                subHeaderRow.CreateCell(1).SetCellValue("");*/

                foreach (var week in weeks)
                {
                    //var activityNames = week.Select(a => a.ActivityName).Distinct().ToList();
                    int activityCount = activityNames.Count;

                    // 🟢 **Create merged headers for each week**
                    var titleCell = titleRow.CreateCell(colIndex);
                    titleCell.SetCellValue($"{week.Key.WeekStart:dd MMM} - {week.Key.WeekEnd:dd MMM yyyy}");

                    var mergedTitleRegion = new NPOI.SS.Util.CellRangeAddress(0, 0, colIndex, colIndex + activityCount - 1);
                    sheet.AddMergedRegion(mergedTitleRegion);

                    foreach (var activity in activityNames)
                    {
                        subHeaderRow.CreateCell(colIndex).SetCellValue(activity); // Attendance column
                        colIndex++;
                    }
                }

                // Leave a blank column for separation
                colIndex++;

                // 🟢 **Second Table: Attendance Percentage Per Activity in a Separate Table**
                foreach (var activity in activityNames)
                {
                    subHeaderRow.CreateCell(colIndex).SetCellValue($"Percentage {activity}");
                    colIndex++;
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
                        //var activities = week.Select(a => a.ActivityName).Distinct();
                        foreach (var activity in activityNames)
                        {
                            var record = monthGroup.FirstOrDefault(r => r.MemberName == members[i] && r.WeekStart == week.Key.WeekStart && r.ActivityName == activity);
                            row.CreateCell(colIndex).SetCellValue(record?.Attendance ?? 0);
                            colIndex++;
                        }
                    }

                    colIndex++; // 🟢 **Leave space between weeks**

                    // 🟢 **Fill Percentage Attendance Data for the Month**
                    foreach (var activity in activityNames)
                    {
                        var record = monthGroup.FirstOrDefault(r => r.MemberName == members[i] && r.ActivityName == activity);
                        row.CreateCell(colIndex).SetCellValue(Convert.ToDouble(record?.AttendancePercentage ?? 0)); // Monthly Percentage
                        colIndex++;
                    }
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