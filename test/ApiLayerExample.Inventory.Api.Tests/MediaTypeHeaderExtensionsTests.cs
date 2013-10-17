using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ApiLayerExample.Inventory.Api.MediaType;
using Xunit;
using Xunit.Extensions;

namespace ApiLayerExample.Inventory.Api.Tests
{

    public class TestClass
    {
        
    }


    public class MediaTypeHeaderExtensionsTests
    {
        [InlineData("application/javascript", "application/javascript")]
        [InlineData("application/rss+xml", "application/xml")]
        [InlineData("application/atom+xml", "application/xml")]
        [InlineData("application/hal+json", "application/json")]
        [InlineData("application.this.that/hal.this.that+json", "application.this.that/json")]
        [InlineData("application.this.that123/hal.this.that+json.456", "application.this.that123/json.456")]
        [Theory]
        public void ExtractFormat(string mediaType, string expectedFormat)
        {
            Assert.Equal(expectedFormat,
                         MediaTypeHeaderExtensions.ExtractFormat(mediaType));
        }

        [InlineData("application/atom+xml", 
            typeof(TestClass), 
            "1.0.0.0", true, 
            "ApiLayerExample.Inventory.Api.Tests.TestClass", 
            "application_xml", 
            "application_atom+xml")]
        [Theory]
        public void AddFiveLevel(string mediaType, Type type, 
            string version, bool isText, string domainModel,
            string format, string schema)
        {
            var content = new ByteArrayContent(new byte[0]);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            content.Headers.ContentType.AddFiveLevelsOfMediaType(type);
            Assert.Equal(isText.ToString().ToLower(), content.Headers.ContentType.Parameters.First(x=>
                x.Name == FiveLevelsOfMediaTypeParameters.IsText).Value);
            Assert.Equal(version, content.Headers.ContentType.Parameters.First(x =>
                x.Name == FiveLevelsOfMediaTypeParameters.Version).Value);
            Assert.Equal(domainModel, content.Headers.ContentType.Parameters.First(x =>
                x.Name == FiveLevelsOfMediaTypeParameters.DomainModel).Value);
            Assert.Equal(format, content.Headers.ContentType.Parameters.First(x =>
                x.Name == FiveLevelsOfMediaTypeParameters.Format).Value);
            Assert.Equal(schema, content.Headers.ContentType.Parameters.First(x =>
                x.Name == FiveLevelsOfMediaTypeParameters.Schema).Value);


        }
    }
}
