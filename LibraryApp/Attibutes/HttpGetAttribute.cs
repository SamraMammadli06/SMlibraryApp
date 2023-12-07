using LibraryApp.Attributes.Base;

namespace LibraryApp.Attributes
{
    public class HttpGetAttribute : HttpAttribute
    {
        public HttpGetAttribute(string routing) : base(HttpMethod.Get, routing) {}

        public HttpGetAttribute() : base(HttpMethod.Get, null) {}
    }
}