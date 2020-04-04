using Dapper.Contrib.Extensions;

namespace API.Models
{
    [Table("Shr_FileAttachment")]
    public class FileAttachment
    {
        public long Id { get; set; }
        public byte[] Content { get; set; }
        public int Size { get; set; }
    }
}
