using LibraryApp.Attributes.Base;

namespace LibraryApp.Attributes
{
    public class HttpPutAttribute : HttpAttribute
    {
        public HttpPutAttribute(string routing) : base(HttpMethod.Put, routing) {}

        public HttpPutAttribute() : base(HttpMethod.Put, null) {}
    }
}