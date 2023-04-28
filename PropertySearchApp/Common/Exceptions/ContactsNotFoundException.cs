using PropertySearchApp.Common.Exceptions.Abstract;

namespace PropertySearchApp.Common.Exceptions;

public class ContactsNotFoundException : Abstract.HandledApplicationException
{
    public ContactsNotFoundException(string message) : base(message)
    {
        
    }
}
