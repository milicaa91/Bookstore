using OrderManagementService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OrderManagementService.Domain.Entities
{
    public class OutboxEvent
    {
        public Guid Id { get; set; }
        [Required]
        public EventType EventType{ get; set; }
        [Required]
        public string Payload { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public string? Error { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PublishedAt { get; set; }
    }
}
