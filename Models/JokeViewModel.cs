using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jokes_project.Models
{
  public class JokeViewModel
  {
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }

    public JokeViewModel()
    {

    }
  }
}
