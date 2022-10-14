using OpenQA.Selenium;
using RideModel;
namespace rider_manager
{
    public class NlpMessages
    {
        public float[] Confidnes { get; set; }

        public float Label { get; set; } = 0;
        public NlpMessages(string message)
        {
            var sampleData = new GoodRideModel.ModelInput()
            {
                Col0 = message
            };



            //Load model and predict output
            var result = GoodRideModel.Predict(sampleData);
            Confidnes = new float[result.Score.Length];
            for (int i = 0; i < result.Score.Length; i++)
            {
                if (result.Score.Length - 1 != i)
                {
                    this.Confidnes[i] = result.Score[i + 1];

                }
                else
                {
                    this.Confidnes[3] = result.Score[0];

                }

            }
            this.Label = result.PredictedLabel;


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

}
