using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptDiary.Data;
using System.Collections.Generic;

namespace CryptDiaryUnitTest
{
    [TestClass]
    public class HashtagTests
    {
        [TestMethod]
        public void GetHashtagsAllValid()
        {
            string SampleTextWith3Hashtags =
                "I am a #SampleText that Contains 3 #Hashtags and the last one has several ugly chars at its #end.!?)))()(";

            List<CryptDiary.Data.Hashtag> Hashtags = new List<Hashtag>();
            try
            {
                Hashtags = Hashtag.GetHashtags(SampleTextWith3Hashtags);
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }

            // Vergleich
            if (Hashtags.Count != 3)
            {
                Assert.Fail("Anzahl der Hashtags entspricht nicht den Erwartungen. Erwartet: 3, erhalten: " + Hashtags.Count.ToString());
            }

            if (Hashtags[0].Text != "sampletext" || Hashtags[1].Text != "hashtags" || Hashtags[2].Text != "end")
            {
                Assert.Fail("Falsche Hashtags bekommen: Erwartet: sampletext, hashtags, end\n" +
                    "bekommen: " + Hashtags[0].Text + ", " + Hashtags[1].Text + ", " + Hashtags[2].Text);
            }
        }

        [TestMethod]
        public void GetHashtagsEmptyText()
        {
            string EmptyText = "";

            List<CryptDiary.Data.Hashtag> Hashtags = new List<Hashtag>();
            try
            {
                Hashtags = Hashtag.GetHashtags(EmptyText);
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }

            // Vergleich
            if (Hashtags.Count != 0)
            {
                Assert.Fail("Anzahl der Hashtags entspricht nicht den Erwartungen. Erwartet: 3, erhalten: " + Hashtags.Count.ToString());
            }
        }

        [TestMethod]
        public void GetHashtagsWhiteSpaceAtTheEnd()
        {
            string SampleText = "#Hashtag ";

            List<CryptDiary.Data.Hashtag> Hashtags = new List<Hashtag>();
            try
            {
                Hashtags = Hashtag.GetHashtags(SampleText);
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }

            // Vergleich
            if (Hashtags.Count != 1)
            {
                Assert.Fail("Anzahl der Hashtags entspricht nicht den Erwartungen. Erwartet: 1, erhalten: " + Hashtags.Count.ToString());
            }

            if (Hashtags[0].Text != "hashtag")
            {
                Assert.Fail("Hashtag \"hashtag\" erwartet. Erhalten: \"" + Hashtags[0].Text + "\"");
            }
        }

        [TestMethod]
        public void GetHashtagsDoubleHashtags()
        {
            string SampleText = "#Hashtag #hashtag.";

            List<CryptDiary.Data.Hashtag> Hashtags = new List<Hashtag>();
            try
            {
                Hashtags = Hashtag.GetHashtags(SampleText);
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }

            // Vergleich
            if (Hashtags.Count != 1)
            {
                Assert.Fail("Anzahl der Hashtags entspricht nicht den Erwartungen. Erwartet: 1, erhalten: " + Hashtags.Count.ToString());
            }
        }

