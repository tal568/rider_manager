namespace rider_manager
{
    public interface INlpPredict
    {
        float[]? Confidnes { get; set; }
        float Label { get; set; }
        void NlpPredictText(string message);

    }
}