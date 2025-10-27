namespace Money_Box;

public class DeepLinkMessage
{
    public string Target { get; set; }
    public string Folio { get; set; }

    public DeepLinkMessage(string target, string folio)
    {
        Target = target;
        Folio = folio;
    }
}
