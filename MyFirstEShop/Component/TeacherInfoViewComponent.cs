using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Component
{
    public class TeacherInfoViewComponent : ViewComponent
    {
        private readonly ITeacherRepository teacherRepository;
        public TeacherInfoViewComponent(ITeacherRepository _teacherRepository)
        {
            teacherRepository = _teacherRepository ;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = int.Parse(UserClaimsPrincipal.FindFirst("UserId").Value);

            var teacher = teacherRepository.GetTeacherWithProducts(userId);

            return View("~/Views/ViewComponent/TeacherPanel/TeacherInfoView.cshtml", teacher);
        }
    }
}
