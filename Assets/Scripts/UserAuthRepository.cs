using System;
public class UserAuthRepository
{
    public string accessToken { get; set; }
    private static readonly Lazy<UserAuthRepository> instance =
    new Lazy<UserAuthRepository>(() => new UserAuthRepository());
    public static UserAuthRepository Instance
    {
        get
        {
            return instance.Value;
        }
    }
    

    public UserAuthRepository()
    {

    }

    
}
