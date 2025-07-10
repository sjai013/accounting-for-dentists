namespace AccountingForDentists;

public sealed class TenantProvider(IHttpContextAccessor httpContextAccessor)
{

    public string? TenantId => httpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Type == "tid").Single().Value;
    public string? UserObjectId => httpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Type == "oid").Single().Value;

}