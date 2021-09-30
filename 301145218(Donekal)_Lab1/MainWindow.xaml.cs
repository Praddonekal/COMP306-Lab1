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
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            bucket form = new bucket();

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            var s3Client1 = new AmazonS3Client(accessKeyID, secretKey);
            List<items> item = new List<items>();

            using (AmazonS3Client s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1))
            {
                ListBucketsResponse response = await s3Client.ListBucketsAsync();
                foreach (S3Bucket buckets in response.Buckets)
                {

                    Console.WriteLine(buckets.BucketName + " " + buckets.CreationDate.ToShortDateString());
                    item.Add(new items { Bucket = buckets.BucketName, Creation = buckets.CreationDate.ToShortDateString() + " " + buckets.CreationDate.ToShortTimeString() });
                }
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
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            Objects form1 = new Objects();

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);
            var s3Client1 = new AmazonS3Client(accessKeyID, secretKey);
            using (AmazonS3Client s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1))
            {
                ListBucketsResponse response = await s3Client.ListBucketsAsync();
                foreach (S3Bucket buckets in response.Buckets)
                {
                    form1.cmbbox.Items.Add(buckets.BucketName);
                }
            }
            form1.Show();
        }
    }
}
