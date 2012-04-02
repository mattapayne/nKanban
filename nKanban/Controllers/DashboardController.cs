using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using nKanban.Models;
using nKanban.Services;

namespace nKanban.Controllers
{
    [Authorize]
    public class DashboardController : AbstractBaseController
    {
        private readonly IUserService _userService;
        private readonly IKanbanBoardService _kanbanBoardService;

        public DashboardController(IUserService userService, IKanbanBoardService kanbanBoardService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            if (kanbanBoardService == null)
            {
                throw new ArgumentNullException("kanbanBoardService");
            }

            _userService = userService;
            _kanbanBoardService = kanbanBoardService;
        }

        public ActionResult Show()
        {
            var kanbanBoards = _kanbanBoardService.GetKanbanBoardsByUser(CurrentUser.Id);
            var models = kanbanBoards.Select(Mapper.Map<KanbanBoardViewModel>);

            var model = new DashboardViewModel() { KanbanBoards = models };

            return View(model);
        }
    }
}
