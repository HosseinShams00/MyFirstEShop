using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Areas.Admin.Models.ViewModel;
using MyFirstEShop.Models;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Repositories;
using Microsoft.AspNetCore.Authorization;
using MyFirstEShop.Attributes;

namespace MyFirstEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [TypeFilter(typeof(CheckUserSecurityStampAttribute))]
    [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanSeeTicket })]
    public class TicketManagementController : Controller
    {
        public int AdminId
        {
            get
            {
                return int.Parse(User.FindFirst("UserId").Value);
            }
        }

        private readonly ITicketRepository ticketRepository;
        public TicketManagementController(ITicketRepository _ticketRepository)
        {
            ticketRepository = _ticketRepository;
        }

        public IActionResult Index()
        {
            var tickets = ticketRepository.GetAllTicketsForAdmin();
            return View(tickets);
        }

        public IActionResult ReadTickets(int userId, int MessageId)
        {
            var ticket = ticketRepository.ReadTicket(userId, MessageId);
            return View(ticket);
        }

        [HttpPost]
        public IActionResult SetAnswer(TicketViewModel ticketViewModel)
        {
            ticketRepository.SetAnswerForTicket(ticketViewModel.UserId, AdminId, ticketViewModel.MessageId, ticketViewModel.AdminAnswer);
            return RedirectToAction("Index");
        }

    }
}
