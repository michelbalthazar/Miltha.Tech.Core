using System.IO.Compression;
using System.Text;

namespace System
{
    public static class ByteExtensions
    {
        public static async Task<byte[]> CompressGZipAsync(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            using var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                await gzipStream.WriteAsync(bytes);
            }

            return memoryStream.ToArray();
        }

        public static async Task<byte[]> DecompressGZipAsync(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            using var memoryStream = new MemoryStream(bytes);
            using var outputStream = new MemoryStream();
            using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                await decompressStream.CopyToAsync(outputStream);
            }

            return outputStream.ToArray();
        }

        public static string ToBase64(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            return Convert.ToBase64String(bytes);
        }

        public static string ToStringExtended(this byte[] bytes)
        {
            if (bytes == null)
                return string.Empty;

            return Encoding.UTF8.GetString(bytes);
        }

        public static int ToInt(this byte value) =>
            Convert.ToInt32(value);
    }
}