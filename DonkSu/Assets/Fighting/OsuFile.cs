using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Fighting
{
    public enum HitObjectType { Circle,Slider,Spinner}

    public class HitObject
    {
        public int X { get; set; }
        public int Y { get; set; }

        public HitObjectType Type { get; set; }

        public int AtTime { get; set; }
        public int AproachTime { get; set; }

        public int Length { get; set; }
    }

    public class TimingPoint
    {
        public int Offset { get; set; }
        public float MilisPerBeat { get; set; }

    }

    class OsuFile
    {
        public string FileContents;
        public Dictionary<string, string> PropertiesDictionary;

        public List<HitObject> HitObjects;
        public List<TimingPoint> TimingPoints;

        public OsuFile(string path)
        {
            FileContents = File.ReadAllText(path);

            LoadProps();
            LoadTiming();
            LoadHitObjects();
        }

        private void LoadTiming()
        {
            var cats = FileContents.Split('\n');
            var newCats = cats.ToList().Select(x => x.TrimEnd()).ToList();

            var start = newCats.IndexOf("[TimingPoints]");
            TimingPoints = new List<TimingPoint>();
            var lastNonNeg = 0f;

            for (int ii = start+1; ii < newCats.Count; ii++)
            {
                if (newCats[ii].StartsWith("[") || newCats[ii]=="")
                {
                    break;
                }

                var tokens = newCats[ii].Split(',');
                var nn = float.Parse(tokens[1]);
                var milis = 0f;
                if (nn >= 0)
                {
                    lastNonNeg = nn;
                    milis = nn;
                }
                else
                {
                    milis = lastNonNeg * (-nn / 100.0f);
                }

                var tt = new TimingPoint(){Offset = int.Parse(tokens[0]),MilisPerBeat = milis};
                TimingPoints.Add(tt);
            }
        }

        public TimingPoint GetTimingPointForTime(int time)
        {
            for (int i = TimingPoints.Count-1; i >= 0; i--)
            {
                if (TimingPoints[i].Offset <= time)
                {
                    return TimingPoints[i];
                }
            }

            return null;
        }

        public HitObject HitObjectFromString(string line)
        {
            var tokens = line.Split(',');

            var x = int.Parse(tokens[0]);
            var y = int.Parse(tokens[1]);
            var time = int.Parse(tokens[2]);
            var type = int.Parse(tokens[3]);

            var realType = HitObjectType.Circle;
            var typeBin = ByteConvert(type);
            typeBin = String.Join("",typeBin.Reverse().ToList().Select(h=>h.ToString()).ToArray());

            //0 1 2 3 4
            //1 2 4 8
            if (typeBin[0] == '0')
            {
                realType = HitObjectType.Slider;
            }
            if (typeBin[3] == '1')
            {
                realType = HitObjectType.Spinner;
            }

            var AR = float.Parse(PropertiesDictionary["ApproachRate"]);
            var timeFade = 0f;

            if (AR < 5)
            {
                timeFade = 1200f + 600f * (5 - AR) / 5f;
            }
            else if (AR == 5)
            {
                timeFade = 1200f;
            }
            else
            {
                timeFade = 1200f - 750f * (AR - 5f) / 5f;
            }

            timeFade *= 2;
            if (timeFade >= time)
            {
                timeFade = time;
            }

            var sliderLen = 0;

            if (realType == HitObjectType.Slider)
            {
                //slider duration = pixelLength / (100.0 * SliderMultiplier) * BeatDuration
                var pxLen = int.Parse(tokens[7]);
                var sliderMulty = int.Parse(PropertiesDictionary["SliderMultiplier"]);
                var tt = GetTimingPointForTime(time);
                var BeatDur = tt.MilisPerBeat;

                sliderLen = (int)(pxLen / (100.0f *sliderMulty) * BeatDur);
            }

            var ho = new HitObject() { X = x, Y = y, AtTime = time, Type = realType,AproachTime = time-(int)timeFade,Length = sliderLen};
            return ho;
        }

        public string ByteConvert(int num)
        {
            int[] p = new int[8];
            string pa = "";
            for (int ii = 0; ii <= 7; ii = ii + 1)
            {
                p[7 - ii] = num % 2;
                num = num / 2;
            }
            for (int ii = 0; ii <= 7; ii = ii + 1)
            {
                pa += p[ii].ToString();
            }
            return pa;
        }

        public bool IsTheNextBeatComingUp(float time)
        {
            var next = HitObjects[0];
            if (time*1000 >= next.AproachTime)
            {
                return true;
            }

            return false;
        }

        public HitObject GetNextHitObject()
        {
            var ht = HitObjects[0];
            HitObjects.RemoveAt(0);
            return ht;
        }

        private void LoadHitObjects()
        {
            var cats = FileContents.Split('\n');
            var newCats = cats.ToList().Select(x => x.TrimEnd()).ToList();

            var start = newCats.IndexOf("[HitObjects]");
            HitObjects = new List<HitObject>();

            for (int ii = start+1; ii < cats.Length; ii++)
            {

                var tokens = newCats[ii].Split(',');
                if (tokens.Length > 3)
                {
                    var ho = HitObjectFromString(newCats[ii]);
                    HitObjects.Add(ho);
                }
            }
        }

        private void LoadProps()
        {
            var cats = FileContents.Split('\n');
            PropertiesDictionary = new Dictionary<string, string>();

            foreach (var line in cats)
            {
                if (line == "[Events]")
                {
                    break;
                }

                if (!line.StartsWith("["))
                {
                    var tokens = line.Split(':');
                    if (tokens.Length == 2)
                    {
                        PropertiesDictionary.Add(tokens[0], tokens[1].Trim());
                    }
                }
            }
        }
    }
}
