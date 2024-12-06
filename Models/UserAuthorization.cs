using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblUserAuthorization")]
    public class UserAuthorization
    {
        // ID người dùng
        [Key]
        [Column("iUserID")]
        public int UserID { get; set; }

        // ID chức năng 
        [Column("iFunctionID")]
        public int FunctionID { get; set; }

        // Ghi chú
        [Column("sNote")]
        public string Note { get; set; }
    }
}
