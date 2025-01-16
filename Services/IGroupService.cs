using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Services
{
    public interface IGroupService
    {
        Group GetGroupById(int groupId);
        void UpdateLeader(int groupId, int newLeaderId);
    }
}
