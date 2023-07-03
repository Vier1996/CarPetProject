using System.Text;

namespace Codebase.Extension.String
{
    public class HandledString
    {
        private readonly StringBuilder _builder;
        private readonly bool _isAvailable;

        public HandledString()
        {
            _builder = new StringBuilder(capacity: 10);
            _isAvailable = false;
        }

        public StringBuilder GetBuilder() => _builder;
        
        public bool IsBusy() => _isAvailable;

        public HandledString Append(string @string)
        {
            _builder.Append(@string);
            return this;
        }
        
        public HandledString Append(HandledString handledString)
        {
            _builder.Append(handledString.GetBuilder());
            return this;
        }
    }
}