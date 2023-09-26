using System;
public class UserAuthentication
{
    public string accessToken { get; }
    public UserAuthentication(string accessToken)
    {
        this.accessToken = accessToken;
    }
}
