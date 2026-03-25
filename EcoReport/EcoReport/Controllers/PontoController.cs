using EcoReport.Data;
using EcoReport.Models;
using EcoReport.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Transactions;

namespace EcoReport.Controllers
{

    public class PontoController : Controller
    {
        private readonly ILogger<PontoController> logger;
        private readonly EcoReportContext _context;

        public PontoController(ILogger<PontoController> logger, EcoReportContext context)
        {
            this.logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Visualizar(int id)
        {
            var pontoDto = new VisualizarPontoDTO();
            var classificacoes = new List<string>();
            try
            {
                
                var ponto = await _context.Ponto
                    .Where(ponto => ponto.Id == id)
                    .FirstOrDefaultAsync();

                if (ponto == null)
                {
                    throw new Exception("Ponto é nulo");

                }
                var pontoTiposDeArea = await _context.PontoTipoDeArea
                    .Where(ptipo => ptipo.PontoId == id)
                    .Include(ptipo => ptipo.TipoDeArea)
                    .ToListAsync();

                foreach (var tipoDeArea in pontoTiposDeArea)
                {
                    if (tipoDeArea.TipoDeAreaId != null)
                        classificacoes.Add(tipoDeArea.TipoDeArea.Classificacao);
                    else
                        classificacoes.Add(tipoDeArea.Descricao);

                }

                pontoDto = new VisualizarPontoDTO()
                {
                    Id = ponto.Id,
                    Descricao = ponto.Descricao,
                    Arquivo = ponto.Arquivo,
                    Ativo = ponto.Ativo,
                    Data = ponto.Data,
                    Classificacoes = classificacoes
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao visualizar ponto");
                Console.WriteLine(ex);

            }

            return Json(new
            {
                success = true,
                message = "Ponto carregado.",
                ponto = pontoDto
            });

        }

        [HttpPatch]
        public async Task<IActionResult> AlterarStatus(int id, bool resolvido)
        {
            try
            {
                var novoStatus = !resolvido;

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(id);
                Console.WriteLine(resolvido);


                var ponto = await _context.Ponto.Where(p => p.Id == id).FirstOrDefaultAsync();

                if (ponto == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "O ponto não foi encontrado.",
                    });
                }

                ponto.Ativo = novoStatus;
                
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Status alterado.",
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao mudar status");
                return Json(new
                {
                    success = false,
                    message = "Ocorreu um erro ao mudar status do ponto. Tente novamente mais tarde.",
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PontosSalvos()
        {
            var pontos = await _context.Ponto
                .Where(ponto => ponto.Ativo)
                .Select(ponto => new PontoSalvoDTO 
                {
                    Id = ponto.Id,
                    Lat = ponto.Lat,
                    Lon = ponto.Lon
                })
                .ToListAsync();

            return Ok(pontos);
        }
        private async Task<bool> VerificarTamanhoArquivo(IFormFile arquivo)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            return arquivo.Length <= maxFileSize;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(PontoRequestDTO pontoRequest)
        {
            try
            {
                Console.WriteLine(pontoRequest.Lat);
                Console.WriteLine(pontoRequest.Lon);

                var idArquivo = Guid.NewGuid();

                int idPonto;

                string? arquivoWExtension = null;
                string? arquivoNome = null;

                if (pontoRequest.Arquivo != null)
                {
                    string fileExtension = Path.GetExtension(pontoRequest.Arquivo.FileName);
                    arquivoWExtension = idArquivo + "" + fileExtension;
                    arquivoNome = pontoRequest.Arquivo.FileName;

                    var arquivoValido = await VerificarTamanhoArquivo(pontoRequest.Arquivo);

                    if (!arquivoValido)
                    {
                        return Json(new { success = false, message = "O arquivo é muito grande. O tamanho máximo permitido é 5 MB." });
                    }

                    var pasta = Path.Combine("pontos");

                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", pasta);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var path = Path.Combine(directory, arquivoWExtension);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        pontoRequest.Arquivo.CopyTo(stream);
                    }
                }

                using var transaction = _context.Database.BeginTransaction();

                try
                {

                    var ponto = new Ponto()
                    {

                        Descricao = pontoRequest.Descricao,
                        Lat = pontoRequest.Lat,
                        Lon = pontoRequest.Lon,
                        Arquivo = arquivoWExtension,
                        ArquivoDes = arquivoNome,
                        Ativo = true
                    };

                    await _context.Ponto.AddAsync(ponto);

                    await _context.SaveChangesAsync();

                    idPonto = ponto.Id;

                    if (pontoRequest.OutraClassificacao != null)
                    {
                        var classificacao = new PontoTipoDeArea()
                        {
                            PontoId = idPonto,
                            TipoDeArea = null,
                            Descricao = pontoRequest.OutraClassificacao
                        };
                        await _context.PontoTipoDeArea.AddAsync(classificacao);
                    }

                    if (pontoRequest.Tipos == null)
                    {
                        pontoRequest.Tipos = new List<int>();
                    }

                    var listaTiposLimpos = pontoRequest.Tipos.Distinct().ToList();

                    if (listaTiposLimpos.Count == 0 && String.IsNullOrEmpty(pontoRequest.OutraClassificacao))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Envie pelo menos um tipo de classificação."
                        });
                    }
                   
                    if (listaTiposLimpos.Count > 0)
                    {
                        // pega somente os tipos de área que existem no banco, para nao dar erro de chave estrangeira
                        var tipos = await _context.TipoDeArea.Where(t => listaTiposLimpos.Contains(t.Id)).ToListAsync();
                        foreach (var tipo in tipos)
                        {
                            var classificacao = new PontoTipoDeArea()
                            {
                                PontoId = idPonto,
                                TipoDeAreaId = tipo.Id,
                                Descricao = null
                            };
                            await _context.PontoTipoDeArea.AddAsync(classificacao);
                        }
                    }
                    

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Json(new { success = false, 
                        message = "Ocorreu um erro ao criar o ponto." });

                }

                return Json(new { success = true, message = "Ponto criado.", id = idPonto});
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao criar ponto");
                return Json(new
                {
                    success = false,
                    message = "Ocorreu um erro ao criar o ponto. Tente novamente mais tarde.",
                });

            }
        }
    }
}
