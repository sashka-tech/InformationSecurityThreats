using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Lab2
{

    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            progressBar.Value = 0;
            DownloadExcelFile();
        }
        public void DownloadExcelFile()
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                    webClient.DownloadFileAsync(new Uri(textBox1.Text), textBox2.Text);
                    //webClient.DownloadFile(new Uri("https://bdu.fstec.ru/files/documents/thrlist.xlsx"), "thrlist.xlsx");
                }
                catch (Exception)
                {
                    MessageBox.Show("Downloading error, please try again");
                }
            }
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            progressBar.Value = e.ProgressPercentage;

        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("Download succesfully!");
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
            ((WebClient)sender).Dispose();
            button.IsEnabled = true;
            Close();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox2.Text = Path.GetFileName(textBox1.Text);
        }
    }
}
