using ListaTarefa.Context;
using ListaTarefa.Models;
using ListaTarefa.Interfaces;
using ListaTarefa.Servico;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// pegando a configuração da conecção.
builder.Services.AddDbContext<OrganizadorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddScoped<ITarefa, Starefa>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();
 

//app.UseHttpsRedirection();


app.MapPost("/Tarefas", ([FromBody] Tarefa tarefa, ITarefa itarefa) =>
{
   
    var tarefas = new Tarefa
    {
        Titulo = tarefa.Titulo,
        Descricao = tarefa.Descricao,
        Data = tarefa.Data,
        Status = tarefa.Status
    };
    itarefa.Incluir(tarefas);
    return Results.Created($"/tarefas/{tarefas.Id}", tarefas);
})
.WithName("Tarefas")
.WithOpenApi();


app.MapGet("/Tarefas/ObterTodos", ([FromQuery] int pagina, ITarefa tarefa) =>
{
   var tarefas = tarefa.Todos(pagina);
   return Results.Ok(tarefas);
})
.WithName("TarefasTodos")
.WithOpenApi();


app.MapGet("/Tarefas/{id}", ([FromQuery] int id, ITarefa Starefa) =>
{
   var tarefas = Starefa.BuscaPorId(id);
   if(tarefas == null) return Results.NotFound();
   return Results.Ok(tarefas);
})
.WithName("TarefasId")
.WithOpenApi();


// app.MapGet("/Tarefas/{titulo}", ([FromQuery] string titulo, ITarefa Starefa) =>
// {
//    var tarefas = Starefa.BuscaPorTitulo(titulo);
//    if(tarefas == null) return Results.NotFound();
//    return Results.Ok(tarefas);
// })
// .WithName("TarefasTit")
// .WithOpenApi();


// app.MapGet("/Tarefas/{descricao}", ([FromQuery] string descricao, ITarefa Starefa) =>
// {
//    var tarefas = Starefa.BuscaPorTitulo(descricao);
//    if(tarefas == null) return Results.NotFound();
//    return Results.Ok(tarefas);
// })
// .WithName("TarefasDescricao")
// .WithOpenApi();


// app.MapGet("/Tarefas/{data}", ([FromQuery] string data, ITarefa Starefa) =>
// {
//    var tarefas = Starefa.BuscaPorTitulo(data);
//    if(tarefas == null) return Results.NotFound();
//    return Results.Ok(tarefas);
// })
// .WithName("TarefasData")
// .WithOpenApi();


// app.MapGet("/Tarefas/{status}", ([FromQuery] string status, ITarefa Starefa) =>
// {
//    var tarefas = Starefa.BuscaPorTitulo(status);
//    if(tarefas == null) return Results.NotFound();
//    return Results.Ok(tarefas);
// })
// .WithName("TarefasStatus")
// .WithOpenApi();


app.MapGet("/Tarefas/PorCampo", ([FromQuery] int pagina,[FromQuery] string? titulo, [FromQuery] string? descricao, [FromQuery] DateTime? data, [FromQuery] EnumStatusTarefa? status, ITarefa tarefaService) =>
{
    var tarefas = tarefaService.Todos(pagina, titulo, descricao, data, status);
    return Results.Ok(tarefas);
})
.WithName("TarefasStr")
.WithOpenApi();


app.MapDelete("/Tarefas/{id}", ([FromQuery] int id, ITarefa Starefa) =>
{
   var tarefas = Starefa.BuscaPorId(id);
                if(tarefas == null) return Results.NotFound();

                Starefa.Apagar(tarefas);

                return Results.NoContent();
})
.WithName("TarefasDel")
.WithOpenApi();


app.MapPut("/Tarefas/{id}", ([FromRoute] int id, Tarefa tarefa, ITarefa Starefa) =>
{
   var tarefaAnterior = Starefa.BuscaPorId(id);
   if(tarefaAnterior == null) return Results.NotFound();

    tarefaAnterior.Titulo = tarefa.Titulo;
    tarefaAnterior.Descricao = tarefa.Descricao;
    tarefaAnterior.Data = tarefa.Data;
    tarefaAnterior.Status = tarefa.Status;    

   Starefa.Atualizar(tarefaAnterior);               
   return Results.Ok(tarefaAnterior);
})
.WithName("Tarefas3")
.WithOpenApi();



        // [HttpGet("{id}")]
        // public IActionResult ObterPorId(int id)
        // {
        //     // TODO: Buscar o Id no banco utilizando o EF
        //     // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
        //     // caso contrário retornar OK com a tarefa encontrada
        //     return Ok();
        // }

        

        // [HttpGet("ObterPorTitulo")]
        // public IActionResult ObterPorTitulo(string titulo)
        // {
        //     // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
        //     // Dica: Usar como exemplo o endpoint ObterPorData
        //     return Ok();
        // }

        // [HttpGet("ObterPorData")]
        // public IActionResult ObterPorData(DateTime data)
        // {
        //     var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
        //     return Ok(tarefa);
        // }

        // [HttpGet("ObterPorStatus")]
        // public IActionResult ObterPorStatus(EnumStatusTarefa status)
        // {
        //     // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
        //     // Dica: Usar como exemplo o endpoint ObterPorData
        //     var tarefa = _context.Tarefas.Where(x => x.Status == status);
        //     return Ok(tarefa);
        // }

           


app.Run();

