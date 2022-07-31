using System;

namespace MessageReceiverNetClassic
{
    internal static class DecoderHelper
    {
        public static void ValidateSize(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("SR.GetString(SR.ValueMustBePositive)");
            }
        }
    }
}