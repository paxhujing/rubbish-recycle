using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Main.App_Start
{
    public class TextPlainFormatter : MediaTypeFormatter
    {
        public TextPlainFormatter()
        {
            base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

        public override Boolean CanReadType(Type type)
        {
            return type == typeof(String);
        }

        public override Boolean CanWriteType(Type type)
        {
            return type == typeof(String);
        }

        public override Task WriteToStreamAsync(Type type, Object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                using (StreamWriter sw = new StreamWriter(writeStream))
                {
                    sw.Write(value);
                    sw.Flush();
                }
            });
        }

        public override Task<Object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.Factory.StartNew(() =>
                {
                    using (StreamReader sr = new StreamReader(readStream))
                    {
                        return (Object)sr.ReadToEnd();
                    }
                });
        }
    }
}
