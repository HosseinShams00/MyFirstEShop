using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyFirstEShop.Repositories;
using MyFirstEShop.Models.ViewModels;
using System.Collections.Generic;

namespace MyFirstEShop.Component
{
    public class SendTicketViewComponent : ViewComponent
    {
        private readonly ITicketRepository ticketRepository;
        public SendTicketViewComponent( ITicketRepository _ticketRepository)
        {
            ticketRepository = _ticketRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Views/ViewComponent/UserSetting/Ticket.cshtml", ticketRepository.GetAllTickets(int.Parse(UserClaimsPrincipal.FindFirst("UserID").Value)));
        }

    }
}
