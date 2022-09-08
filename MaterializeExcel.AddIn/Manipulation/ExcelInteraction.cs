using System.Linq;
using System.Text.RegularExpressions;
using MaterializeExcel.Events;
using Microsoft.Office.Interop.Excel;

namespace MaterializeExcel.AddIn.Manipulation
{
    public class ExcelInteraction
    {
        private readonly Application _excelApp;

        public ExcelInteraction()
        {
            _excelApp = AddInContext.ExcelApp;
        }

        public void WriteQueryToSheet(AddToSheetRequest request)
        {
            var wb = _excelApp.ActiveWorkbook;
            if (wb == null)
                return;

            var fullyQualifiedName = GetFullyQualifiedName(request);
            dynamic ws = AddNewSheet(wb, fullyQualifiedName);
            ws.Range["A1"].Formula2 = $"=MZ_TAIL(\"{fullyQualifiedName}\")";
        }

        private static string GetFullyQualifiedName(AddToSheetRequest request)
        {
            // system is not a real database name, so exclude from fq name
            return request.DatabaseName == "system"
                ? $"{request.SchemaName}.{request.ObjectName}"
                : $"{request.DatabaseName}.{request.SchemaName}.{request.ObjectName}";
        }

        private static string Truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        private static Worksheet AddNewSheet(Workbook workbook, string sheetName)
        {
            Worksheet worksheet = workbook.Worksheets.Add(Type: XlSheetType.xlWorksheet);

            string[] invalidChars = { ":", "\"", "/", "?", "*", "[", "]" };
            var pattern = string.Join("|", invalidChars.Select(Regex.Escape));
            var safeName = Regex.Replace(sheetName, pattern, "_");
            // sheet name max len is 31, but leave a few chars for duplicates
            safeName = Truncate(safeName, 26);

            var nextIndex = 0;
            foreach (Worksheet sheet in workbook.Worksheets)
            {
                if (sheet.Name.StartsWith(safeName))
                {
                    if (nextIndex == 0)
                        nextIndex = 1;
                    var m = Regex.Match(sheet.Name, @"\((?<index>\d+)\)$");
                    if (!m.Success) continue;
                    var match = int.Parse(m.Groups["index"].Value);
                    if (match >= nextIndex)
                        nextIndex = match + 1;
                }
            }

            worksheet.Name = nextIndex == 0 ? safeName : $"{safeName} ({nextIndex})";
            return worksheet;
        }
    }
}