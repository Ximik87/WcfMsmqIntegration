using System;
using System.IO;

namespace MessageReceiverNetClassic
{
    internal struct IntDecoder
    {
        private int _value;
        private short _index;
        private bool _isValueDecoded;
        private const int LastIndex = 4;

        public int Value
        {
            get
            {
                if (!_isValueDecoded)
                    throw new InvalidOperationException("SR.GetString(SR.FramingValueNotAvailable))");
                return _value;
            }
        }

        public bool IsValueDecoded
        {
            get { return _isValueDecoded; }
        }

        public void Reset()
        {
            _index = 0;
            _value = 0;
            _isValueDecoded = false;
        }

        public int Decode(byte[] buffer, int offset, int size)
        {
            DecoderHelper.ValidateSize(size);
            if (_isValueDecoded)
            {
                throw new InvalidOperationException("SR.GetString(SR.FramingValueNotAvailable))");
            }
            int bytesConsumed = 0;
            while (bytesConsumed < size)
            {
                int next = buffer[offset];
                _value |= (next & 0x7F) << (_index * 7);
                bytesConsumed++;
                if (_index == LastIndex && (next & 0xF8) != 0)
                {
                    throw new InvalidDataException("SR.GetString(SR.FramingSizeTooLarge))");
                }
                _index++;
                if ((next & 0x80) == 0)
                {
                    _isValueDecoded = true;
                    break;
                }
                offset++;
            }
            return bytesConsumed;
        }
    }
}