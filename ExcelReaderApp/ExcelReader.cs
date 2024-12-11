using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReaderApp
{
    public class ExcelReader
    {
        private string _filePath;

        public ExcelReader(string filePath)
        {
            _filePath = filePath;
        }

        public object[,] ReadExcelData()
        {
            using (var package = new ExcelPackage(new FileInfo(_filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                object[,] data = worksheet.Cells[1, 1, rowCount, colCount].Value as object[,];
                return data;
            }
        }
    }
}
