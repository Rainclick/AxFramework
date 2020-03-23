using System;

namespace Common
{
    public sealed class AxDisplay : Attribute
    {
        public string Title { get; }
        public string Key { get; }
        public AxOp Parent { get; }
        public int Order { get; set; }


        public AxDisplay(string title, string key, AxOp parent, int order = 0)
        {
            Title = title;
            Key = key;
            Parent = parent;
            Order = order;
        }
    }
}