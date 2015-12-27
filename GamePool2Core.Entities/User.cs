
using System;

namespace GamePool2Core.Entities
{
    [Serializable]
    public class User
    {
        public string Id { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public string PasswordHash { get; set; }
        public bool IsLocked { get; set; }

        public bool IsEligible { get; set; }
        public int PoolScore { get; set; }
        public int LostPoints { get; set; }
        public int PossiblePoints { get; set; }

    }
}
