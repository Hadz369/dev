using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptDiary.Data
{
    public struct DiaryEntryStatistics
    {
        public DateTime DateAndTime {get; set;}
        public int NumberOfChars { get; set; }
        public int NumberOfWords { get; set; }
        public int NumberOfParagraphs { get; set; }
        public int MinutesOfEditing { get; set; }
        public int MinutesOfReading { get; set; }
    }
}
