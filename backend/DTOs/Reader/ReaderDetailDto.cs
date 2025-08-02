namespace backend.DTOs.Reader
{
    public class ReaderDetailDto
    {
        public string UserName { get; set; }

        public string? FullName { get; set; }

        public string? NickName { get; set; }

        public string? Avatar { get; set; }

        public int CreditScore { get; set; }

        public string AccountStatus { get; set; }

        public string Permission { get; set; }
    }
}
