using System;
using System.Text;

namespace ACMESharp.Crypto
{
    public static partial class CryptoHelper
    {

        /// <summary>
        /// Collection of convenient crypto operations working
        /// with URL-safe Base64 encoding.
        /// </summary>
        public static partial class Base64
        {

            /// <summary>
            /// URL-safe Base64 encoding as prescribed in RFC 7515 Appendix C.
            /// </summary>
            public static ReadOnlySpan<char> UrlEncode(ReadOnlySpan<char> raw, Encoding? encoding = null)
            {
                encoding ??= Encoding.UTF8;
                Span<byte> rawBytes = stackalloc byte[encoding.GetMaxByteCount(raw.Length)];
                bool getBytes = encoding.TryGetBytes(raw, rawBytes, out int bytesWritten);
                if (!getBytes)
                {
                    throw new InvalidOperationException("Failed to encode the input data to bytes.");
                }
                return UrlEncode(new Span<byte>([..rawBytes.Slice(0, bytesWritten)]));
            }

            /// <summary>
            /// URL-safe Base64 encoding as prescribed in RFC 7515 Appendix C.
            /// </summary>
            public static ReadOnlySpan<char> UrlEncode(Span<byte> raw)
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
            /// Calculate the maximum length of a URL-safe Base64 encoding
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
            public static byte[] UrlDecode(string enc)
            {
                string raw = enc;
                raw = raw.Replace('-', '+');  // 62nd char of encoding
                raw = raw.Replace('_', '/');  // 63rd char of encoding
                switch (raw.Length % 4)       // Pad with trailing '='s
                {
                    case 0: break;               // No pad chars in this case
                    case 2: raw += "=="; break;  // Two pad chars
                    case 3: raw += "="; break;   // One pad char
                    default:
                        throw new System.Exception("Illegal base64url string!");
                }
                return Convert.FromBase64String(raw); // Standard base64 decoder
            }

            public static string UrlDecodeToString(string enc, Encoding? encoding = null)
            {
                encoding ??= Encoding.UTF8;
                return encoding.GetString(UrlDecode(enc));
            }
        }
    }
}
