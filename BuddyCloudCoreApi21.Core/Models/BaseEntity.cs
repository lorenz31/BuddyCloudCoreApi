using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BuddyCloudCoreApi21.Core.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}