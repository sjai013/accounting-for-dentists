namespace AccountingForDentists;

public sealed class TenantProvider(IHttpContextAccessor httpContextAccessor)
{

    public string TenantId => httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == "tid").SingleOrDefault()?.Value ?? "";
    public string UserObjectId => httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == "oid").SingleOrDefault()?.Value ?? "";

}