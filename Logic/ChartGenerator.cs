using System.Diagnostics;
using Newtonsoft.Json;
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
        // Create the Node.js script for chart generation
        string chartScript = CreateChartScript(xData, yData);

        // Execute the Node.js script and capture the chart image
        return RunNodeScript(chartScript);
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
      private string CreateChartScript(List<string> xData, List<double> yData)
    {
        // Generate a Node.js script string with Chart.js code
        return $@"
const {"ChartJSNodeCanvas"} = require('chartjs-node-canvas');
const fs = require('fs');

(async () => {{
    const width = 800;
    const height = 600;
    const chartJSNodeCanvas = new ChartJSNodeCanvas({{ width, height }});

    const configuration = {{
        type: 'bar',
        data: {{
            labels: {JsonConvert.SerializeObject(xData)},
            datasets: [{{
                label: 'Time Alone (minutes)',
                data: {JsonConvert.SerializeObject(yData)},
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }}]
        }},
        options: {{
            scales: {{
                y: {{
                    beginAtZero: true
                }}
            }}
        }}
    }};

    const image = await chartJSNodeCanvas.renderToBuffer(configuration);
    fs.writeFileSync('chart.png', image);
    console.log('Chart saved as chart.png');
}})();
";
    }

    private byte[] RunNodeScript(string script)
    {
        string tempScriptPath = Path.Combine(Path.GetTempPath(), "generate_chart.js");
        File.WriteAllText(tempScriptPath, script);

        // Execute the Node.js script
        var processInfo = new ProcessStartInfo("node", tempScriptPath)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(processInfo);
        process.WaitForExit();

        // Read the generated image file
        string chartPath = Path.Combine(Path.GetTempPath(), "chart.png");
        byte[] imageData = File.ReadAllBytes(chartPath);

        // Clean up temporary files
        File.Delete(tempScriptPath);
        File.Delete(chartPath);

        return imageData;
    }
}