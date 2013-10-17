using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ApiLayerExample.Inventory.Api
{
    internal class FiveLevelsOfMediaTypeParameters
    {
        public const string IsText = "is-text";
        public const string DomainModel = "domain-model";
        public const string Version = "version";
        public const string Schema = "schema";
        public const string Format = "format";
    }

    public class FiveLevelsOfMediaTypeFormatter : MediaTypeFormatter
    {
        private MediaTypeFormatter _internalFormatter;

        public FiveLevelsOfMediaTypeFormatter(MediaTypeFormatter internalFormatter)
        {
            _internalFormatter = internalFormatter;
        }

        public override bool CanReadType(Type type)
        {
            return _internalFormatter.CanReadType(type);
        }

        public override bool CanWriteType(Type type)
        {
            return _internalFormatter.CanWriteType(type);
        }

        public override Task WriteToStreamAsync(Type type, object value, 
            Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        {
            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext)
                .ContinueWith(t =>
                                  {
                                      if (!t.IsFaulted)
                                      {
                                          content.Headers.ContentType.Parameters
                                              .AddFiveLevelsOfMediaType(content.Headers.ContentType.MediaType, type);
                                      }
                                      return t;

                                  });
        }
    }

    internal static class NameValueHeaderValueCollectionExtensions
    {
        public static void AddFiveLevelsOfMediaType(this ICollection<NameValueHeaderValue> headers, 
            string contentType, Type type)
        {
            headers.Clear();
            headers.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.DomainModel, 
                type.AssemblyQualifiedName));
            headers.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Version, 
                type.Assembly.GetName().Version.ToString()));
            headers.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Format,
                ExtractFormat(contentType)));
            headers.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.Schema,
                contentType));
            headers.Add(new NameValueHeaderValue(FiveLevelsOfMediaTypeParameters.IsText,
                "true")); // TODO: a map of textual content types

            
        }

        internal static string ExtractFormat(string mediaType)
        {
            const string NonCanonicalSchemaFormat = @"^([\w\d.]+)/(?:([\w\d.]+)\+)?([\w\d]+)$";

            var match = Regex.Match(mediaType, NonCanonicalSchemaFormat);
            return match.Success
                       ? string.Format("{0}/{1}", match.Groups[1].Value, match.Groups[2].Value)
                       : mediaType;
        }
    }
        
}