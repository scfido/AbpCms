using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Cms.Todo.Core
{
    [Table("CmsTodos")]
    public class Todo : Entity, IHasCreationTime
    {
        public bool IsDone { get; set; }

        public string Title { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
