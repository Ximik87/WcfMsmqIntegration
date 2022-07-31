using System;
using System.IO;

namespace MessageReceiverNetClassic
{
    internal class ViaStringDecoder : StringDecoder
    {
        private Uri _via;

        public ViaStringDecoder(int sizeQuota)
            : base(sizeQuota)
        {
        }

        protected override Exception OnSizeQuotaExceeded(int size)
        {
            Exception result = new InvalidDataException("SR.GetString(SR.FramingViaTooLong, size)");
            return result;
        }

        protected override void OnComplete(string value)
        {
            try
            {
                _via = new Uri(value);
                base.OnComplete(value);
            }
            catch (UriFormatException exception)
            {
                throw new InvalidDataException("SR.GetString(SR.FramingViaNotUri, value)", exception);
            }
        }

        public Uri ValueAsUri
        {
            get
            {
                if (!IsValueDecoded)
                    throw new InvalidOperationException("SR.GetString(SR.FramingValueNotAvailable))");
                return _via;
            }
        }
    }
}