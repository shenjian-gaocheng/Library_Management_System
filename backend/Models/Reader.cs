


namespace backend.Models;

public class Reader: User
{
    public string ReaderID { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public int? CreditScore { get; set; }

    public string ReaderType { get; set; }

    public string AccountStatus { get; set; }

    public string Permission { get; set; } = "ÆÕÍ¨";

    public string UserName
    {
        get { return ReaderID; }
        set { ReaderID = value; }
    }
}