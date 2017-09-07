using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Citations.Models;

namespace Citations.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //получение всех категорий из БД
            ViewBag.Categories = Category.Read();

            return View();
        }

        //получение всех цитат
        [HttpGet]
        public IEnumerable<Citation> Get()
        {
            return CitationCRUD.Read();
        }

        //создание цитаты
        [HttpPost]
        public IActionResult Create(Citation citation)
        {
            citation.Date = DateTime.Now;
            citation.Category.Id = Category.GetIdByName(citation.Category.Name);
            CitationCRUD.Create(citation);

            return RedirectToAction("Index");
        }

        //загрузка представления для изменения цитаты
        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.Categories = Category.Read();
            if (id != 0)
            {
                Citation citation = Citation.GetCitation(id);
                if (citation != null)
                    return View(citation);
            }
            return NotFound();
        }

        //изменеие цитаты
        [HttpPost]
        public IActionResult Update(Citation citation)
        {
            if (citation.Id != 0)
            {
                citation.Date = DateTime.Now;
                citation.Category.Id = Category.GetIdByName(citation.Category.Name);
                CitationCRUD.Update(citation);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        //удаление цитаты
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                CitationCRUD.Delete(id);
                return Ok();
            }
            return NotFound();
        }

        //поиск цитат по тексту и автору
        [HttpPost]
        public IActionResult Search(String text, String author)
        {
            List<Citation> citations = Citation.Search(text,author);
            return Ok(citations);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
