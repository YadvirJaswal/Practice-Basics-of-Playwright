using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Basics_of_Playwright.Models
{
    public class TestCase
    {
        public string TestCaseId { get; set; }
        public string TestCaseTitle { get; set; }
        public string TestCaseObjective {  get; set; }
        public string Preconditions {  get; set; }
        public string TestSteps {  get; set; }
        public string TestData {  get; set; }
        public string ExpectedResult {get; set; }
        public string ActualResult { get; set; }
        public string TestStatus {  get; set; }
    }
}
