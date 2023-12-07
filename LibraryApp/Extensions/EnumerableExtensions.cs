namespace LibraryApp.Extensions;

using System.Text;

public static class EnumerableExtensions
{
    public static string GetHtml<T>(this IEnumerable<T> items)
    {
        Type type = typeof(T);

        var props = type.GetProperties();

        StringBuilder sb = new StringBuilder(100);
        sb.Append("<ul>");
        foreach (var item in items)
        {
            
            foreach (var prop in props)
            {
                if(prop.Name == "BookId")
                    continue;
                sb.Append($"<li><span>{prop.Name}: </span>{prop.GetValue(item)}</li>");
                
            }
            sb.Append("<br/>");
        }
        sb.Append("</ul>");

        return sb.ToString();
    }
}