using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework
{
    public class FileAttachment : BaseEntity
    {
        public string Title { get; set; }
        public byte[] ContentBytes { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public int Key { get; set; }
        public long Size { get; set; }
        public string TypeName { get; set; }
        [ForeignKey("FileAttachmentTypeId")]
        public FileAttachmentType FileAttachmentType { get; set; }
        public int FileAttachmentTypeId { get; set; }
    }

    public class FileAttachmentType : BaseEntity
    {
        public string TypeTitle { get; set; }
        public bool IsBaseType { get; set; }
        public FileAttachmentTypeEnum AttachmentTypeEnum { get; set; }
    }

    public enum FileAttachmentTypeEnum
    {
        UserAvatar,
        CardMeli,
        ShenasNameh,
        Passport,
        Contract,
        UserDefined
    }
}
