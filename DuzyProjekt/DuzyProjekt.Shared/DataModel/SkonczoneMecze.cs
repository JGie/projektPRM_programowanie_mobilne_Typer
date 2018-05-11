using System;
using System.Collections.Generic;
using System.Text;
using Windows.Web.Http;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Schema;

namespace DuzyProjekt.DataModel
{
    public class SkonczoneMecze
    {
        public SkonczoneMecze(String data, String d_gospodarzy, String d_gosci, String wynik)
        {
            this.Data = data;
            this.D_gospodarzy = d_gospodarzy;
            this.D_gosci = d_gosci;
            this.Wynik = wynik;
        }

        public string Data { get; private set; }

        public string D_gospodarzy { get; private set; }

        public string D_gosci { get; private set; }

        public string Wynik { get; private set; }


        public static async Task<object> GetGroupsAsync()
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.kimonolabs.com/api/5271o3da?apikey=EeR0aBnyfuydMZiQb6kbSDRPWMTCZcqr");


                var response = await request.GetResponseAsync().ConfigureAwait(false);
                var stream = response.GetResponseStream();

                var streamReader = new StreamReader(stream);
                string responseText = streamReader.ReadToEnd();



                responseText = Regex.Replace(responseText,@"\s:\s","-");

                string strRegex = @"\[[\x20-\x7E\n\r\S]+\]";
                Regex myRegex = new Regex(strRegex, RegexOptions.None);

                Match match = myRegex.Match(responseText);

                SkonczoneMecze[] skonczoneObiekty = JsonConvert.DeserializeObject<SkonczoneMecze[]>(match.Value);



                foreach (var row in skonczoneObiekty)
                {
                    Debug.WriteLine(row.Wynik);


                }


                return skonczoneObiekty;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
