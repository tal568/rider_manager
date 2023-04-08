using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rider_manager.Interfaces;

namespace rider_manager.servises
{
    internal class NlpTrain:INlpTrain
    {
        public NlpTrain()
        {

        }
        public  void SaveMassages(string txt)
        {
            using StreamWriter writer = new StreamWriter(@".\saved_text_nlp\messages_saved.txt");


            writer.WriteLine(txt.Replace('\n', ' '));
        }
    }
}
