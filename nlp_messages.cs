using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using RideModel;
namespace rider_manager
{
    public class NlpMessages
    {
        private float[] confidnes;
        public float[] Confidnes
        {
            get { return confidnes; }
            set { confidnes = value; }
        }
        private float label =0;
        public float Label
        {
            get { return label; }
            set { label = value; }

        }
        public NlpMessages(string message)
        {
            var sampleData = new GoodRideModel.ModelInput()
            {
                Col0 = message
            };
        


            //Load model and predict output
            var result = GoodRideModel.Predict(sampleData);
            confidnes = new float[result.Score.Length];
            for (int i = 0; i < result.Score.Length; i++)
            {
                if (result.Score.Length - 1 != i)
                {
                   this.confidnes[i]=result.Score[i + 1];

                }
                else
                {
                   this.confidnes[3] =result.Score[0];

                }

            }
            this.label = result.PredictedLabel;
           
            
        }
    }
    
}
