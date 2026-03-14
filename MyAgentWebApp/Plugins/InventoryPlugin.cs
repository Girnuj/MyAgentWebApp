using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace MyAgentWebApp.Plugins;

public sealed class InventoryPlugin
{
    private readonly Dictionary<string, bool> _availability= new()
    {
        { "item1", true },
        { "item2", true },
        { "item3", false }
    };

    [KernelFunction]
    [Description("Indica si existe en inventario todos los componentes necesarios para ensamblar el SKU.")]
    public bool CheckAvailabilityForSku([Description("el SKU del producto que se desea ensamblar")]string sku)
    {
        return _availability.First(_availability => _availability.Key.Equals(sku, StringComparison.OrdinalIgnoreCase)).Value;
    }
}
