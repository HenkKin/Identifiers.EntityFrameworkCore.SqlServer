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
        private readonly GuidDbContext _exampleDbContext;

        public ValuesController(GuidDbContext exampleDbContext)
        {
            _exampleDbContext = exampleDbContext;
        }

        [Route("Test")]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var exampleEntity1 = new ExampleIdentifierEntity
            {
                Name = "Henk Kin"
            };

            var exampleEntity2TranslationNlNl = new ExampleIdentifierEntityTranslation { Description = "Omschrijving", LocaleId = "nl-NL" };
            var exampleEntity2TranslationEnGb = new ExampleIdentifierEntityTranslation { Description = "Description", LocaleId = "en-GB" };
            var exampleEntity2 = new ExampleIdentifierEntity
            {
                Name = "Kin Henk",
                Translations = new List<ExampleIdentifierEntityTranslation>
                {
                    exampleEntity2TranslationNlNl,
                    exampleEntity2TranslationEnGb
                }
            };

            _exampleDbContext.Set<ExampleIdentifierEntity>().Add(exampleEntity1);
            _exampleDbContext.Set<ExampleIdentifierEntity>().Add(exampleEntity2);

            await _exampleDbContext.SaveChangesAsync();

            exampleEntity2TranslationNlNl.Description += " geupdated";
            exampleEntity2.Translations.Add(new ExampleIdentifierEntityTranslation{ Description = "Comment", LocaleId = "fr-FR"});
            exampleEntity2.Name = "Updated example";

            await _exampleDbContext.SaveChangesAsync();

            _exampleDbContext.Set<ExampleIdentifierEntity>().Remove(exampleEntity1);

            await _exampleDbContext.SaveChangesAsync();

            var entities = await _exampleDbContext.Set<ExampleIdentifierEntity>().AsNoTracking().ToListAsync();
           return Json(new{ entities });
        }

        [Route("get-all")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exampleEntities = await _exampleDbContext.Set<ExampleIdentifierEntity>().AsNoTracking()
                .ToListAsync();


            return Json(new { exampleEntities });
        }
    }
}
