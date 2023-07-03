using System.Collections.Generic;

namespace Codebase.Extension.String
{
    public class StringHub
    {
        private static readonly List<HandledString> _handledStrings = new List<HandledString>();

        public static HandledString GetString()
        {
            for (int i = 0; i < _handledStrings.Count; i++)
            {
                if (_handledStrings[i].IsBusy())
                    return _handledStrings[i];
            }

            return new HandledString();
        }
    }
}