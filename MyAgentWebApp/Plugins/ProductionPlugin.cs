using Microsoft.Extensions.Caching.Memory;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace MyAgentWebApp.Plugins;

public sealed class ProductionPlugin(IMemoryCache memoryCache)
{
    [KernelFunction]
    [Description("Crea una orden de producción para un SKU específico.")]
    public async Task<OrderCreated> CreatedOrder(
        [Description("El Identificador del cliente")] string customerId
        ,[Description("El SKU del producto a esamblar")] string sku
        ,[Description("La cantidad a esamblar")] int quantity = 1
    )
    {
        var orderId = Guid.NewGuid();
        var newOrder = new OrderCreated(orderId, customerId, sku, quantity, DateTime.UtcNow);
        return newOrder;
    }

    [KernelFunction]
    [Description("Inicia el proceso de ensamblaje para una orden de producción específica.")]
    public async Task StartAssembly([Description("El identificador de la orden")] Guid orderId)
    {
        System.Timers.Timer timer = new(10_000);
        OrderStatus currentStatus = OrderStatus.InProgress;

        timer.Elapsed += (sender, e) =>
        {
            memoryCache.Set(orderId, currentStatus);
            (currentStatus, bool stopTimer) = currentStatus switch
            {
                OrderStatus.InProgress => (OrderStatus.Completed, false),
                OrderStatus.Completed => (OrderStatus.Completed, true),
                _ => (currentStatus, false)
            };
            if (stopTimer)
            {
                timer.Stop();
                timer.Dispose();
            }
        };
        timer.Start();
    }

    [KernelFunction]
    [Description("Obtiene el estado actual de una orden de producción específica.")]
    public string GetOrder([Description("El identificador de la orden")] Guid orderId)
    {
        var currentStatus = memoryCache.Get<OrderStatus>(orderId);
        return Enum.GetName(currentStatus)!;
}
}

public record OrderCreated(Guid OrderId, string CustomerId, string Sku, int Quantity, DateTime OrderDate);

public enum OrderStatus
{
    Pending,
    InProgress,
    Completed,
    Cancelled
}

