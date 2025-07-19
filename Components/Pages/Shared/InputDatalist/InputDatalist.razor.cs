using Microsoft.AspNetCore.Components;

namespace AccountingForDentists.Components.Pages.Shared.InputDatalist;

public partial class InputDatalist
{
    protected override void OnInitialized()
    {
        if (Attributes?.TryGetValue("class", out object? classObject) == true)
        {
            _class = classObject?.ToString() ?? "";
        }

        if (Attributes?.TryGetValue("id", out object? idObject) == true)
        {
            _id = idObject?.ToString() ?? "";
        }

        datalistId = Guid.NewGuid().ToString();
    }


    private void OnValueChange(ChangeEventArgs args)
    {
        this.Value = (string?)args.Value ?? "";
        ValueChanged.InvokeAsync(this.Value);
    }
}