using WebNC_BTL_QLCV.Models;
using WebNC_BTL_QLCV.Repositories;
using WebNC_BTL_QLCV.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    // Lấy thông tin nhóm
    public Group GetGroupById(int groupId)
    {
        return _groupRepository.GetGroupById(groupId);
    }

    // Cập nhật trưởng nhóm
    public void UpdateLeader(int groupId, int newLeaderId)
    {
        _groupRepository.UpdateLeader(groupId, newLeaderId);
    }
}
