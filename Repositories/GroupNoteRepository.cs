﻿using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public class GroupNoteRepository : IGroupNoteRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupNoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // lấy ra toàn bộ danh sách ghi chú
        public IEnumerable<GroupNote> GetAllGroupNote()
        {
            return _context.GroupNotes.ToList();
        }

        // lấy ra danh sách ghi chú cá nhân theo id
        public GroupNote GetGroupNoteById(int id)
        {
            return _context.GroupNotes.Find(id);
        }

        //lấy ra danh sách ghi chú theo id công việc
        public IEnumerable<GroupNote> GetGroupNoteByGroupTaskID(int GroupTaskId)
        {
            return _context.GroupNotes.Where(t => t.GroupTaskID == GroupTaskId).ToList();
        }

        // thêm mới 1 ghi chú
        public void AddGroupNote(GroupNote GroupNote)
        {
            _context.GroupNotes.Add(GroupNote);
            _context.SaveChanges();
        }

        // sửa 1 ghi chú
        public void UpdateGroupNote(GroupNote groupNote)
        {
            var existingNote = _context.GroupNotes.Find(groupNote.GroupNoteID);

            // cập nhật các trường cần thiết
            existingNote.GroupNoteDetails = groupNote.GroupNoteDetails;
            //_context.GroupNote.Update(GroupNote);

            // đánh dấu trường cập nhật
            _context.Entry(existingNote).Property(x => x.GroupNoteDetails).IsModified = true;

            // các trường không cập nhật
            _context.Entry(existingNote).Property(x => x.GroupNote_CreationDate).IsModified = false;

            _context.SaveChanges();
        }

        // xóa 1 ghi chú
        public void DeleteGroupNote(int id)
        {
            var GroupNote = GetGroupNoteById(id);
            if (GroupNote != null)
            {
                _context.GroupNotes.Remove(GroupNote);
                _context.SaveChanges();
            }
        }
    }
}
