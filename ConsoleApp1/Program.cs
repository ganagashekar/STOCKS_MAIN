using System.Diagnostics;
using System.Reflection;
using Tesseract;
using IronOcr;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {




            //var ocr = new IronTesseract();

            //using (var ocrInput = new OcrInput())
            //{
            //    ocrInput.LoadImage(@"C:\Users\ganga\Downloads\168445955 (1).png");
            //    IronOcr.License.LicenseKey = "IRONSUITE.GANGA123081.GMAIL.COM.29427-EB99B9300D-NGVRL-EGKFSWQIT6RH-T4BJ57PA46B6-W5G4DIUQBY64-TT6CPKNYICJ2-KQWSYSSUJ4OG-YK7JNANNVAI2-TMZ7L6-TSSMGIOTK5WNUA-DEPLOYMENT.TRIAL-744YO4.TRIAL.EXPIRES.22.NOV.2026";
            //    // ocrInput.LoadPdf("document.pdf");

            //    //Optionally Apply Filters if needed:
            //    ocrInput.Deskew();  // use only if image not straight
            //    ocrInput.DeNoise(); // use only if image contains digital noise

            //    var ocrResult = ocr.Read(ocrInput);
            //    Console.WriteLine(ocrResult.Text);
            //}

            var path = "C:\\Program Files\\Tesseract-OCR\\tessdata\\";



            using (var engine = new TesseractEngine(path, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(@"C:\Users\ganga\Downloads\1395604624.png"))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        // text variable contains a string with all words found
                    }
                }
            }

            //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            //path = Path.Combine(path, "tessdata");
            //path = path.Replace("file:\\", "");
            //using (var engine = new TesseractEngine(path, "eng", EngineMode.Default))
            //{
            //    engine.SetVariable("tessedit_char_whitelist", "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            //    engine.SetVariable("tessedit_unrej_any_wd", true);

            //    using (var img = Pix.LoadFromFile(@"C:\Users\ganga\Downloads\168445955 (1).png"))
            //    {
            //        using (var page = engine.Process(img))
            //        {
            //            var text = page.GetText();
            //            // text variable contains a string with all words found
            //        }
            //    }

            //    //using (var page = engine.Process(bitmap, PageSegMode.SingleLine))
            //    //    res = page.GetText();
            //}
            //    Process process = Process.Start("tesseract.exe", "out");
            //    process.WaitForExit();
            //    if (process.ExitCode == 0)
            //    {
            //        string content = File.ReadAllText("out.txt");
            //    }
            //}
        }
    }
}
