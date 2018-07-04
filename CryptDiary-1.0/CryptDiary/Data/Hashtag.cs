using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptDiary.Data
{
    public class Hashtag
    {
        public string Text { get; set; }

        public Hashtag(string text)
        {
            this.Text = text;
        }
        
        /// <summary>
        /// searches for hashtagStrings beginning with "#" and adds them to a List
        /// </summary>
        /// <param name="text">the text to search for Hashtags</param>
        /// <returns></returns>
        static public List<Hashtag> GetHashtags(string text)
        {
            // check arguments
            if (text == null || text == "")
            {
                return new List<Hashtag>();
            }

            // make all lowercase
            text = text.ToLower();

            // get words
            string[] words = text.Split(new char[]{' ','\n'});

            // get words starting with '#'
            List<string> hashtagStrings = new List<string>();
            foreach (string word in words)
            {
                if (word != null && word.Length > 0)
                {
                    if (word[0] == '#' /*&& word.Length >= 2*/) // prevents "# " resulting in empty hashtags
                    {
                        hashtagStrings.Add(word.Substring(1));
                    }
                }
            }

            List<Hashtag> hashtags = new List<Hashtag>();
            for (int i = 0; i < hashtagStrings.Count; i++)
            {
                // reduce to alphanumeric
                for (int j = 0; j < hashtagStrings[i].Length; j++)
                {
                    if (!char.IsLetterOrDigit(hashtagStrings[i][j]))
                    {
                        hashtagStrings[i] = hashtagStrings[i].Remove(j, 1);
                        j--;
                    }
                }

                // prevent doubles and empty hashtags
                bool hashtagalreadyexists = false;
                foreach (Hashtag hashtag in hashtags)
                {
                    if (hashtag.Text == hashtagStrings[i])
                    {
                        hashtagalreadyexists = true;
                        break;
                    }
                }

                if (!hashtagalreadyexists && hashtagStrings[i].Length >= 1)
                {
                    hashtags.Add(new Hashtag(hashtagStrings[i]));
                }
            }

            return hashtags;
        }

        /// <summary>
        /// Adds "today's" hashtags to the dictionary respectively deletes orphaned ones 
        /// </summary>
        /// <param name="allHashtags">List of hashtags of all DiaryEntries</param>
        /// <param name="TodayHashtags">List of hashtags of particular DiaryEntry</param>
        /// <returns></returns>
        static public HashtagDictionary EditHashtagsDictionary(HashtagDictionary allHashtags, DiaryEntry diaryEntry)
        {
            // check today's date in global dictionary, if it contains hashtags that doesn't exist in today's DiaryEntry and remove the dates
            List<Hashtag> todaysHashtagsInGlobalDictionary = new List<Hashtag>();
            foreach (var dictionaryEntry in allHashtags)
            {
                // every time today's date appears, add the corresponding hashtag to the list
                if (dictionaryEntry.Value.Contains(diaryEntry.Date))
                {
                    todaysHashtagsInGlobalDictionary.Add(dictionaryEntry.Key);
                }
            }

            // now we have a list of hashtags of today's date in the global hashtag dictionary
            // let's see if these hashtags actually appear in today's diaryEntry
            foreach (Hashtag hashtag in todaysHashtagsInGlobalDictionary)
            {
                // if it doesn't appear in today's DiaryEntry, remove today's date from global dictionary entry of this hashtag
                if (!diaryEntry.Hashtags.Contains(hashtag))
                {
                    allHashtags[hashtag].Remove(diaryEntry.Date);
                }
                // check, if there are hashtag entries without a date and delete them
                if (allHashtags[hashtag].Count == 0)
                {
                    allHashtags.Remove(hashtag);
                }
            }

            // now orphaned hashtags should be gone

            // we can now add today's hashtags to the global diary
            foreach (Hashtag hashtag in diaryEntry.Hashtags)
            {
                // if hashtag exists in global dictionary, add today's date to this entry, if it doesn't exist already
                if (allHashtags.ContainsKey(hashtag))
                {
                    if (!allHashtags[hashtag].Contains(diaryEntry.Date))
                    {
                        allHashtags[hashtag].Add(diaryEntry.Date);
                    }
                }
                // if hashtag doesn't exist in global dictionary, add the hashtag and the date
                else
                {
                    allHashtags.Add(hashtag, new List<DateTime>() { diaryEntry.Date });
                }
            }

            return allHashtags;
        }

        /// <summary>
        /// compare Hashtag objects based on their Text properties
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Hashtag compareHashtag = obj as Hashtag;
            return (this.Text == compareHashtag.Text);
        }

        // overload operators == and !=
        static public bool operator ==(Hashtag hashtag1, Hashtag hashtag2)
        {
            return hashtag1.Equals(hashtag2);
        }

        static public bool operator !=(Hashtag hashtag1, Hashtag hashtag2)
        {
            return !hashtag1.Equals(hashtag2);
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
