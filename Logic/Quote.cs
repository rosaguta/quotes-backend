using System;

namespace Logic;

public class Quote
{
    public string? id { get; set; }
    public string text { get; set; }
    public string person { get; set; }
    public string? Context { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public string? GeneratedQuote
    {
        get
        {
            return this.ToString();
        } 
    }

    
    public override string ToString()
    {
        return text + " - " + person + " " + ToTimeZoneTime(DateTimeCreated, "W. Europe Standard Time").ToString("dd/MM/yyyy HH:mm");
    }

    public string ToStringWithContext()
    {
        if (Context is null)
        {
            Context = "";
        }
        return text + " - " + person + " " + ToTimeZoneTime(DateTimeCreated, "W. Europe Standard Time").ToString("dd/MM/yyyy HH:mm") + " | " + Context;
    }
    private static DateTime ToTimeZoneTime(DateTime time, string timeZoneId)
    {
        var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        DateTime utcTime;
        if (time.Kind == DateTimeKind.Local)
        {
            utcTime = time.ToUniversalTime();
        }
        else
        {
            utcTime = DateTime.SpecifyKind(time, DateTimeKind.Utc);
        }

        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
    }

    
}