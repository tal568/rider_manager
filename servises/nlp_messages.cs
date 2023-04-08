using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using RideModel_WebApi1;
namespace rider_manager;

public interface INlpMessages
{
    global::System.Single[] Confidnes { get; set; }
    global::System.Single Label { get; set; }

    void NlpLoadMessage(global::System.String message);
}

public class NlpMessages : INlpMessages
{
    public float[] Confidnes { get; set; }
    public float Label { get; set; } = 0;
    private readonly ILogger<NlpMessages> _log;

#pragma warning disable CS8618 // Non-nullable property 'Confidnes' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public NlpMessages(ILogger<NlpMessages> log)
#pragma warning restore CS8618 // Non-nullable property 'Confidnes' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    {
        _log = log;
    }

    public void NlpLoadMessage(string message)
    {
        var sampleData = new RideModel.ModelInput()
        {
            Col0 = message
        };

        //Load model and predict output
        var result = RideModel.Predict(sampleData);
        Confidnes = new float[result.Score.Length];
        this.Confidnes = result.Score;


    }
    public static void TrainNlp(IList<IWebElement> messages)
    {
        using (StreamWriter writer = new StreamWriter(@".\saved_text_nlp\messages.txt"))
        {
            foreach (IWebElement message in messages)
            {
                writer.WriteLine(message.Text.Replace('\n', ' ').Replace(',', '.'));
            }
        }
    }
    public static void SaveMassages(string txt)
    {
        using StreamWriter writer = new StreamWriter(@".\saved_text_nlp\messages_saved.txt");


        writer.WriteLine(txt.Replace('\n', ' '));
    }
}

