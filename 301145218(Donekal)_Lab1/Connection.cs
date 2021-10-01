using Amazon.S3;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _301145218_Donekal__Lab1
{
    class Connection
    {
        public AmazonS3Client Connect()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            String accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            String secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            AmazonS3Client client = new AmazonS3Client(accessKeyID, secretKey);
            return client;
        }
    }
}
