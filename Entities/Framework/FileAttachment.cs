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
    }
}
