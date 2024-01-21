namespace Logic;

public class Quote
{
    public string text { get; set; }
    public string person { get; set; }
    public DateTime DateTimeCreated { get; set; }

    public override string ToString()
    {
        return text + " - " + person + " " + DateTimeCreated.ToString("dd/M/yyyy HH:mm");
    }
}