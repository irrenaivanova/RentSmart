namespace RentSmart.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RentSmart.Data.Common.Repositories;
    using RentSmart.Data.Models;
    public class PropertyTypeController : AdministrationController
    {
        private readonly IDeletableEntityRepository<PropertyType> typesRepository;

        public PropertyTypeController(IDeletableEntityRepository<PropertyType> typesRepository)
        {
            this.typesRepository = typesRepository;
        }

        // GET: Administration/PropertyType
        public async Task<IActionResult> Index()
        {
            return this.View(await this.typesRepository.AllWithDeleted().ToListAsync());
        }

        // GET: Administration/PropertyType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var type = await this.typesRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (type == null)
            {
                return this.NotFound();
            }

            return this.View(type);
        }

        // GET: Administration/PropertyType/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/PropertyTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] PropertyType type)
        {
            if (this.ModelState.IsValid)
            {
                await this.typesRepository.AddAsync(type);
                await this.typesRepository.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(type);
        }

        // GET: Administration/PropertyType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = this.typesRepository.All().FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // POST: Administration/PropertyType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] PropertyType type)
        {
            if (id != type.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.typesRepository.Update(type);
                    await this.typesRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.CategoryExists(type.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(type);
        }

        // GET: Administration/PropertyType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.typesRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // POST: Administration/PropertyType/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = this.typesRepository.All().FirstOrDefault(x => x.Id == id);
            this.typesRepository.Delete(category);
            await this.typesRepository.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool CategoryExists(int id)
        {
            return this.typesRepository.All().Any(e => e.Id == id);
        }
    }
}
