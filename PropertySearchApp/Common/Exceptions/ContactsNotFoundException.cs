using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class ContactsNotFoundException : BaseApplicationException
{
    public ContactsNotFoundException(string message) : base(message)
    {
        
    }
}
