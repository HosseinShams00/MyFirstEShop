using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Attributes;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Controllers
{
    [Authorize]
    [TypeFilter(typeof(CheckUserSecurityStampAttribute))]
    public class TicketController : Controller
    {
        private readonly ITicketRepository ticketRepository;

        public int UserId
        {
            get
            {
                return int.Parse(User.FindFirst("UserId").Value);
            }

        }




        public TicketController(ITicketRepository _ticketRepository)
        {
            ticketRepository = _ticketRepository;
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitNewTicket(TicketViewModel ticketViewModel)
        {
            ticketRepository.SetNewTicket(ticketViewModel, UserId);
            return RedirectToAction("Index", "Setting");
        }

        public IActionResult ReadTicket(int MID)
        {
            var Ticket = ticketRepository.ReadTicket(UserId, MID);
            return View(Ticket);
        }

    }
}
