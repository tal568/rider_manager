using OpenQA.Selenium;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML;

namespace whatapp_ride_joiner
{
    public class ride_manger
    {
        public static void Main(string[] args)
        {
            /*
            MyWebDriver webdriver = new MyWebDriver();
            webdriver.choose_group("אבא");
            IList<IWebElement> messages = webdriver.GetMassges();
            webdriver.Close();
            //Load sample data
            */
            //Load sample data
            var sampleData = new Ride_choser_model.ModelInput()
            {
                Col0 = @"מישהו יוצא ב17:00",
            };

            //Load model and predict output
            var result = Ride_choser_model.Predict(sampleData);
            for (int i=0; i<result.Score.Length;i++)
             {
                if (result.Score.Length - 1 != i)
                {
                    Console.WriteLine("for the value " + (i) + " the % is " + result.Score[i+1]);

                }
                else
                {
                    Console.WriteLine("for the value " + 3  + " the % is " + result.Score[0]);

                }

            }
            Console.WriteLine("the predictedlabel is " + result.PredictedLabel);


        }




        public static void train_nlp(IList<IWebElement> messages)
        {
            using (StreamWriter writer = new StreamWriter(@"C:\Users\tal\source\repos\whatapp_driver\messages.txt"))
            {
                foreach (IWebElement message in messages)
                {
                    writer.WriteLine(message.Text.Replace('\n', ' '));
                }
            }
        }
    }
}