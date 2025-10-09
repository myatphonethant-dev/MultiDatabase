using System;
using System.Collections.Generic;

namespace MultiDatabase.Models.Postgres;

public partial class TblBranch
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Map { get; set; } = null!;

    public string? Email { get; set; }

    public bool Delflag { get; set; }

    public DateTime Createddatetime { get; set; }

    public string Createduser { get; set; } = null!;

    public DateTime? Updateddatetime { get; set; }

    public string? Updateduser { get; set; }
}
