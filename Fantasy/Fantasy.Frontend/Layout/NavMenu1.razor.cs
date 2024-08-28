using Fantasy.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Fantasy.Frontend.Layout
{
    public partial class NavMenu
    {
        [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    }
}