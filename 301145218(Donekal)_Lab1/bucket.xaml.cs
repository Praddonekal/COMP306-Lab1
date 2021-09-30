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
    /// Interaction logic for bucket.xaml
    /// </summary>
    public partial class bucket : Window 
    {
         

        public bucket()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
       
        class items
        {
            public String Bucket { get; set; }
            public String Creation { get; set; }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            var s3Client1 = new AmazonS3Client(accessKeyID, secretKey);
            List<items> item = new List<items>();

            var respons = await s3Client1.PutBucketAsync(new PutBucketRequest
            {
                BucketName = textboxbucket.Text
            });
            message.Content = "Bucket Successfully Added";

            using (AmazonS3Client s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1))
            {
                ListBucketsResponse response = await s3Client.ListBucketsAsync();
                foreach (S3Bucket buckets in response.Buckets)
                {

                    Console.WriteLine(buckets.BucketName + " " + buckets.CreationDate.ToShortDateString());
                    item.Add(new items { Bucket = buckets.BucketName, Creation = buckets.CreationDate.ToShortDateString() + " " + buckets.CreationDate.ToShortTimeString() });
                }
            }
            dataGrid1.ItemsSource = item;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
