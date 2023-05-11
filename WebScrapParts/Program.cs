using Microsoft.EntityFrameworkCore;
using WebScrapParts;
using WebScrapParts.Entities;
using WebScrapParts.Helpers;
using WebScrapParts.Infraestructure.Persistence.Contexts;

Console.WriteLine("=====================================================");
Console.WriteLine("OBTENER LA INFORMACION DE UN SITIO WEB");

//var scrapySelenium = new ScrapySelenium();
//var dc_scrapySelenium = new DC_ScrapySelenium();

Console.WriteLine(ConnectionStrings.StringConn_PC22);

Console.WriteLine("=====================================================");

// instanciamos manualmente el db context
var db = new WebScrapPartsDbContext();

//List<AppProductosScrapy> registrosParaScrapp = await db.AppProductosScrapy
//                                                       .AsNoTracking()
//                                                       .Where(x => x.Encontrado == false)
//                                                       .ToListAsync();

//dc_scrapySelenium.ScrapDcParts(registrosParaScrapp);

//scrapySelenium.ScrapDrivParts(registrosParaScrapp);

//*********************************************


List<AppYearMakeModel> appYearMakeModelsTodos = await db.AppYearMakeModel
                                                   .AsNoTracking()
                                                   .Where(x => x.Scratch == false && x.YearId >= 2004)
                                                   .OrderBy(x => x.YearId)
                                                   .ToListAsync();


if (appYearMakeModelsTodos.Count == 0)
{
    Console.WriteLine("No se Encontraron Registros");
    return;
}

var dpYMM = new DP_Scrapy_By_YMM();

var tareas = new List<Task>()
{
   // dpYMM.DPScrapByYmm(appYearMakeModels92),
    //dpYMM.DPScrapByYmm(appYearMakeModels93),
    dpYMM.DPScrapByYmm(appYearMakeModelsTodos),
    //dpYMM.DPScrapByYmm(appYearMakeModels20),
};

await Task.WhenAll(tareas);

//var dpYMM_Parallel = new DP_Scrapy_By_YMM_Paralell();
//dpYMM_Parallel.DPScrapByYmm_Parallel(appYearMakeModels);

//**********************************************************************

Console.WriteLine("Fin de la Aplicacion");