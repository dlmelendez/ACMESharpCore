using System;
using System.Text;

namespace PKISharp.SimplePKI.Util
{
    /// <summary>
    /// Collection of convenient crypto operations working
    /// with URL-safe Base64 encoding.
    /// </summary>
    internal static class Base64Tool
    {

        /// <summary>
        /// URL-safe Base64 encoding as prescribed in RFC 7515 Appendix C.
        /// </summary>
        public static ReadOnlySpan<char> UrlEncode(ReadOnlySpan<char> raw, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            Span<byte> rawBytes = stackalloc byte[encoding.GetMaxByteCount(raw.Length)];
            bool getBytes = encoding.TryGetBytes(raw, rawBytes, out int bytesWritten);
            if (!getBytes)
            {
                throw new InvalidOperationException("Failed to encode the input data to bytes.");
            }
            return UrlEncode(new Span<byte>([.. rawBytes.Slice(0, bytesWritten)]));
        }

        /// <summary>
        /// URL-safe Base64 encoding as prescribed in RFC 7515 Appendix C.
        /// </summary>
        public static ReadOnlySpan<char> UrlEncode(ReadOnlySpan<byte> raw)
        {
            Span<char> enc = stackalloc char[GetMaxBase64Length(raw.Length)];
            bool encoded = Convert.TryToBase64Chars(raw, enc, out int encodingCharCount, Base64FormattingOptions.None);
            if (!encoded)
            {
                throw new InvalidOperationException("Failed to encode the input data to base64.");
            }
            enc.Replace('+', '-');               // 62nd char of encoding
            enc.Replace('/', '_');               // 63rd char of encoding
                                                 //Find the first '=' and take the substring up to that point
            int indexOfFiller = enc.IndexOf<char>('=');
            if (indexOfFiller > 0)
            {
                return new ReadOnlySpan<char>([.. enc.Slice(0, indexOfFiller)]);
            }
            return new ReadOnlySpan<char>([.. enc.Slice(0, encodingCharCount)]);
        }

        /// <summary>
        /// Calculate the maximum length of a URL-safe Base64 encoding from byte array length
        /// </summary>
        /// <param name="byteArrayLength"></param>
        /// <returns></returns>
        public static int GetMaxBase64Length(int byteArrayLength)
        {
            return ((byteArrayLength + 2) / 3) * 4;
        }


        /// <summary>
        /// URL-safe Base64 decoding as prescribed in RFC 7515 Appendix C.
        /// </summary>
        public static ReadOnlySpan<byte> UrlDecode(ReadOnlySpan<char> enc)
        {
            int encodedLength = enc.Length;
            //Create a Span<byte> to hold the encoded chars plus maximum padding
            Span<char> raw = stackalloc char[encodedLength + 2];
            enc.CopyTo(raw);
            raw.Replace('-', '+');  // 62nd char of encoding
            raw.Replace('_', '/');  // 63rd char of encoding
            int padding = 0;

            // Pad with trailing '='s
            int mod = encodedLength % 4;
            if (mod != 0)
            {
                if (mod == 2)
                {
                    // Two pad chars
                    padding = 2;
                    raw[encodedLength] = '=';
                    raw[encodedLength + 1] = '=';
                }
                else if (mod == 3)
                {
                    // One pad char
                    padding = 1;
                    raw[encodedLength] = '=';
                }
                else
                {
                    throw new InvalidOperationException("Illegal base64url string!");
                }
            }

            int totalEncodedLength = encodedLength + padding;
            int maxByteLength = (totalEncodedLength / 4) * 3;
            Span<byte> rawBytes = stackalloc byte[maxByteLength];
            bool decoded = Convert.TryFromBase64Chars(raw.Slice(0, totalEncodedLength), rawBytes, out int bytesWritten);
            if (!decoded)
            {
                throw new InvalidOperationException($"Failed to decode the input data from base64: {enc.ToString()}");
            }
            return new ReadOnlySpan<byte>([.. rawBytes.Slice(0, bytesWritten)]); // Standard base64 decoder
        }

        public static string UrlDecodeToString(ReadOnlySpan<char> enc, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            return encoding.GetString(UrlDecode(enc));
        }
    }
}
