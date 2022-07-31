using System;
using System.IO;

namespace MessageReceiverNetClassic
{
    internal class ServerSingletonSizedDecoder : FramingDecoder
    {
        private readonly ViaStringDecoder _viaDecoder;
        private readonly ContentTypeStringDecoder _contentTypeDecoder;
        private State _currentState;
        private string _contentType;

        public ServerSingletonSizedDecoder(long streamPosition, int maxViaLength, int maxContentTypeLength)
            : base(streamPosition)
        {
            this._viaDecoder = new ViaStringDecoder(maxViaLength);
            this._contentTypeDecoder = new ContentTypeStringDecoder(maxContentTypeLength);
            this._currentState = State.ReadingViaRecord;
        }

        public int Decode(byte[] bytes, int offset, int size)
        {
            DecoderHelper.ValidateSize(size);

            try
            {
                int bytesConsumed;
                FramingRecordType recordType;
                switch (_currentState)
                {
                    case State.ReadingViaRecord:
                        recordType = (FramingRecordType)bytes[offset];
                        ValidateRecordType(FramingRecordType.Via, recordType);
                        bytesConsumed = 1;
                        _viaDecoder.Reset();
                        _currentState = State.ReadingViaString;
                        break;
                    case State.ReadingViaString:
                        bytesConsumed = _viaDecoder.Decode(bytes, offset, size);
                        if (_viaDecoder.IsValueDecoded)
                            _currentState = State.ReadingContentTypeRecord;
                        break;
                    case State.ReadingContentTypeRecord:
                        recordType = (FramingRecordType)bytes[offset];
                        if (recordType == FramingRecordType.KnownEncoding)
                        {
                            bytesConsumed = 1;
                            _currentState = State.ReadingContentTypeByte;
                        }
                        else
                        {
                            ValidateRecordType(FramingRecordType.ExtensibleEncoding, recordType);
                            bytesConsumed = 1;
                            _contentTypeDecoder.Reset();
                            _currentState = State.ReadingContentTypeString;
                        }
                        break;
                    case State.ReadingContentTypeByte:
                        _contentType = ContentTypeStringDecoder.GetString((FramingEncodingType)bytes[offset]);
                        bytesConsumed = 1;
                        _currentState = State.Start;
                        break;
                    case State.ReadingContentTypeString:
                        bytesConsumed = _contentTypeDecoder.Decode(bytes, offset, size);
                        if (_contentTypeDecoder.IsValueDecoded)
                        {
                            _currentState = State.Start;
                            _contentType = _contentTypeDecoder.Value;
                        }
                        break;
                    case State.Start:
                        throw new InvalidDataException("SR.GetString(SR.FramingAtEnd)))");
                    default:
                        throw new InvalidDataException("SR.GetString(SR.InvalidDecoderStateMachine)))");
                }

                StreamPosition += bytesConsumed;
                return bytesConsumed;
            }
            catch (InvalidDataException e)
            {
                throw CreateException(e);
            }
        }

        public void Reset(long streamPosition)
        {
            this.StreamPosition = streamPosition;
            this._currentState = State.ReadingViaRecord;
        }

        public State CurrentState
        {
            get { return _currentState; }
        }

        protected override string CurrentStateAsString
        {
            get { return _currentState.ToString(); }
        }

        public Uri Via
        {
            get
            {
                if (_currentState < State.ReadingContentTypeRecord)
                    throw new InvalidOperationException("SR.GetString(SR.FramingValueNotAvailable))");

                return _viaDecoder.ValueAsUri;
            }
        }

        public string ContentType
        {
            get
            {
                if (_currentState < State.Start)
                    throw new InvalidOperationException("SR.GetString(SR.FramingValueNotAvailable))");

                return _contentType;
            }
        }

        public enum State
        {
            ReadingViaRecord,
            ReadingViaString,
            ReadingContentTypeRecord,
            ReadingContentTypeString,
            ReadingContentTypeByte,
            Start,
        }
    }
}