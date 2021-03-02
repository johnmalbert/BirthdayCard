using System;
using System.ComponentModel.DataAnnotations;

namespace Birthday_Card.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required(ErrorMessage="Please enter your name.")]
        public string Creator { get; set; }

        [Required(ErrorMessage="Please enter favorite memory, something you appreciate, or anything nice at all!")]
        public string Note { get; set; }

        public string ImgLink { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}