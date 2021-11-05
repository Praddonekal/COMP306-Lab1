using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _301145218_Donekal__Lab3.Models
{
    [DynamoDBTable("Reviews")]
    public class Review
    {
        public string ReviewID { get; set; }
        [Required(ErrorMessage = "Please enter Title!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter Review!")]
        public string ReviewDescription { get; set; }

        public int MovieId { get; set; }
        public int MovieRating { get; set; }
        public Movie Movie { get; set; }
        public string UserEmail { get; set; }
    }
}
