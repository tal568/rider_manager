using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using RideModel_WebApi1;
using rider_manager.Interfaces;

namespace rider_manager;



public class NlpPredict : INlpPredict
{
    public float[]? Confidnes { get; set; }
    public float Label { get; set; }

    public NlpPredict()
    {
        Confidnes = null;
        Label=0;
    }

    public void NlpPredictText(string message)
    {
        var sampleData = new RideModel.ModelInput()
        {
            Col0 = message
        };

        //Load model and predict output
        var result = RideModel.Predict(sampleData);
        Confidnes = new float[result.Score.Length];
        this.Label = result.PredictedLabel;
        this.Confidnes = result.Score;


    }

}

