namespace AccountingForDentists.Components.Pages;

public partial class Account(ILogger<Account> logger, IHttpContextAccessor accessor)
{
    public string UserInfo()
    {
        var user = accessor.HttpContext?.User;
        var claims = user?.Claims;
        var oid = user?.Claims.Where(x => x.Type == "oid").Single().Value;
        var tid = user?.Claims.Where(x => x.Type == "tid").Single().Value;

        var identities = user?.Identities;
        return claims?.ToString() ?? "";

    }
}