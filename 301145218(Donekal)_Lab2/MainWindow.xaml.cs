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

namespace _301145218_Donekal__Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User user;
        private string tableName = "Users";
        private AmazonDynamoDBClient client;
        private DynamoDBContext context;



        public MainWindow()
        {
            InitializeComponent();
            user = new User();
            this.BtnLogin.IsEnabled = true;

        }


        private void TxtUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            user.UserEmail = TxtUserEmail.Text;
        }

        private void BtnSignup_Click(object sender, RoutedEventArgs e)
        {
            creatingTable();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            await loadDataAsync();
        }

        private async void creatingTable()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);


            if (string.IsNullOrEmpty(TxtUserEmail.Text) || string.IsNullOrEmpty(TxtPassword.Password))
            {
                MessageBox.Show("Fields can't be empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
                context = new DynamoDBContext(client);
                List<string> currentTables = client.ListTablesAsync().Result.TableNames;


                if (!currentTables.Contains(tableName))
                {
                    await CreateUserTable(client, tableName);
                    await saveUser(context);
                }
                else
                {
                    await saveUser(context);
                }
                BtnLogin.IsEnabled = true;
            }
        }
        public static async Task CreateUserTable(AmazonDynamoDBClient client, string tableName)
        {
            var tableResponse = client.ListTablesAsync();
            if (!tableResponse.Result.TableNames.Contains(tableName))
            {
                await Task.Run(() =>
                {

                    client.CreateTableAsync(new CreateTableRequest
                    {
                        TableName = "Users",
                        ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 5, WriteCapacityUnits = 5 },
                        KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName="UserEmail",
                        KeyType=KeyType.HASH
                    }
                },
                        AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName="UserEmail", 
                        AttributeType=ScalarAttributeType.S
                    }
                }
                    });

                    CreateTableRequest request = new CreateTableRequest
                    {
                        TableName = "Bookshelf",
                        AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "UserEmail",
                        AttributeType = ScalarAttributeType.S
                    }
                },
                        KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "UserEmail",
                        KeyType = KeyType.HASH //Partition key
                    }
                },
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 5,
                            WriteCapacityUnits = 5
                        },
                    };
                    var response = client.CreateTableAsync(request);
                    Thread.Sleep(5000);
                });
            }

        }
        private async Task saveUser(DynamoDBContext context)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            bool userExisted;
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            Table table = Table.LoadTable(client, "Users");
            Table table2 = Table.LoadTable(client, "Bookshelf");
            string email = TxtUserEmail.Text;
            Document doc = await table.GetItemAsync(email);
            if (doc == null)
            {
                userExisted = false;
            }
            else
            {
                userExisted = true;
            }

            user.Password = TxtPassword.Password;
            var book = new Document();
            book["UserEmail"] = email;
            book["BookTitle1"] = "AWS Certified Solutions";
            book["DateTime1"] = DateTime.Now;
            book["LastPage1"] = 1;
            book["BookTitle2"] = "Beginning Serverless Computing";
            book["DateTime2"] = DateTime.Now;
            book["LastPage2"] = 1;
            book["BookTitle3"] = "Docker";
            book["DateTime3"] = DateTime.Now;
            book["LastPage3"] = 1;
            /*
            user.BookTitle1 = "AWS Certified Solutions";
            user.DateTime1 = DateTime.Now;
            user.LastPage1 = Convert.ToInt32("1");
            user.BookTitle2 = "Beginning Serverless Computing";
            user.DateTime2 = DateTime.Now;
            user.LastPage2 = Convert.ToInt32("1");
            user.BookTitle3 = "Docker";
            user.DateTime3 = DateTime.Now;
            user.LastPage3 = Convert.ToInt32("1");
            */
            if (userExisted)
            {
                MessageBox.Show("Account exists already!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                await Task.Run(() =>
                {
                    context.SaveAsync<User>(user);
                    table2.PutItemAsync(book);
                    MessageBox.Show("Account Created Successfully!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        public async Task loadDataAsync()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);
            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            Table table = Table.LoadTable(client, "Users");
            string email = TxtUserEmail.Text;
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Fields can't be empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                    Document doc = await table.GetItemAsync(email);
                    string emailInput = doc.Values.ElementAt(1);
                    string userPasword = doc.Values.ElementAt(0);
                    string result = emailInput;
                    string pass = userPasword;
                    if (email == result & password == pass)
                    {

                        BooksList booksForm = new BooksList(emailInput);

                        MessageBox.Show("Successfully Logged In");
                        booksForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Email or Password entered!");
                    }

            }
        }
       
    }

}
