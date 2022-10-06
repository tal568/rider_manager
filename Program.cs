using OpenQA.Selenium;
using rider_manager;
using System.Collections.Generic;
using System.IO;
using RideModel;
namespace whatapp_ride_joiner
{
    public class ride_manger
    {
        public static async Task Main(string[] args)
        {

            MyWebDriver webdriver = new MyWebDriver();
            webdriver.choose_group("אבא");
                  var timer = new PeriodicTimer(TimeSpan.FromSeconds(50));

                  while (await timer.WaitForNextTickAsync())
                  {
                      IList<IWebElement> messages = webdriver.GetMassges();


                      IWebElement? best_message = FilterMessagesNlp(messages);


                      if (best_message != null)
                      {
                          webdriver.Replay(best_message);
                        //  timer.Dispose();

                     //     webdriver.Close();
                      }
                      Console.WriteLine("alive");
                   


                  }
         //  Console.WriteLine("with to load");
         //  Console.ReadLine();
         //  IList<IWebElement> messages = webdriver.GetMassges();
         //  TrainNlp(messages);
        }




            public static IWebElement? FilterMessagesNlp(IList<IWebElement> messages)
            {
            float best_ride = 0.65f;
            IWebElement best_message=null;
            foreach (IWebElement message in messages) 
            {
                NlpMessages predicted_message = new NlpMessages(message.Text);
                if (predicted_message.Label==2 && predicted_message.Confidnes[2]> best_ride) 
                {
                    best_message=message;
                    best_ride=predicted_message.Confidnes[3];
                }
                string save = "for: " + message.Text + "\n % was: " + String.Join($"\n",predicted_message.Confidnes);
                save_messages(save);
            }
            return best_message ;
        }



        public static void TrainNlp(IList<IWebElement> messages)
        {
            using (StreamWriter writer = new StreamWriter(@".\saved_text_nlp\messages.txt"))
            {
                foreach (IWebElement message in messages)
                {
                    writer.WriteLine(message.Text.Replace('\n', ' ').Replace(',','.'));
                }
            }
        }
        public static void save_messages(string txt)
        {
            using (StreamWriter writer = new StreamWriter(@".\saved_text_nlp\messages_saved.txt"))
            {
                
                
                    writer.WriteLine(txt.Replace('\n', ' '));
                
            }
        }
    }
}