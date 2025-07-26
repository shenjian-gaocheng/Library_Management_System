namespace backend.DTOs
{
    /**
     * Reader 数据传输对象
     */
    public class ReaderDto
    {
        public string ReaderID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int CreditScore { get; set; }
        public string ReaderType { get; set; }
        public string AccountStatus { get; set; }
        public string Permission { get; set; }
    }
}
