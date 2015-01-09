
using System;

namespace GamePool2Core.Entities
{
    [Serializable]
    public class Setting
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public string Value { get; set; }
    }
}
