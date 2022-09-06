using System;
using System.Collections.Generic;
using System.Linq;
using MaterializeExcel.Events;
using NetOffice.ExcelApi;
using NetOffice.ExcelApi.Enums;

namespace MaterializeExcelAddIn.Manipulation
{
    public class ExcelInteraction
    {
        private readonly Application excelApp;

        public ExcelInteraction(Application excelApp)
        {
            this.excelApp = excelApp;
        }

        public IList<string> WorksheetsName()
        {

            if (excelApp?.Sheets == null)
            {
                return new List<string>();
            }
            return excelApp.Sheets
                .Select(sheet => ((Worksheet)sheet).Name)
                .ToList();

        }

        public void WriteQueryToSheet(AddToSheetRequest request)
        {
        
            // var sheet = (Worksheet)excelApp.Sheets
            //     .First(o => ((Worksheet)o).Name
            //                 == request.SheetDestination);
            //
            // // Write Header
            // var header = new List<object> { "Name", "Date", "Start", "End" };
            //
            // var destination = sheet.get_Range("A1", Type.Missing);
            // WriteListHorizontally(destination, header);
            //
            // // find the next empty cell from column A
            // var nextRow = 2;
            // if (!string.IsNullOrEmpty(sheet.Cells[2, 1]?.Value?.ToString()))
            // {
            //     var lastCell = sheet.Cells[1, 1].End(XlDirection.xlDown);
            //     nextRow = lastCell.Row + 1;
            // }
            //
            // var entry = new List<object>
            // {
            //     request.Data.Name,
            //     request.Data.Date.Date.ToString("d"),
            //     request.Data.StartingTime.ToString("hh:mm"),
            //     request.Data.EndTime.ToString("hh:mm")
            // };
            //
            // destination = sheet.Cells[nextRow, 1];
            // WriteListHorizontally(destination, entry);
        
        }

        private Range WriteListHorizontally(Range destination, IList<object> list)
        {
            var data = new object[1, list.Count];
            for (var i = 0; i < list.Count; i++)
            {
                data[0, i] = list[i];
            }
            return WriteRange(destination, data);
        }

        private Range WriteRange(Range destinationFirstCell, object[,] data)
        {
            // Create a Range of the correct size:
            var rows = data.GetLength(0);
            var columns = data.GetLength(1);

            var range = destinationFirstCell;
            range = range.get_Resize(rows, columns);

            // Assign the Array to the Range in one shot:
            range.set_Value(Type.Missing, data);
            return range;
        }
    }
}