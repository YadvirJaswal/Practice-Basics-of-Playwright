using OfficeOpenXml;

namespace Practice_Basics_of_Playwright.Utilities
{
    public class TestStatusUpdater
    {
        public void UpdateTestStatus(string filePath, string sheetName, string testCaseId, string status)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Excel file is not found", filePath);
            }

            //Load the excel file
            var fileInfo = new FileInfo(filePath);
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null)
                {
                    throw new Exception($"Sheet '{sheetName}' not found in the Excel file.");
                }

                int testCaseIdColumn = 1;
                int testStatusColumn = 9;

                bool isUpdated = false;

                //Find the row for test case id
                var rows = worksheet.Dimension.End.Row;
                for (int row = 2;  row <= rows; row++)
                {
                    var currentTestCaseId = worksheet.Cells[row, testCaseIdColumn].Text;
                    if (currentTestCaseId == testCaseId)
                    {
                        // Update the status
                        worksheet.Cells[row, testStatusColumn].Value = status;
                        isUpdated = true;
                        break;
                    }
                }
                if (!isUpdated)
                {
                    throw new Exception($"TestCaseId '{testCaseId}' not found in sheet '{sheetName}'.");
                }
                // Save the changes to the Excel file
                //package.Save();
                package.SaveAs(filePath);
            }
        }
    }
}