        [TestMethod]
        public void GetHashtagsLongText()
        {
            string SampleText;
            #region longText
            SampleText = @"Hashtags sind schnell gemacht: Einfach ein paar passende Schlagworte finden und mit dem #-Zeichen davor in der Bildbeschreibung oder nachträglich in den Kommentaren eintragen – fertig. Verwendet man bereits im Instagram-Netzwerk verwendete Schlagworte wie #frankfurt, #newyork oder #sunset, so wird das eigene Foto diesem Pool aus Bildern hinzugefügt. Kreiert man einen neuen Hashtag (beliebt sind hierbei emotionale Äußerungen wie #ickfreumirso oder #soeinscheisstag), so kann man sich geehrt fühlen, der Erste zu sein.

                So sehen korrekte Hashtags aus
                Bei der Schreibweise von Hashtags gibt es ein paar Dinge zu beachten. Die Länge ist egal, sie können kurz – also auch nur aus einem Zeichen bestehen – oder ausführlicher sein. Allerdings dürfen sie nur Buchstaben und Zahlen, keine Sonderzeichen wie %, &, $ oder § beinhalten. Da Instagram ein internationales Netzwerk ist, wird hier hauptsächlich in Englisch kommuniziert – so auch bei den Hashtags. Es kann aber nicht schaden, hin und wieder deutsche Begriffe einzustreuen.

                Hashtags enden bei einem Leerzeichen. Deswegen muss alles zusammen (zum Beispiel #newyork) oder mit einem Unterstrich getrennt (#new_york) geschrieben werden. Groß- und Kleinschreibung ist zu vernachlässigen, da Instagram nur Kleinschreibung berücksichtigt.

                Viel hilft nicht viel
                Es gibt zwar keine ausgewiesene Beschränkung für die Anzahl an Hashtags, zu viele sollte man aber nicht einbauen. Das sieht einerseits unschön aus und wirkt andererseits so, als könne man sein Foto nicht präzise beschreiben. Oder man wolle durch extrem viele Schlagworte unbedingt Aufmerksamkeit erregen. Wer Hashtags einsetzt, sollte also versuchen, so treffend wie möglich sein. Allgemeine Beschreibungen wie #instagram oder #photography mögen zwar stimmen, sie werden aber inflationär vergeben. Da geht das eigene Pixel-Kunstwerk schnell in der Masse unter.

                Je präziser der Hashtag also, desto genauer kann man sich mit Instagram-Fotografen mit den gleichen Interessen vernetzen. Auch Abwandlungen und nähere Angaben sind hier nützlich. Neben #rennsport empfiehlt es sich beispielsweise, zusätzlich #vettel und #formel1 anzugeben.

                Nicht zu anstößig bitte
                Man kann zwar frei Hashtags vergeben, aber manche werden trotzdem keine Vernetzung mit gleichartigen Fotos ergeben. Denn Instagram setzt einen Bad-Word-Filter ein, durch den bestimmte Begriffe wie #sexy oder #nude zu keinen Ergebnissen führen – egal, wie viele Bilder es in Wirklichkeit mit diesen Schlagworten gibt.";
            #endregion

            List<Hashtag> Hashtags = new List<Hashtag>();
            try
            {
                Hashtags = Hashtag.GetHashtags(SampleText);
                int count = Hashtags.Count;
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }
        }

        [TestMethod]
        public void EditHashtagsDictionaryAllValid()
        {
            CryptDiary.Data.DiaryEntry diaryEntry = new CryptDiary.Data.DiaryEntry(DateTime.Today, "#hashtag1 #hashtag2 #hashtag3");
            HashtagDictionary allHashtags = new HashtagDictionary();

            allHashtags.Add(new CryptDiary.Data.Hashtag("hashtag1"), new List<DateTime>() { DateTime.Today });
            allHashtags.Add(new CryptDiary.Data.Hashtag("hashtag2"), new List<DateTime>() { DateTime.Today.AddDays(1) });
            allHashtags.Add(new CryptDiary.Data.Hashtag("hashtag4"), new List<DateTime>() { DateTime.Today });
            
            Hashtag.EditHashtagsDictionary(allHashtags, diaryEntry);

            // 3 hashtags should be in dictionary
            if (allHashtags.Count != 3)
            {
                Assert.Fail("Falsche Größe des Hashtag-Dictionarys. Erwaretet: 3, tatsächlich: " + allHashtags.Count.ToString());
            }

            // now hashtag1, hashtag2 and hashtag3 should be in dictionary
            if (!allHashtags.ContainsKey(new CryptDiary.Data.Hashtag("hashtag1")))
            {
                Assert.Fail("hashtag1 kommt nicht vor.");
            }
            if (!allHashtags.ContainsKey(new CryptDiary.Data.Hashtag("hashtag2")))
            {
                Assert.Fail("hashtag2 kommt nicht vor.");
            }
            if (!allHashtags.ContainsKey(new CryptDiary.Data.Hashtag("hashtag3")))
            {
                Assert.Fail("hashtag3 kommt nicht vor.");
            }

            // hashtag4 should not exist anymore
            if (allHashtags.ContainsKey(new CryptDiary.Data.Hashtag("hashtag4")))
            {
                Assert.Fail("hashtag4 sollte eigentlich gelöscht sein.");
            }

            // check hashtag1-dates
            {
                var DateList = allHashtags[new CryptDiary.Data.Hashtag("hashtag1")];
                {
                    if (DateList.Count != 1)
                    {
                        Assert.Fail("Falsche Anzahl Daten in hashtag1. Erwartet: 1, tatsächlich: " + DateList.Count.ToString());
                    }
                    if (!DateList.Contains(DateTime.Today))
                    {
                        Assert.Fail("Heutiges Datum kommt nicht vor in hashtag1.");
                    }
                }
            }

            // check hashtag2-dates
            {
                var DateList = allHashtags[new CryptDiary.Data.Hashtag("hashtag2")];
                {
                    if (DateList.Count != 2)
                    {
                        Assert.Fail("Falsche Anzahl Daten in hashtag2. Erwartet: 2, tatsächlich: " + DateList.Count.ToString());
                    }
                    if (!DateList.Contains(DateTime.Today))
                    {
                        Assert.Fail("Heutiges Datum kommt nicht vor in hashtag2.");
                    }
                    if (!DateList.Contains(DateTime.Today.AddDays(1)))
                    {
                        Assert.Fail("Morgiges Datum kommt nicht vor in hashtag2.");
                    }
                }
            }

            // check hashtag3-dates
            {
                var DateList = allHashtags[new CryptDiary.Data.Hashtag("hashtag3")];
                {
                    if (DateList.Count != 1)
                    {
                        Assert.Fail("Falsche Anzahl Daten in hashtag1. Erwartet: 1, tatsächlich: " + DateList.Count.ToString());
                    }
                    if (!DateList.Contains(DateTime.Today))
                    {
                        Assert.Fail("Heutiges Datum kommt nicht vor in hashtag3.");
                    }
                }
            }
        }
    }
}
