using Microsoft.AspNetCore.Mvc;
using PythonWebCompiler.Models;
using System.Diagnostics;

namespace PythonWebCompiler.Controllers
{
    public class PythonCompilerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Compile(CodeInput codeInput)
        {
            if (codeInput == null || string.IsNullOrEmpty(codeInput.Code))
            {
                return View("Index", new CodeInput());
            }

            // Thực hiện biên dịch mã Python
            string result = CompilePythonCode(codeInput.Code);

            // Trả về view với mã nguồn và kết quả đầu ra
            return View("Index", new CodeInput
            {
                Code = codeInput.Code,
                Output = result // Trả về kết quả biên dịch
            });
        }



        private string CompilePythonCode(string code)
        {
            string result = string.Empty;

            // Lưu mã vào tệp tạm thời
            string filePath = Path.Combine(Path.GetTempPath(), "temp_code.py");
            System.IO.File.WriteAllText(filePath, code);

            // Cấu hình Process để thực thi mã Python
            var startInfo = new ProcessStartInfo
            {
                FileName = "python", // Đảm bảo Python có trong PATH
                Arguments = filePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(startInfo))
            {
                // Đọc kết quả đầu ra
                result = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
                process.WaitForExit(); // Chờ quá trình hoàn thành
            }

            return result; // Trả về kết quả
        }

    }
}
