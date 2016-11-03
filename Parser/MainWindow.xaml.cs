using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using System.Net;
using Parser.Models;

namespace Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static WebClient wClient;
        public static WebRequest request;
        public static WebResponse response;
        public static Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
        string urls = "";
        
        HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Net.WebClient web = new System.Net.WebClient();
            web.Encoding = UTF8Encoding.UTF8;

            string str = web.DownloadString(textBoxForUrl.Text.ToString());
            
            
            doc.LoadHtml(str);
            
            textBoxForUrl.Text = "Loaded\n" ;

            var rows = doc.DocumentNode.SelectNodes("//div[@class='description']");
            if (rows!=null)
            {
                // Берем все описания резюме на странице
                foreach (HtmlNode item in rows)
                {
                    textBoxForUrl.Text += "\n--------------------------" + "\n";
                    var childNodes = item.ChildNodes;
                    textBoxForUrl.Text += childNodes[1].Element("a").Attributes["href"].Value + "\n";
                    textBoxForUrl.Text += childNodes[1].InnerText.Trim();
                    textBoxForUrl.Text += childNodes[3].InnerText + "\n";
                    textBoxForUrl.Text += childNodes[5].Element("p").InnerText;
                    textBoxForUrl.Text += "\n--------------------------" + "\n";
                }
            }
            else
            {
                textBoxForUrl.Text = "Null";
            }
            



        }


        private void buttonWriteToDB_Click(object sender, RoutedEventArgs e)
        {
            Candidate candidate;
            string url;
            string vacancyName;
            string salary;
            string description;
            CandidateContext db = new CandidateContext();
            var rows = doc.DocumentNode.SelectNodes("//div[@class='description']");
            if (rows != null)
            {
                foreach (HtmlNode item in rows)
                {
                    var childNodes = item.ChildNodes;
                    int counter = 0;
                    url = childNodes[1].Element("a").Attributes["href"].Value + "\n";
                    vacancyName = childNodes[1].InnerText.Trim();
                    salary = childNodes[3].InnerText + "\n";
                    description = childNodes[5].Element("p").InnerText;
                    candidate = new Candidate { url = url, vacancyName = vacancyName, Salary = salary, Description = description };
                    if (candidate != null)
                    {
                        db.Candidates.Add(candidate);
                        db.SaveChanges();
                        textBoxForUrl.Text = "";
                        textBoxForUrl.Text += string.Format("{0} ready", ++counter);
                    }
                    textBoxForUrl.Text = "Запись в базу данных завершена";
                }
            }
            else
            {
                textBoxForUrl.Text = "Error";
            }
        }
    }
}
