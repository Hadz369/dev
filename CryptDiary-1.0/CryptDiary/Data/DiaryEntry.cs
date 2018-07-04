using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptDiary.Data
{
    public class DiaryEntry
    {
        public DateTime Date { get; set; }
        // at text set scan for Hashtags
        private string text;
        public string Text 
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                Hashtags = Hashtag.GetHashtags(text);
            }
        }
        public DiaryEntryStatistics Statistics { get; set; }
        public List<Hashtag> Hashtags { get; set; }

        public DiaryEntry() { }

        public DiaryEntry(DateTime date, string text)
        {
            this.Date = date;
            this.Text = text;
        }

        public void SaveHashtags()
        {

        }
    }
}
