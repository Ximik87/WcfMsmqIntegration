using System;
using System.IO;
using System.ServiceModel;

namespace MessageReceiverNetClassic
{
    internal abstract class FramingDecoder
    {
        protected FramingDecoder()
        {
        }

        protected FramingDecoder(long streamPosition)
        {
            this.StreamPosition = streamPosition;
        }

        protected abstract string CurrentStateAsString { get; }

        public long StreamPosition { get; set; }

        protected void ValidateFramingMode(FramingMode mode)
        {
            switch (mode)
            {
                case FramingMode.Singleton:
                case FramingMode.Duplex:
                case FramingMode.Simplex:
                case FramingMode.SingletonSized:
                    break;
                default:
                {
                    throw new Exception(
                        "SR.FramingModeNotSupported, mode.ToString())), FramingEncodingString.UnsupportedModeFault)");
                }
            }
        }

        protected void ValidateRecordType(FramingRecordType expectedType, FramingRecordType foundType)
        {
            if (foundType != expectedType)
            {
                throw new InvalidDataException("SR.GetString(SR.FramingRecordTypeMismatch, expectedType.ToString(), foundType.ToString())");
            }
        }



        protected void ValidateMajorVersion(int majorVersion)
        {
            if (majorVersion != FramingVersion.Major)
            {
                Exception exception = new InvalidDataException(@"SR.GetString(
                    SR.FramingVersionNotSupported, majorVersion)), FramingEncodingString.UnsupportedVersionFault");
                throw exception;
            }
        }

        public Exception CreatePrematureEOFException()
        {
            return CreateException(new InvalidDataException("SR.GetString(SR.FramingPrematureEOF)"));
        }

        protected Exception CreateException(InvalidDataException innerException)
        {
            return new ProtocolException("SR.GetString(SR.FramingError, StreamPosition, CurrentStateAsString)",
                innerException);
        }
    }
}