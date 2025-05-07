using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebNC_BTL_QLCV.Models
{
    public class GroupTaskCreateViewModel
    {
        public GroupTask GroupTask { get; set; }
        public IEnumerable<SelectListItem> GroupMembers { get; set; }
    }
}
