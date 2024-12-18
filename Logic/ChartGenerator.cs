namespace Logic;
public class ChartGenerator
{
    private List<Loner> userTotals;
    private List<string> xData;
    private List<double> yData;
    
    public ChartGenerator()
    {
        userTotals = new List<Loner>();
        xData = new List<string>();
        yData = new List<double>();
    }
    
    public byte[] generateImage(List<Loner> loners)
    {
        prepareData(loners);
        prepareChartData();
        
    }

    private void prepareData(List<Loner> loners)
    {
        foreach (Loner loner in loners)
        {
            ProcessLoner(loner);
        }
        
    }
    private void ProcessLoner(Loner loner)
    {
        int? LonerIndexOfExistingLoner = FindExistingLoner(loner.DiscordUuid);
        if (LonerIndexOfExistingLoner != null)
        {
            int index = LonerIndexOfExistingLoner.Value;
            updateTimeOfUserTotals(index, loner);
        }else{
            userTotals.Add(loner);
        }
    }
    private int? FindExistingLoner(string discordUuid)
    {
        foreach (Loner loner in userTotals)
        {
            if (loner.DiscordUuid == discordUuid)
                return userTotals.IndexOf(loner);
        }
        return null;
    }

    private void updateTimeOfUserTotals(int index, Loner loner)
    {
        userTotals[index].AloneInMillis += loner.AloneInMillis;
    }

    private void prepareChartData()
    {
        foreach (Loner loner in userTotals)
        {
            xData.Add(loner.DiscordUsername);
            yData.Add(loner.AloneInMillis / (60000)); // convert to minutes
        }
    }
    
}