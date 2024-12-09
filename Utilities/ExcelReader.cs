
using OfficeOpenXml;
using Practice_Basics_of_Playwright.Models;

namespace Practice_Basics_of_Playwright.Utilities
{
    public class ExcelReader
    {
        public Dictionary<string, List<TestCase>> ReadExcelFile(string filePath, string[] sheetNames)
        {
            var sheetData = new Dictionary<string, List<TestCase>>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // ensure the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Excel file is not found",filePath);
            }          

            //Load excel file
            var fileInfo = new FileInfo(filePath);
            using (var package = new ExcelPackage(fileInfo))
            {
                // Loop through each worksheet
                foreach (var sheetName in sheetNames)
                {
                    var worksheet = package.Workbook.Worksheets[sheetName];
                    if (worksheet == null)
                    {
                        Console.WriteLine($"Sheet not found: {sheetName}");
                        continue;
                    }

                    var worksheetName = worksheet.Name;
                    var testcases = new List<TestCase>();

                    int rowCount = worksheet.Dimension.Rows;
                    for(int row = 2;row <= rowCount; row++)
                    {
                        var testCase = new TestCase()
                        {
                            TestCaseId = worksheet.Cells[row, 1].Text,
                            TestCaseTitle = worksheet.Cells[row, 2].Text,
                            TestCaseObjective = worksheet.Cells[row, 3].Text,
                            Preconditions = worksheet.Cells[row,4].Text,
                            TestSteps = worksheet.Cells[row, 5].Text,
                            TestData = worksheet.Cells[row,6].Text,
                            ExpectedResult = worksheet.Cells[row, 7].Text,
                            ActualResult = worksheet.Cells[row, 8].Text,
                            TestStatus = worksheet.Cells[row, 9].Text
                        };
                        testcases.Add(testCase);
                    }
                    // Add the test cases to the dictionary with the sheet name as the key
                    sheetData.Add(worksheetName, testcases);
                }
            }
            return sheetData;
        }
    }
}
