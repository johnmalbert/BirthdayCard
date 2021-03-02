using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;  
using System.ComponentModel.DataAnnotations;

namespace Birthday_Card.ViewModels
{
    public class MessageViewModel
    {
        [Key]
        public int MessageId { get; set; }

        [Required(ErrorMessage="Please enter your name.")]
        public string Creator { get; set; }

        [Required(ErrorMessage="Please enter favorite memory, something you appreciate, or anything nice at all!")]
        public string Note { get; set; }

        [Display(Name = "Upload a fun Erin Photo!")]  
        public IFormFile ProfileImage { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}