using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Schema;
using System.Net;
using System.Linq;

namespace DuzyProjekt.DataModel
{


    public class PrzyszleMecze
    {
        public PrzyszleMecze(String data, String d_gospodarzy, String d_gosci)
        {
            this.Data = data;
            this.D_gospodarzy = d_gospodarzy;
            this.D_gosci = d_gosci;
            this.Obstawiony = false;

        }

        public string Data { get; set; }
        public string D_gospodarzy { get; set; }
        public string D_gosci { get; set; }
        public bool Obstawiony { get; set; }

        public override string ToString()
        {
            return this.D_gospodarzy;
        }
    }

    public sealed class PrzyszleMeczeSource
    {
        private static PrzyszleMeczeSource _przyszleMeczeSource = new PrzyszleMeczeSource();

        private ObservableCollection<PrzyszleMecze> _grupa = new ObservableCollection<PrzyszleMecze>();
        public ObservableCollection<PrzyszleMecze> Grupa
        {
            get { return this._grupa; }
        }

        public static async Task<IEnumerable<PrzyszleMecze>> GetGroupsAsync()
        {
            await _przyszleMeczeSource.GetPrzyszleMeczeAsyn();

            return _przyszleMeczeSource.Grupa;
        }

        public static async Task<PrzyszleMecze> GetGroupAsync(string gospodarz)
        {
            await _przyszleMeczeSource.GetPrzyszleMeczeAsyn();
            var matches = _przyszleMeczeSource.Grupa.Where((group) => group.D_gospodarzy.Equals(gospodarz));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetPrzyszleMeczeAsyn()
        {

            if (_grupa.Count != 0)
                return;

 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.kimonolabs.com/api/3b5vmnp0?apikey=EeR0aBnyfuydMZiQb6kbSDRPWMTCZcqr");


                var response = await request.GetResponseAsync().ConfigureAwait(false);
                var stream = response.GetResponseStream();

                var streamReader = new StreamReader(stream);
                string responseText = streamReader.ReadToEnd();

                string strRegex = @"\[[\x20-\x7E\n\r\S]+\]";
                Regex myRegex = new Regex(strRegex, RegexOptions.None);

                Match match = myRegex.Match(responseText);

                PrzyszleMecze[] przyszleObiekty = JsonConvert.DeserializeObject<PrzyszleMecze[]>(match.Value);


                for (int i = 0; i < 8;i++ )
                {
                    var a = przyszleObiekty[i];
                    this.Grupa.Add(a);
                }


            }

    }
    }








