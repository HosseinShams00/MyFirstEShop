using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Areas.Admin.Models.ViewModel;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Areas.Admin.Components
{
    public class HomeMenuViewComponent : ViewComponent
    {
        private readonly IProductRepository productRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly IUserAccessRepository userAccessRepository;
        public HomeMenuViewComponent(IProductRepository _productRepository, ITicketRepository _ticketRepository, IUserAccessRepository _userAccessRepository)
        {
            productRepository = _productRepository;
            ticketRepository = _ticketRepository;
            userAccessRepository = _userAccessRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = new HomeMenuViewModel()
            {
                SuspendProductsCount = productRepository.GetSuspendProductsCount(),
                TicketsCount = ticketRepository.GetTicketCount(),
                AdminAccess = userAccessRepository.GetUserAccess(int.Parse(UserClaimsPrincipal.FindFirst("UserId").Value))
            };

            return View("~/Areas/Admin/Views/Home/HomeMenuComponentView.cshtml", viewModel);
        }

    }
}
