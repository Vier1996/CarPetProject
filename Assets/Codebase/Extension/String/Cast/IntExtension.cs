using System.Collections.Generic;

namespace Codebase.Extension.String.Cast
{
    public static class IntExtension
    {
        private static readonly List<HandledString> HandledStrings;

        static IntExtension() => HandledStrings = new List<HandledString>(10);

        public static string ToStringNonAlloc(this int value)
        {
            return GetHandledString()
                .Append(value)
                .Release()
                .ToString();
        }

        public static string ToStringNonAlloc(this long value) => 
            GetHandledString()
                .Append(value)
                .Release()
                .ToString();
        
        public static string ToStringNonAlloc(this float value) => 
            GetHandledString()
                .Append(value)
                .Release()
                .ToString();
        
        public static string ToStringNonAlloc(this double value) => 
            GetHandledString()
                .Append(value)
                .Release()
                .ToString();

        private static HandledString GetHandledString()
        {
            for (int i = 0; i < HandledStrings.Count; i++)
            {
                if (!HandledStrings[i].IsBusy)
                    return HandledStrings[i].Reserve().Clear();
            }
            
            HandledString handledString = new HandledString();
            
            HandledStrings.Add(handledString);

            return handledString;
        }
    }
}