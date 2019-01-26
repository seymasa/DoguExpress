using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Media;
using System.ComponentModel;
using System.Windows.Forms;

namespace EasternExpressTicked
{
    public class UnitTest1
    {

        private static By nereden_, nereye_, tarih_;
        private static IWebDriver driver = new ChromeDriver(); //Google Chrome’un açılması için yapıyoruz. Aynı zamanda driver diye nesne tanımlamış olduk. Bu nesne üzerinden işlemleri yapacağız.

        public static bool TestMethod1(string nereden, string nereye, DateTime tarih, BackgroundWorker worker)
        {
            worker.ReportProgress(0);
            driver.Manage().Window.Minimize();

            driver.Navigate().GoToUrl("https://ebilet.tcddtasimacilik.gov.tr/");

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            worker.ReportProgress(20); // Henüz 20% durumundayız... (ChromeDriver oluşturuldu)

            string _tarih_ = tarih.Date.ToString().Substring(0, 10);

            nereden_ = By.Id("nereden");
            driver.FindElement(nereden_).Clear();
            driver.FindElement(nereden_).SendKeys(nereden);

            nereye_ = By.Id("nereye");
            driver.FindElement(nereye_).Clear();
            driver.FindElement(nereye_).SendKeys(nereye);

            tarih_ = By.Id("trCalGid_input");
            driver.FindElement(tarih_).Clear();
            driver.FindElement(tarih_).SendKeys(_tarih_);

            worker.ReportProgress(40);
            // (driver as IJavaScriptExecutor).ExecuteScript("PrimeFaces.ab({source:'btnSeferSorgula',process:'biletAramaForm',update:'msg'});");
            var element = driver.FindElement(By.Name("btnSeferSorgula"));
            element.Click();
            worker.ReportProgress(60);

            bool complate = false;
            do
            {
                Console.WriteLine("Sorgunun bitip bitmediğini kontrol ediyoruz...");
                complate = (bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");

            } while (!complate);

            worker.ReportProgress(100); // 100% durumundayız... (AJAX sorgusu bitti!)
            Console.WriteLine("Sorgu bitmiş!");

            string currUrl = driver.Url;
            Console.WriteLine("Url: " + currUrl);

            bool sonuc = !(currUrl == "https://ebilet.tcddtasimacilik.gov.tr/view/eybis/tnmGenel/tcddWebContent.jsf" ||
                currUrl.StartsWith("https://ebilet.tcddtasimacilik.gov.tr/view/eybis/tnmGenel/tcddWebContent.jsf"));


            return sonuc;


            /* try
            {
                SoundPlayer mediaPlayer = new SoundPlayer();
                string path = Application.StartupPath.ToString() + "\\at.wav";
                mediaPlayer.SoundLocation = path;
                mediaPlayer.Play();
                driver.FindElement(By.Name("mainTabView:btnDevam44"));
                Console.WriteLine("sefer var");
            }
            catch  { Console.WriteLine("Sefer Yok"); } */
        }
        public static void DriverStop()
        {
            driver.Quit();
        }
    }
}
