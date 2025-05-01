using System;
namespace DenModerneProduktion.Components.Pages.PageModels
{
    public static class Extensions
    {
        public static Dictionary<T, T2> AddOrReplace<T, T2>(this Dictionary<T, T2> dic, T key, T2 value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }

            return dic;
        }
    }
}

