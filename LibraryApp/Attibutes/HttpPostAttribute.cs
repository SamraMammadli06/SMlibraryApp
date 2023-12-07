using LibraryApp.Attributes.Base;

namespace LibraryApp.Attributes
{
    public class HttpPostAttribute : HttpAttribute
    {
        public HttpPostAttribute(string routing) : base(HttpMethod.Post, routing) {}

        public HttpPostAttribute() : base(HttpMethod.Post, null) {}
    }
}