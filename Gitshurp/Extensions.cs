using System;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Gitshurp
{
    public static class Extensions
    {
        public static string FindByProp(this HtmlNode node, string name, string value)
        {
            var n = node
                .Descendants()
                .Where(x => x.Attributes != null)
                .Where(x => x.Attributes.Count > 0)
                .Where(x => x.Attributes[name] != null)
                .FirstOrDefault(x => x.Attributes[name].Value == value);
            return n?.InnerText?.Trim();
        }
        public static string GetString(this byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }
        public static byte[] ReadToEnd(this System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}