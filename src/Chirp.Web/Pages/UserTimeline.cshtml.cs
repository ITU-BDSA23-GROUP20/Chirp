﻿using Chirp.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public ICollection<CheepViewModel> Cheeps { get; set; }
    public string Authorname;

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        int page;
        if(Request.Query.ContainsKey("page")){
            page = int.Parse(Request.Query["page"]);
        } else{
            page = 1;
        }

        Authorname = author;
        
        try
        {
            Cheeps = _service.GetCheepsFromAuthor(author, page);
        }
        catch (Exception e)
        {
            Cheeps = new List<CheepViewModel>();
        }

        return Page();
    }
}
