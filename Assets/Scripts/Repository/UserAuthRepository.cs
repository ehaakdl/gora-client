using System;
/*
 * 유저 인증 정보 담는 저장소 ex) 로그인 후 accessToken 등
 */
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
