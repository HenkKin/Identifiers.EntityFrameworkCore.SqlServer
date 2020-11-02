using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessClientExample.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataAccessClientExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        private readonly IntDbContext _exampleDbContext;

        public ValuesController(IntDbContext exampleDbContext)
        {
            _exampleDbContext = exampleDbContext;
        }

        [Route("Test")]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var exampleEntity1 = new ExampleEntity
            {
                Name = "Henk Kin"
            };

            var exampleEntity2TranslationNlNl = new ExampleEntityTranslation { Description = "Omschrijving", LocaleId = "nl-NL" };
            var exampleEntity2TranslationEnGb = new ExampleEntityTranslation { Description = "Description", LocaleId = "en-GB" };
            var exampleEntity2 = new ExampleEntity
            {
                Name = "Kin Henk",
                Translations = new List<ExampleEntityTranslation>
                {
                    exampleEntity2TranslationNlNl,
                    exampleEntity2TranslationEnGb
                }
            };

            _exampleDbContext.Set<ExampleEntity>().Add(exampleEntity1);
            _exampleDbContext.Set<ExampleEntity>().Add(exampleEntity2);

            await _exampleDbContext.SaveChangesAsync    ();

            exampleEntity2TranslationNlNl.Description += " geupdated";
            exampleEntity2.Translations.Add(new ExampleEntityTranslation{ Description = "Comment", LocaleId = "fr-FR"});
            exampleEntity2.Name = "Updated example";

            await _exampleDbContext.SaveChangesAsync();

            _exampleDbContext.Set<ExampleEntity>().Remove(exampleEntity1);

            await _exampleDbContext.SaveChangesAsync();

            var entities = await _exampleDbContext.Set<ExampleEntity>().AsNoTracking().ToListAsync();
           return Json(new{ entities });
        }

        [Route("get-all")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exampleEntities = await _exampleDbContext.Set<ExampleEntity>().AsNoTracking()
                .ToListAsync();


            return Json(new { exampleEntities });
        }
    }
}
