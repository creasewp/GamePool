using System;
using Newtonsoft.Json;

namespace GamePool2Core.Entities
{
    [Serializable]
    public class Game : BaseEntity
    {
        public string Id { get; set; }
        public string Description { get; set; }

        private string m_HomeTeamId;
        public string HomeTeamId { get { return m_HomeTeamId; } set { m_HomeTeamId = value; RaisePropertyChanged(); } }
        [JsonIgnore]
        public Team HomeTeam { get; set; }
        private string m_AwayTeamId;
        public string AwayTeamId { get { return m_AwayTeamId; } set { m_AwayTeamId = value; RaisePropertyChanged(); } }
        [JsonIgnore]
        public Team AwayTeam { get; set; }

        public byte HomeScore { get; set; }
        public byte AwayScore { get; set; }
        public string GameDateTime { get; set; }
        public string Network { get; set; }
        public bool IsGameFinished{ get; set; }
        public int HomeSelectedCount { get; set; }
        public int AwaySelectedCount { get; set; }
    }
}
