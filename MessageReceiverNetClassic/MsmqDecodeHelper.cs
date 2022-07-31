using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace MessageReceiverNetClassic
{
    public static class MsmqDecodeHelper
    {
        private const int DefaultMaxViaSize = 2048;
        private const int DefaultMaxContentTypeSize = 256;

        private static void ReadServerMode(ServerModeDecoder modeDecoder, byte[] incoming, ref int offset, ref int size)
        {

            do
            {
                if (size <= 0)
                {
                    throw modeDecoder.CreatePrematureEOFException();
                }

                int decoded = modeDecoder.Decode(incoming, offset, size);
                offset += decoded;
                size -= decoded;

            } while (ServerModeDecoder.State.Done != modeDecoder.CurrentState);

        }

        public static Message DecodeTransportDatagram(Stream stream)
        {
            var bufferManager = BufferManager.CreateBufferManager(16, int.MaxValue);
            var a = new BinaryMessageEncodingBindingElement();
            var encoder = a.CreateMessageEncoderFactory().Encoder;
            //var stream = File.OpenRead(@"c:\msmq.bin");

            //long lookupId = msmqMessage.LookupId.Value;
            int size = (int)stream.Length;
            int offset = 0;
            byte[] incoming = new byte[size];
            stream.Read(incoming, 0, size);

            var modeDecoder = new ServerModeDecoder();

            try
            {
                ReadServerMode(modeDecoder, incoming, ref offset, ref size);
            }
            catch (ProtocolException ex)
            {
                throw new Exception("listener.NormalizePoisonException(messageProperty.LookupId", ex);
            }

            if (modeDecoder.Mode != FramingMode.SingletonSized)
            {
                throw new Exception("listener.NormalizePoisonException(messageProperty.LookupId, new ProtocolException(SR.GetString(SR.MsmqBadFrame))");
            }

            var decoder = new ServerSingletonSizedDecoder(0, DefaultMaxViaSize, DefaultMaxContentTypeSize);
            try
            {
                do
                {
                    if (size <= 0)
                    {
                        throw decoder.CreatePrematureEOFException();
                    }

                    int decoded = decoder.Decode(incoming, offset, size);
                    offset += decoded;
                    size -= decoded;

                } while (decoder.CurrentState != ServerSingletonSizedDecoder.State.Start);
            }
            catch (ProtocolException ex)
            {
                throw new Exception("listener.NormalizePoisonException(messageProperty.LookupId", ex);
            }

            if (size > MaxReceivedMessageSize)
            {
                throw new Exception("listener.NormalizePoisonException(messageProperty.LookupId, MaxMessageSizeStream.CreateMaxReceivedMessageSizeExceededException(listener.MaxReceivedMessageSize)");
            }

            if (!encoder.IsContentTypeSupported(decoder.ContentType))
            {
                throw new Exception("listener.NormalizePoisonException(messageProperty.LookupId, new ProtocolException(SR.GetString(SR.MsmqBadContentType))");
            }

            byte[] envelopeBuffer = bufferManager.TakeBuffer(size);
            Buffer.BlockCopy(incoming, offset, envelopeBuffer, 0, size);

            Message message;

            try
            {
                message = encoder.ReadMessage(new ArraySegment<byte>(envelopeBuffer, 0, size), bufferManager);
            }
            catch (XmlException e)
            {
                throw new Exception("listener.NormalizePoisonException(messageProperty.LookupId, new ProtocolException(SR.GetString(SR.MsmqBadXml)", e);
            }

            return message;
        }

        private const int MaxReceivedMessageSize = int.MaxValue;
    }
}