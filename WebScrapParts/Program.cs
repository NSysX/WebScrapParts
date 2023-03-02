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
List<AppYearMakeModel> appYearMakeModels97 = await db.AppYearMakeModel
                                                   .AsNoTracking()
                                                   .Where(x => x.Found == false && x.YearId == 1997)
                                                   .ToListAsync();

List<AppYearMakeModel> appYearMakeModels98 = await db.AppYearMakeModel
                                                   .AsNoTracking()
                                                   .Where(x => x.Found == false && x.YearId == 1998)
                                                   .ToListAsync();

List<AppYearMakeModel> appYearMakeModels99 = await db.AppYearMakeModel
                                                   .AsNoTracking()
                                                   .Where(x => x.Found == false && x.YearId == 1999)
                                                   .ToListAsync();

List<AppYearMakeModel> appYearMakeModels00 = await db.AppYearMakeModel
                                                   .AsNoTracking()
                                                   .Where(x => x.Found == false && x.YearId == 2000)
                                                   .ToListAsync();

if (appYearMakeModels97.Count == 0)
{
    Console.WriteLine("No se Encontraron Registros");
    return;
}

var dpYMM = new DP_Scrapy_By_YMM();

var tareas = new List<Task>()
{
    dpYMM.DPScrapByYmm(appYearMakeModels97),
    dpYMM.DPScrapByYmm(appYearMakeModels98),
    dpYMM.DPScrapByYmm(appYearMakeModels99),
    dpYMM.DPScrapByYmm(appYearMakeModels00),
};

await Task.WhenAll(tareas);

//var dpYMM_Parallel = new DP_Scrapy_By_YMM_Paralell();
//dpYMM_Parallel.DPScrapByYmm_Parallel(appYearMakeModels);

//**********************************************************************

Console.WriteLine("Fin de la Aplicacion");