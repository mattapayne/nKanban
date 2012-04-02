using System.ComponentModel.DataAnnotations;

namespace nKanban.Models
{
    public class CreateKanbanBoardViewModel
    {
        [Required(ErrorMessage = "A name is required.")]
        [Display(Name = "Name")]
        [StringLength(255)]
        public string Name { get; set; }
    }
}