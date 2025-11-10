using Microsoft.AspNetCore.Mvc;
using CRUD.Models;
using System.Collections.Generic;
using System.Linq;

namespace CRUD.Controllers
{
    public class ProjetoController : Controller
    {
        private static List<Projeto> projetos = new List<Projeto>();

        public IActionResult Index()
        {
            return View(projetos);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Projeto projeto)
        {
            projeto.Id = projetos.Any() ? projetos.Max(p => p.Id) + 1 : 1;
            projeto.Etapas = new List<Etapa>(); // Nenhuma etapa por padrão
            projetos.Add(projeto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HttpPost]
        public IActionResult ToggleEtapa(int projetoId, int etapaId)
        {
            var projeto = projetos.FirstOrDefault(p => p.Id == projetoId);
            var etapa = projeto?.Etapas.FirstOrDefault(e => e.Id == etapaId);
            if (etapa != null) etapa.Concluida = !etapa.Concluida;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditEtapa(int projetoId, int etapaId, string novoNome)
        {
            var projeto = projetos.FirstOrDefault(p => p.Id == projetoId);
            var etapa = projeto?.Etapas.FirstOrDefault(e => e.Id == etapaId);
            if (etapa != null && !string.IsNullOrWhiteSpace(novoNome))
            {
                etapa.Nome = novoNome;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteEtapa(int projetoId, int etapaId)
        {
            var projeto = projetos.FirstOrDefault(p => p.Id == projetoId);
            var etapa = projeto?.Etapas.FirstOrDefault(e => e.Id == etapaId);
            if (etapa != null) projeto.Etapas.Remove(etapa);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult AddEtapa(int projetoId, string nomeEtapa)
        {
            var projeto = projetos.FirstOrDefault(p => p.Id == projetoId);
            if (projeto != null && projeto.Etapas.Count < 5)
            {
                int novoId = projeto.Etapas.Any() ? projeto.Etapas.Max(e => e.Id) + 1 : 1;
                projeto.Etapas.Add(new Etapa { Id = novoId, Nome = nomeEtapa });
            }
            return RedirectToAction("Index");
        }
    }
}
