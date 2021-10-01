using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;


namespace _301145218_Donekal__Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static Connection conn = new Connection();
        AmazonS3Client client = conn.Connect();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            bucket form = new bucket();

            List<items> item = new List<items>();

            ListBucketsResponse response = await client.ListBucketsAsync();
                foreach (S3Bucket buckets in response.Buckets)
                {
                    Console.WriteLine(buckets.BucketName + " " + buckets.CreationDate.ToShortDateString());
                    item.Add(new items { Bucket = buckets.BucketName, Creation = buckets.CreationDate.ToShortDateString() + " " + buckets.CreationDate.ToShortTimeString() });
                }
            form.dataGrid1.ItemsSource = item;
            form.Show();
        }

        class items
        {
            public String Bucket { get; set; }
            public String Creation { get; set; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Objects form1 = new Objects();
            ListBucketsResponse response = await client.ListBucketsAsync();
                foreach (S3Bucket buckets in response.Buckets)
                {
                    form1.cmbbox.Items.Add(buckets.BucketName);
                }
            form1.Show();
        }
    }
}
