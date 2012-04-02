using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using nKanban.Domain;
using nKanban.Models;
using nKanban.Services;

namespace nKanban.Controllers
{
    [Authorize]
    public class KanbanBoardController : AbstractBaseController
    {
        private readonly IKanbanBoardService _kanbanBoardService;

        public KanbanBoardController(IKanbanBoardService kanbanBoardService)
        {
            if(kanbanBoardService == null)
            {
                throw new ArgumentNullException("kanbanBoardService");
            }

            _kanbanBoardService = kanbanBoardService;
        }

        public ActionResult New()
        {
            var model = new CreateKanbanBoardViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateKanbanBoardViewModel model)
        {
            if(ModelState.IsValid)
            {
                var board = Mapper.Map<KanbanBoard>(model);
                var errors = _kanbanBoardService.CreateKanbanBoard(CurrentUser.Id, board);

                if(!errors.Any())
                {
                    return RedirectToRoute("Dashboard");
                }

                AddServiceErrors(errors);
            }

            return View("New", model);
        }
    }
}
