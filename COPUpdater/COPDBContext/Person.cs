using System;
using System.Collections.Generic;

namespace IO.Swagger.COPDBContext
{
    public partial class Person
    {
        public Person()
        {
            Action = new HashSet<Action>();
            PersonRoles = new HashSet<PersonRoles>();
            PersonThings = new HashSet<PersonThings>();
        }

        public int Personid { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public short? Gender { get; set; }
        public bool? Isactive { get; set; }
        public string Phone { get; set; }
        public int Organizationid { get; set; }

        public Organization Organization { get; set; }
        public UserAuth UserAuth { get; set; }
        public ICollection<Action> Action { get; set; }
        public ICollection<PersonRoles> PersonRoles { get; set; }
        public ICollection<PersonThings> PersonThings { get; set; }
    }
}
