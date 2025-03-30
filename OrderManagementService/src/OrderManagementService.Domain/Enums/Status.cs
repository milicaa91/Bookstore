using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Domain.Enums
{
    public enum Status
    {
        Pending = 1,      
        Processing = 2,  
        Completed = 3,   
        Canceled = 4,   
        Failed = 5,       
        Refunded = 6,    
        Shipped = 7,     
        Delivered = 8,  
        OnHold = 9,
        Processed = 10
    }

    public enum EventType
    {
        OrderCreated = 1,
        OrderUpdated = 2,
        OrderDeleted = 3,
        OrderStatusChanged = 4
    }
}
