using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace CurrencyExchange.Contracts.Repositories.Entities
{
    [Table("user", Schema = "public")]
    public class UserEntity
    {
        [Key]
        [Column("user_id"), Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // [DefaultValue("public.uuid_generate_v4()")]
        public Guid UserId { get; set; }

        [Column("login", TypeName = "text"), Required]
        public string Login { get; set; } = null!;

        [Column("last_name", TypeName = "text"), Required]
        public string LastName { get; set; } = null!;

        [Column("first_name", TypeName = "text"), Required]
        public string FirstName { get; set; } = null!;

        [Column("created_on", TypeName = "timestamp")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on", TypeName = "timestamp")]
        public DateTime? UpdatedOn { get; set; }

        public List<UserAccountEntity> Account { get; set; } = new();
    }
}
