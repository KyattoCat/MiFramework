using System.IO;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MiFramework.IO
{
    public class ExcelReader : IDisposable
    {
        private SpreadsheetDocument? spreadsheetDocument;
        private IEnumerator<Row>? rows;
        private IEnumerator<Cell>? cells;

        public ExcelReader(string filePath, string? sheetName = null)
        {
            try
            {
                spreadsheetDocument = SpreadsheetDocument.Open(filePath, false);
            }
            catch (OpenXmlPackageException ex)
            {
                Debug.LogException(ex.Message);
                return;
            }

            var workbookPart = spreadsheetDocument.WorkbookPart;
            var worksheetPart = workbookPart?.WorksheetParts.First();
            if (worksheetPart == null)
            {
                Debug.LogError("Open Sheet Failed");
                return;
            }

            var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            rows = sheetData?.Elements<Row>().GetEnumerator();

            if (rows == null)
            {
                Debug.LogError("Open Sheet Failed");
                return;
            }

            cells = GetNextRow();
        }

        private IEnumerator<Cell>? GetNextRow()
        {
            if (rows == null)
                throw new IndexOutOfRangeException();

            var result = rows?.Current?.Elements<Cell>()?.GetEnumerator();
            
            rows?.MoveNext();
            
            return result;
        }

        public string? GetNextColumn()
        {
            string? result = cells?.Current?.CellValue?.Text;

            if (cells?.MoveNext() == false)
            {
                cells = GetNextRow();
            }

            return result;
        }

        public void Dispose()
        {
            spreadsheetDocument?.Dispose();
        }
    }
}
