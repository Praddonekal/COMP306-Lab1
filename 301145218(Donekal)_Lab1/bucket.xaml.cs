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
        String accessKeyID = "";//Access Key Here
        String secretKey = ""; //Secret Key Here

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

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            var s3Client1 = new AmazonS3Client(accessKeyID, secretKey);
            List<items> item = new List<items>();

            var respons = await s3Client1.PutBucketAsync(new PutBucketRequest
            {
                BucketName = textboxbucket.Text
            });
            message.Content = "Bucket Successfully Added";
            textboxbucket.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
