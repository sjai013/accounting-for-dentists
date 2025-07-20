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

    public string GetSubject()
    {
        var sub = httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == "sub").SingleOrDefault()?.Value.ToString() ?? "";
        return sub;
    }

    public void InitialiseTenant()
    {
        var baseTenantPath = BaseTenantPath();
        var attachmentsDirectory = AttachmentsDirectory();
        Directory.CreateDirectory(baseTenantPath);
        Directory.CreateDirectory(attachmentsDirectory);
    }

    private string BaseTenantPath()
    {
        var tenantId = GetTenantId();
        var userId = GetUserObjectId();
        var tenantPath = $"tenants/{tenantId:N}/{userId:N}";
        return tenantPath;
    }


    public string DatabasePath()
    {
        return $"{BaseTenantPath()}/db";
    }

    public string AttachmentsDirectory()
    {
        return $"{BaseTenantPath()}/files/";

    }

}