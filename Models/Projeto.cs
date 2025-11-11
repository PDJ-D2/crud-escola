using System.Collections.Generic;

namespace CRUD.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }

        // Lista de etapas do projeto
        public List<Etapa> Etapas { get; set; } = new List<Etapa>();
    }
}


// Etapa.cs
namespace CRUD.Models
{
    public class Etapa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Concluida { get; set; }
        public List<string> Fotos { get; set; } = new List<string>();
    }
}

