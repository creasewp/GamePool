using System;
using Newtonsoft.Json;

namespace GamePool2Core.Entities
{
            [Serializable]   
    public class UserGame : BaseEntity
    {
        private int m_Confidence;

        public string Id { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public string GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }

        public int Confidence
        {
            get { return m_Confidence; }
            set { m_Confidence = value; RaisePropertyChanged(); }
        }

        public string WinnerTeamId { get; set; }
    }
}
