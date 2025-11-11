using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CRUD.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CRUD.Controllers
{
    public class ProjetoController : Controller
    {
        private static List<Projeto> projetos = new List<Projeto>();

        public IActionResult Index() => View(projetos);

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Projeto projeto)
        {
            projeto.Id = projetos.Any() ? projetos.Max(p => p.Id) + 1 : 1;
            projeto.Etapas = projeto.Etapas ?? new List<Etapa>();
            projetos.Add(projeto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ToggleEtapa(int projetoId, int etapaId)
        {
            var etapa = projetos.FirstOrDefault(p => p.Id == projetoId)?
                               .Etapas.FirstOrDefault(e => e.Id == etapaId);
            if (etapa != null)
                etapa.Concluida = !etapa.Concluida;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditEtapa(int projetoId, int etapaId, string novoNome)
        {
            var etapa = projetos.FirstOrDefault(p => p.Id == projetoId)?
                               .Etapas.FirstOrDefault(e => e.Id == etapaId);
            if (etapa != null && !string.IsNullOrWhiteSpace(novoNome))
                etapa.Nome = novoNome;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddEtapa(int projetoId, string nomeEtapa)
        {
            var projeto = projetos.FirstOrDefault(p => p.Id == projetoId);

            if (projeto != null)
            {
                if (projeto.Etapas == null)
                    projeto.Etapas = new List<Etapa>();

                if (projeto.Etapas.Count < 5)
                {
                    int novoId = projeto.Etapas.Any() ? projeto.Etapas.Max(e => e.Id) + 1 : 1;
                    projeto.Etapas.Add(new Etapa { Id = novoId, Nome = nomeEtapa });
                }
            }

            // redireciona pro Index pra página ser recarregada (o fetch não segue redirect)
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UploadFoto(int projetoId, int etapaId, IFormFile foto)
        {
            var projeto = projetos.FirstOrDefault(p => p.Id == projetoId);
            var etapa = projeto?.Etapas.FirstOrDefault(e => e.Id == etapaId);

            if (etapa != null && foto != null && foto.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(foto.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    foto.CopyTo(stream);
                }

                etapa.Fotos.Add($"/uploads/{fileName}");
            }

            // Volta para Index (você já mostra popup via JS)
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteEtapa(int projetoId, int etapaId)
        {
            var projeto = projetos.FirstOrDefault(p => p.Id == projetoId);
            var etapa = projeto?.Etapas.FirstOrDefault(e => e.Id == etapaId);
            if (etapa != null)
                projeto.Etapas.Remove(etapa);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int projetoId)
        {
            var projeto = projetos.FirstOrDefault(p => p.Id == projetoId);
            if (projeto != null)
                projetos.Remove(projeto);
            return RedirectToAction("Index");
        }

        public IActionResult Registros() => View(projetos);
    }
}
