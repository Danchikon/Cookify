using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace Cookify.Application.Common.Helpers;

public static class GuidToShortStringConverter
{
    private const char EqualsChar = '-';
    private const char Hyphen = '-';
    private const char Underscore = '_';
    private const char Slash = '/';
    private const char Plus = '+';
    
    private const byte SlashByte = (byte)'/';
    private const byte PlusByte = (byte)'+';
    
    public static string FromGuidToShortString(Guid value)
    {
        Span<byte> idBytes = stackalloc byte[16];
        Span<byte> base64Bytes = stackalloc byte[24];

        MemoryMarshal.TryWrite(idBytes, ref value);
        Base64.EncodeToUtf8(idBytes, base64Bytes, out _, out _);

        Span<char> chars = stackalloc char[22];

        for (var i = 0; i < 22; i++)
        {
            chars[i] = base64Bytes[i] switch
            {
                SlashByte => Hyphen,
                PlusByte => Underscore,
                _ => (char)base64Bytes[i]
            };
        }

        return new string(chars);
    }
    
    public static Guid FromShortStringToGuid(ReadOnlySpan<char> shortString)
    {
        Span<char> base64Chars = stackalloc char[24];

        for (var i = 0; i < 22; i++)
        {
            base64Chars[i] = shortString[i] switch
            {
                Hyphen => Slash,
                Underscore => Plus,
                _ => shortString[i]
            };
        }

        base64Chars[23] = EqualsChar;
        base64Chars[24] = EqualsChar;

        Span<byte> idBytes = stackalloc byte[16];
        Convert.TryFromBase64Chars(base64Chars, idBytes, out _);
        return new Guid(idBytes);
    }
}