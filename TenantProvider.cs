namespace AccountingForDentists;

public sealed class TenantProvider(IHttpContextAccessor httpContextAccessor)
{
    public Guid GetTenantId()
    {
        if (Guid.TryParse(httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == "tid").SingleOrDefault()?.Value, out var guid))
        {
            return guid;
        }
        return Guid.Empty;
    }

    public Guid GetUserObjectId()
    {
        if (Guid.TryParse(httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == "oid").SingleOrDefault()?.Value, out var guid))
        {
            return guid;
        }
        return Guid.Empty;
    }

}