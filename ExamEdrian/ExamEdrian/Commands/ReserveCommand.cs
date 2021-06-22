using System;
using Newtonsoft.Json;

namespace ExamEdrian.Commands
{
    public class ReserveCommand
    {
        public string ResId { get; set; }

        public string UserEmail { get; set; }
    }
}
