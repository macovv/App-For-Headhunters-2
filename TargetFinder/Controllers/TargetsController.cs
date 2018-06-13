﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TargetFinder.Models;
using TargetFinder.Models.Target;

namespace TargetFinder.Controllers
{
    public class TargetsController : Controller
    {
        private static int currentIDNum;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly TargetFinderContext _context;

        public TargetsController(TargetFinderContext context, IHostingEnvironment environment)
        {
            _appEnvironment = environment;
            _context = context;
        }

        // GET: Targets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Target.ToListAsync());
        }

        // GET: Targets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var target = await _context.Target
                .SingleOrDefaultAsync(m => m.TargetId == id);
            if (target == null)
            {
                return NotFound();
            }

            return View(target);
        }

        // GET: Targets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Targets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TargetId,Name,Alias,Reward,LastLocation,Description")] Target target)
        {
            if (ModelState.IsValid)
            {
                _context.Add(target);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(target);
        }

        // GET: Targets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var target = await _context.Target.SingleOrDefaultAsync(m => m.TargetId == id);
            if (target == null)
            {
                return NotFound();
            }
            return View(target);
        }

        // POST: Targets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TargetId,Name,Alias,Reward,LastLocation,Description")] Target target, IFormFile file)
        {
            if (id != target.TargetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(target);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TargetExists(target.TargetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(target);
        }

        // GET: Targets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var target = await _context.Target
                .SingleOrDefaultAsync(m => m.TargetId == id);
            if (target == null)
            {
                return NotFound();
            }

            return View(target);
        }

        // POST: Targets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string path_Root = _appEnvironment.WebRootPath;
            string path_to_Images = path_Root + "\\images\\" + id + ".jpg";
        
            if (System.IO.File.Exists(path_to_Images))
                System.IO.File.Delete(path_to_Images);

            var target = await _context.Target.SingleOrDefaultAsync(m => m.TargetId == id);
            _context.Target.Remove(target);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TargetExists(int id)
        {
            return _context.Target.Any(e => e.TargetId == id);
        }

        [HttpGet] //1.Load

        public IActionResult AddPhoto(int id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var target =  _context.Target.SingleOrDefaultAsync(m => m.TargetId == id);
            if (target == null)
            {
                return NotFound();
            }
            currentIDNum = id;
            //--< Upload Form >--
            return View();
            //--</ Upload Form >--
        }

        [HttpPost] //Postback
        public async Task<IActionResult> AddPhoto(IFormFile file)

        {

            //--------< Upload_ImageFile() >--------

            //< check >
            if (file == null || file.Length == 0) return Content("file not selected");
            //</ check >

            //< get Path >
            string path_Root = _appEnvironment.WebRootPath;
            string path_to_Images = path_Root + "\\images\\" + currentIDNum.ToString() + ".jpg";
            //</ get Path >

            //< Copy File to Target >
            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //</ Copy File to Target >

            //< output >
            ViewData["FilePath"] = path_to_Images;
            return RedirectToAction("index");
            //</ output >

            //--------</ Upload_ImageFile() >--------
        }

    }
}
