using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;


namespace _301145218_Donekal__Lab2.Models
{
    [DynamoDBTable("Users")]
    public class User
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
        
    }
}
