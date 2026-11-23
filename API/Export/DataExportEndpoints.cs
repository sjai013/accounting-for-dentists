using Microsoft.AspNetCore.Authorization;

namespace AccountingForDentists.API.Export;

public static class DataExportEndpoints
{
    public static void MapDataExportEndpoints(this WebApplication app)
    {
        app.MapGet("/api/export", [Authorize] async (IDataExportService exportService, int[]? FY, CancellationToken cancellationToken) =>
        {
            var export = await exportService.ExportAsync(FY, cancellationToken);
            return Results.File(export.Bytes, "application/zip", export.Filename);
        });
    }
}
