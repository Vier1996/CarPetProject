using System.Text;

namespace Codebase.Extension.String.Cast
{
    public class HandledString
    {
        public bool IsBusy => !_isAvailable;
        
        private readonly StringBuilder _builder;
        
        private bool _isAvailable = true;

        public HandledString() => _builder = new StringBuilder(capacity: 10);

        public HandledString Reserve()
        {
            _isAvailable = false;
            return this;
        }

        public HandledString Release()
        {
            _isAvailable = true;
            return this;
        }

        public HandledString Clear()
        {
            _builder.Clear();
            return this;
        }
        
        public HandledString Append(int @int)
        {
            _builder.Append(@int);
            return this;
        }
        
        public HandledString Append(long @long)
        {
            _builder.Append(@long);
            return this;
        }
        
        public HandledString Append(float @float)
        {
            _builder.Append(@float);
            return this;
        }
        
        public HandledString Append(double @double)
        {
            _builder.Append(@double);
            return this;
        }
        
        public override string ToString() => _builder.ToString();
    }
}