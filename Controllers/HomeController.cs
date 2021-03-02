using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Birthday_Card.Models;
using Microsoft.EntityFrameworkCore;
using Birthday_Card.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting; 


namespace Birthday_Card.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(MyContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("HappyBirthday");
        }

        [HttpGet("card/new")]
        public IActionResult NewCard()
        {
            return View();
        }
        [HttpGet("message")]
        public IActionResult Message()
        {
            return View();
        }

        [HttpPost("post")]
        public IActionResult Post(MessageViewModel message)
        {

            if(ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(message);

                //add to db, then display on confirm page
                //create new message 
                Message newMessage = new Message {
                    Creator = message.Creator,
                    Note = message.Note,
                    ImgLink = uniqueFileName
                };
                _context.Add(newMessage);
                _context.SaveChanges();
                return RedirectToAction("ConfirmPost", newMessage);
            }
            else
            {
                return View("Message");
            }
        }

        [HttpGet("ConfirmPost")]
        public IActionResult ConfirmPost(Message m)
        {
            ViewBag.MessageToConfirm = m;
            return View();
        }
        [HttpGet("card/edit/{num}")]
        public IActionResult Edit(int num)
        {
            ViewBag.Message = _context.Messages.First(m => m.MessageId == num);
            return View();
        }

        [HttpGet("card/display/{num}")]
        public IActionResult ShowCard(int num)
        {
            //display the finished card
            ViewBag.MessageToConfirm = _context.Messages.FirstOrDefault(m => m.MessageId == num);
            if(ViewBag.MesssageToConfirm != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("HappyBirthday");
            }
        }

        [HttpGet("happy-birthday-erin")]
        public IActionResult HappyBirthday()
        {
            ViewBag.Messages = _context.Messages.ToList();
            return View();
        }

        [HttpGet("delete")]
        public IActionResult Delete()
        {
            ViewBag.Messages = _context.Messages.ToList();
            return View();
        }

        [HttpPost("edit/{num}")]
        public IActionResult EditPost(MessageViewModel message, int num)
        {
            Message messageToEdit = _context.Messages.First(m => m.MessageId == num);
            if(ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(message);
                //add to db, then display on confirm page
                //create new message 
                messageToEdit.Creator = message.Creator;
                messageToEdit.Note = message.Note;
                messageToEdit.ImgLink = uniqueFileName;
                _context.SaveChanges();
                return RedirectToAction("ConfirmPost", messageToEdit);
            }
            else
            {
                ViewBag.Message = _context.Messages.First(m => m.MessageId == num);
                return View("Edit");
            }
        }
        [HttpGet("card/{num}/delete")]
        public IActionResult DeleteCard(int num)
        {
            Message messageToDelete = _context.Messages.First(m => m.MessageId == num);
            _context.Remove(messageToDelete);
            _context.SaveChanges();
            return RedirectToAction("Delete");
        }
        private string UploadedFile(MessageViewModel message)
        {
            string uniqueFileName = null;
            if (message.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images"); 
                uniqueFileName = Guid.NewGuid().ToString() + "_" + message.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))  
                {  
                    message.ProfileImage.CopyTo(fileStream);  
                }
            }
            Console.WriteLine(uniqueFileName);
            return uniqueFileName;
        }
    }
}
