using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EasternExpressTicked
{
    public partial class Form1 : Form
    {
        private BackgroundWorker backgroundWorker;

        public Form1()
        {
            InitializeComponent();

            // İşçimizi tanımlayalım
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;

            // İşçimizin hangi işi yapması gerektiğini söylüyoruz (EventHandler ile aynı mantık :))
            backgroundWorker.DoWork +=
                new DoWorkEventHandler(CheckTicket);

            // İşçimiz işi yaparken, gelişmeleri kime haber verecek onu söylüyoruz (TicketProgress'i çalıştır dedik)
            backgroundWorker.ProgressChanged +=
                new ProgressChangedEventHandler(TicketProgress);

            // İşçimiz işi bitirince kime haber verecek onu söylüyoruz (TicketResult'u çalıştır dedik)
            backgroundWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(TicketResult);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Lütfen Bekleyiniz";
            timerTrigger.Enabled = true;
            timerTrigger.Start();

            // timer1.Tick içerisindeki fonksiyonumuz 5 dakika sonra çalışacağından, ilk tıklamada 1 işlemi elle yapıyoruz. (sanki 5 dakika geçmiş gibi)
            // çalışacak fonksiyona gidecek argümanları tek bir parametre yapmamız lazım o yüzden list haline getiriyoruz.
            List<object> arguments = new List<object>();
            arguments.Add(comboBox1.Text);
            arguments.Add(comboBox2.Text);
            arguments.Add(dateTimePicker1.Value);
            backgroundWorker.RunWorkerAsync(arguments);
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I'm Şeyma.\nI developer of this program.\nYou can contact me here : seymasargil@gmail.com\nThis program is developed for dear my home friend(Çağrı Nur Kelleci).\nOhh! yes, you can take advantage of free ", "Hi user!");
        }


        // Arkaplanda çalışacak fonksiyonumuz
        private void CheckTicket(object sender, DoWorkEventArgs e)
        {
            // İşçimizi alalım belki kullanırız
            BackgroundWorker worker = sender as BackgroundWorker;

            // Argümanları paketlemiştik, paketinden çıkartalım
            List<object> arguments = (List<object>)e.Argument;
            string from = (string)arguments[0];
            string to = (string)arguments[1];
            DateTime date = (DateTime)arguments[2];

            // EasternExpress metodumuzdan dönen değeri, işçimizin "sonuç" olarak almasını sağladık. (e.Result)
            e.Result = UnitTest1.TestMethod1(from, to, date, worker);


        }


        private void TicketProgress(object sender, ProgressChangedEventArgs e)
        {
            int percentage = e.ProgressPercentage;
            pbProgress.Value = percentage;
        }

        // İşçimiz bize çalışmasının bittiğini söyleyince...
        private void TicketResult(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // Hata varsa, mesaj göster
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // Eğer kullanıcı tarafından iptal edildiyse...
                lblStatus.Text = "Inquiry, stopped by user!";
            }
            else
            {
                // İşçimiz başarılı bir şekilde bitirdiyse;
                bool status = (bool)e.Result;

                if (status)
                {
                    lblStatus.Text = "Sefer Bulunduu !! ^_^";
                    timerTrigger.Stop();
                    DialogResult dialogResult = MessageBox.Show("İşleme Burada Devam Etmek İster Misin? ? (Hayır, derseniz browser kapatılacaktır!)", "Sefer Bulunduu !! ^_^", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.No)
                    {
                        UnitTest1.DriverStop();
                    }
                }
                else
                {
                    lblStatus.Text = "Üzgünüm!, Seyahat Bulunamadı :( (2 dk sonra tekrar kontrol edilecektir!)";
                }
            }
        }

        // Sayaç tetiklenirse...
        private void TriggerBackground(object sender, EventArgs e)
        {
            // Parametreleri paketle
            List<object> arguments = new List<object>();
            arguments.Add(comboBox1.Text);
            arguments.Add(comboBox2.Text);
            arguments.Add(dateTimePicker1.Value);
            // İşçimize çalışması gerektiğini söyle
            backgroundWorker.RunWorkerAsync(arguments);
            lblStatus.Text = "Sorgulama yapılıyor...";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Birazdan, Programı Kullanmak İçin 'Timer Triger Start' Butonuna Bastığınızda Bir Chrome Tarayıcısı Açılacaktır. Lütfen Bütün İşlemleriniz Bitmeden Açılan Tarayıcıyı Kapatmayınız. ","Bilgi!",MessageBoxButtons.OK, MessageBoxIcon.Information);
            timerTrigger.Tick += new EventHandler(TriggerBackground);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.UseVisualStyleBackColor = false;
            button1.BackColor = Color.GhostWhite;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.UseVisualStyleBackColor = true;
        }
    }
}
