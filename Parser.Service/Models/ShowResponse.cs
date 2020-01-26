using System;
using System.Collections.Generic;

namespace Parser.Models
{

    public class ShowResponse
    {
        public string title { get; set; }
        public string summary { get; set; }
        public string keywords { get; set; }
        public bool _explicit { get; set; }
        public Owner owner { get; set; }
        public string author { get; set; }
        public string language { get; set; }
        public string link { get; set; }
        public string copyright { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public DateTime publishDate { get; set; }
        public string subtitle { get; set; }
        public string type { get; set; }
        public List<Episode> episodes { get; set; }
        public int count { get; set; }
        public List<string> categories { get; set; }
        public float version { get; set; }
        public Signature signature { get; set; }
        public Networkinfo networkInfo { get; set; }
        public string acastSettings { get; set; }
        public Images images { get; set; }
        public string feedUrl { get; set; }
        public bool isAcast { get; set; }
        public string showUrl { get; set; }
        public string showId { get; set; }
        public bool _private { get; set; }
        public bool premium { get; set; }
        public bool unlocked { get; set; }
        public string checkSum { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
    }

    public class Owner
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class Signature
    {
        public string iv { get; set; }
        public string key { get; set; }
        public string algorithm { get; set; }
    }

    public class Networkinfo
    {
        public string name { get; set; }
        public string id { get; set; }
        public object slug { get; set; }
    }

    public class Images
    {
        public string x150 { get; set; }
        public string x350 { get; set; }
        public string x500 { get; set; }
        public string x1000 { get; set; }
        public string x1500 { get; set; }
        public string original { get; set; }
    }

    public class Episode
    {
        public Episode()
        {
            
        }
        public string showId { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public string hash { get; set; }
        public int contentLength { get; set; }
        public string contentType { get; set; }
        public string link { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int duration { get; set; }
        public bool _explicit { get; set; }
        public string keywords { get; set; }
        public string summary { get; set; }
        public DateTime publishDate { get; set; }
        public string subtitle { get; set; }
        public bool isAcast { get; set; }
        public bool premium { get; set; }
        public string acastId { get; set; }
        public string acastSettings { get; set; }
        public Images1 images { get; set; }
        public string episodeUrl { get; set; }
        public string episodeType { get; set; }
        public int episode { get; set; }
        public string metadataUrl { get; set; }
        public string checkSum { get; set; }
    }

    public class Images1
    {
        public string x150 { get; set; }
        public string x350 { get; set; }
        public string x500 { get; set; }
        public string x1000 { get; set; }
        public string x1500 { get; set; }
        public string original { get; set; }
    }


}
