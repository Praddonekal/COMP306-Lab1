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
    /// Interaction logic for Objects.xaml
    /// </summary>
    public partial class Objects : Window
    {
        List<items> item = new List<items>();

        public Objects()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        class items
        {
            public String Object { get; set; }
            public long Size { get; set; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();
            if(response == true)
            {
               string filepath = openFileDialog.FileName;
               fileTxtBox.Text = filepath;
            }
            
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            item.Clear();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var client = new AmazonS3Client(accessKeyID, secretKey);

            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = cmbbox.SelectedItem.ToString(),
                };
                ListObjectsV2Response response;

                response = await client.ListObjectsV2Async(request);

                foreach (S3Object obj in response.S3Objects)
                {
                    item.Add(new items { Object = obj.Key, Size = obj.Size });
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
            }
            objectdatagrid.ItemsSource = item;
            objectdatagrid.Items.Refresh();
        }

            private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            item.Clear();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var client = new AmazonS3Client(accessKeyID, secretKey);

                // 2. Put the object-set ContentType and add metadata.
                var putRequest2 = new PutObjectRequest
                {
                    BucketName = cmbbox.SelectedItem.ToString(),
                    FilePath = fileTxtBox.Text,
                };

                PutObjectResponse response2 = await client.PutObjectAsync(putRequest2);
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = cmbbox.SelectedItem.ToString(),
                };
                ListObjectsV2Response response;

                response = await client.ListObjectsV2Async(request);

                foreach (S3Object obj in response.S3Objects)
                {
                    item.Add(new items { Object = obj.Key, Size = obj.Size });
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
            }
            objectdatagrid.ItemsSource = item;
            objectdatagrid.Items.Refresh();


        }
    }
}
