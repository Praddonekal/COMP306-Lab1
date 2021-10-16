using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Amazon;
using Amazon.Runtime;
using System.Data;
using System.Drawing;
using Color = System.Drawing.Color;
using Table = Amazon.DynamoDBv2.DocumentModel.Table;
using _301145218_Donekal__Lab2.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Syncfusion.Windows.PdfViewer;
using Amazon.S3;
using Amazon.S3.Model;

namespace _301145218_Donekal__Lab2
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : Window
    {
        string userEmail;
        string bookTitle;
        public PdfViewer(string email, string title)
        {
            userEmail = email;
            bookTitle = title;
            InitializeComponent();

            if (bookTitle.Equals("title1"))
            {
                pdfviewer1.ItemSource = GetBook1();
            }
            else if (bookTitle.Equals("title2"))
            {
                pdfviewer1.ItemSource = GetBook2(); 
            }
            else if (bookTitle.Equals("title3"))
            {
                pdfviewer1.ItemSource = GetBook3(); 
            }
            Bookmark(bookTitle);
        }

        private async void Bookmark(String title)
        {
            string bookmark;

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            Table table = Table.LoadTable(client, "Bookshelf");

            Document doc = await table.GetItemAsync(userEmail);

            if(title.Equals("title1"))
            {
                bookmark = doc.Values.ElementAt(7);
            }
            else if(title.Equals("title2"))
            {
                bookmark = doc.Values.ElementAt(8);
            }
            else if(title.Equals("title3"))
            {
                bookmark = doc.Values.ElementAt(9);
            }
            else
            {
                bookmark = "No book specified";
            }
            bookmarkLabel.Content = bookmark;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (bookTitle.Equals("title1"))
            {
                closing1();
            }
            else if (bookTitle.Equals("title2"))
            {
                closing2();
            }
            else if (bookTitle.Equals("title3"))
            {
                closing3();
            }
        }

        public async void closing1()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            UpdateItemOperationConfig config = new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
            };
            Table table = Table.LoadTable(client, "Bookshelf");
            var user = new Document();
            user["UserEmail"] = userEmail;
            user["DateTime1"] = DateTime.Now;
            user["LastPage1"] = pdfviewer1.CurrentPageIndex; 
            await table.UpdateItemAsync(user, config);
            MessageBox.Show("SnapShot Successful");
        }

        public async void closing2()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            UpdateItemOperationConfig config = new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
            };
            Table table = Table.LoadTable(client, "Bookshelf");
            var user = new Document();
            user["UserEmail"] = userEmail;
            user["DateTime2"] = DateTime.Now;
            user["LastPage2"] = pdfviewer1.CurrentPageIndex;
            await table.UpdateItemAsync(user, config);
            MessageBox.Show("SnapShot Successful");
        }

        public async void closing3()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            DynamoDBContext context = new DynamoDBContext(client);

            UpdateItemOperationConfig config = new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
            };
            Table table = Table.LoadTable(client, "Bookshelf");
            var user = new Document();
            user["UserEmail"] = userEmail;
            user["DateTime3"] = DateTime.Now;
            user["LastPage3"] = pdfviewer1.CurrentPageIndex;
            await table.UpdateItemAsync(user, config);
            MessageBox.Show("SnapShot Successful");
        }

        public MemoryStream GetBook1()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            AmazonS3Client client = new AmazonS3Client(accessKeyID, secretKey);

            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = "new-pradyumna";
            request.Key = "AWS Certified Solutions Architect Study Guide, 2nd Edition by Ben Piper, David Clinton.pdf";
            GetObjectResponse response = client.GetObjectAsync(request).Result;

            MemoryStream docStream = new MemoryStream();
            response.ResponseStream.CopyTo(docStream);
            return docStream;
        }
        public MemoryStream GetBook2()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            AmazonS3Client client = new AmazonS3Client(accessKeyID, secretKey);

            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = "new-pradyumna";
            request.Key = "Beginning Serverless Computing Developing with Amazon Web Services, Microsoft Azure, and Google Cloud by Maddie Stigler.pdf";
            GetObjectResponse response = client.GetObjectAsync(request).Result;

            MemoryStream docStream = new MemoryStream();
            response.ResponseStream.CopyTo(docStream);
            return docStream;
        }
        public MemoryStream GetBook3()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            AmazonS3Client client = new AmazonS3Client(accessKeyID, secretKey);

            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = "new-pradyumna";
            request.Key = "Docker Complete Guide To Docker For Beginners And Intermediates by Berg, Craig.pdf";
            GetObjectResponse response = client.GetObjectAsync(request).Result;

            MemoryStream docStream = new MemoryStream();
            response.ResponseStream.CopyTo(docStream);
            return docStream;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (bookTitle.Equals("title1"))
            {
                closing1();
            }
            else if (bookTitle.Equals("title2"))
            {
                closing2();
            }
            else if (bookTitle.Equals("title3"))
            {
                closing3();
            }
        }
    }
}
