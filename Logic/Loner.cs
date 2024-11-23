namespace Logic;

public class Loner
{
    private string id { get; set; }
    public string DiscordUuid { get; set; }
    public string DiscordUsername { get; set; }
    public string DiscordDiscriminator { get; set; }
    public DateTime StartTimeAlone { get; set; }
    public DateTime EndTimeAlone { get; set; }
    public string DiscordVoiceChannelId { get; set; }
    public string DiscordVoiceChannelName { get; set; }
    private long AloneInMillis { get; set; }
    
    public Loner(){}
    public Loner(string id)
    {
        this.id = id;
    }
    public void CalculateAloneDuration()
    {
        if (EndTimeAlone == DateTime.MinValue)
        {
            EndTimeAlone = DateTime.Now;
        }
        AloneInMillis= (long)EndTimeAlone.Subtract(StartTimeAlone).TotalMilliseconds;
    }
    public long GetAloneDurationInMillis()
    {
        return AloneInMillis;
    }

    public string GetId()
    {
        return id;
    }
}