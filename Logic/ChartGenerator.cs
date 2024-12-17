using Microsoft.FSharp.Collections;

namespace Logic;
using Plotly.NET;
public class ChartGenerator
{
    private List<Loner> userTotals;
    private List<string> xData;
    private List<double> yData;
    public ChartGenerator()
    {
        userTotals = new List<Loner>();
    }
    
    public object generateImage(List<Loner> loners)
    {
        prepareData(loners);
        foreach (Loner loner in userTotals)
        {
            xData.Add(loner.DiscordUsername);
            double Minutes = loner.AloneInMillis / 60000;
            yData.Add(Minutes);
        }   
    }

    private void prepareData(List<Loner> loners)
    {
        foreach (Loner loner in loners)
        {
            int? LonerIndexOfExistingLoner = FindExistingLoner(loner.DiscordUuid);
            if (LonerIndexOfExistingLoner != null)
            {
                int index = LonerIndexOfExistingLoner.Value;
                updateTimeOfUserTotals(index, loner);
            }
            
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
}