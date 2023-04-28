using PropertySearchApp.Common.Exceptions.Abstract;
using System.Text;

namespace PropertySearchApp.Extensions;

public static class ApplicationExceptionExtension
{
    public static string BuildExceptionMessage(this Common.Exceptions.Abstract.HandledApplicationException exception)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in exception.Errors)
        {
            stringBuilder.Append(item);
        }
        return stringBuilder.ToString();
    }
}
