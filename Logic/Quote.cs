using System;

namespace Logic;

public class Quote
{
    public string? id { get; set; }
    public string text { get; set; }
    public string person { get; set; }
    public string? Context { get; set; }
    public DateTime DateTimeCreated { get; set; }

    public override string ToString()
    {
        return text + " - " + person + " " + DateTimeCreated.ToString("dd/MM/yyyy HH:mm");
    }
}