using Rahhal.Common.Views;

public  class UserInfoProvider
{
    public UserState userState { get; set; }

    public UserInfoProvider()
    {
        userState = new UserState(); 
    }
}