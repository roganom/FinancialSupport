using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using FinancialSupport.Domain.Entities;
using FinancialSupport.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace FinancialSupport.WebUI.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmprestimoService _emprestimoService;
        private readonly IParcelaService _parcelaService;
        private readonly IWebHostEnvironment _appEnvironment;

        public ClienteController(IUsuarioService usuarioService,
                                 IEmprestimoService emprestimo,
                                 IParcelaService parcelaService,
                                 IWebHostEnvironment appEvironment)
        {
            _usuarioService = usuarioService;
            _parcelaService = parcelaService;
            _emprestimoService = emprestimo;
            _appEnvironment = appEvironment;
        }
        #region Indice
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            return View(usuarios);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsuarioDTO usuario, IFormFile? Arquivo, string url)
        {
            string foto = SobeArquivo(Arquivo);

            //string url_Atual = HttpContext.Request.Path;

            if (ModelState.IsValid)
            {
                usuario.LimiteDisponivel = usuario.Limite;
                usuario.Foto = foto;
                await _usuarioService.Add(usuario);
                if (url.Contains("/Cliente/Create")) return RedirectToAction("Index", "Cliente");
                else if (url.Contains("/Usuario/Create")) return RedirectToAction("Index", "Usuario");
                else return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound("Id nulo.");

            var cliente = await _usuarioService.GetHistoricoById(id);

            if (cliente == null) return NotFound("Não existe cliente com esse Id.");

            var wwwroot = _appEnvironment.WebRootPath;
            var imagem = Path.Combine(wwwroot, "Imagens\\" + cliente.Foto);
            var exists = System.IO.File.Exists(imagem);
            ViewBag.ImageExist = exists;

            return View(DtoToViewModelDetalhamento(cliente));
        }

        private DetalhesClienteViewModel DtoToViewModelDetalhamento(UsuarioDTO usuarioDto)
        {
            DetalhesClienteViewModel viewModel = new DetalhesClienteViewModel();

            viewModel.Foto = usuarioDto.Foto;
            viewModel.Nome = usuarioDto.Nome;
            viewModel.Limite = (decimal?)usuarioDto.Limite;
            viewModel.LimiteDisponivel = (decimal?)usuarioDto.LimiteDisponivel;

            if (usuarioDto.Emprestimos != null)
            {
                foreach (var emp in usuarioDto.Emprestimos)
                {
                    var objAtivo = new EmprestimoListaViewModel();
                    var objHistorico = new HistoricoListaViewModel();
                    decimal EmprestimoInativoIdValorDasParcelas = 0;
                    int EmprestimoInativoIdNumeroDeParcelas = 0;
                    int EmprestimoInativoIdNumeroDeParcelasEmAtraso = 0;
                    int QtdeParcelasPagas = 0;
                    int QtdeParcelasAtrasadas = 0;

                    if (emp.Ativo)
                    {
                        objAtivo.Data = emp.Data.Value.ToString("dd/MM/yyyy");
                        objAtivo.Valor = emp.Valor.ToString("N2");
                        if (emp.NumeroParcelas != 0 && emp.Parcelas.Count > 0)
                        {
                            /// será que eu tenho um empréstimo sem parcela???
                            objAtivo.ValorParcela = (emp.Parcelas[0].ValorParcela).ToString("N2");
                        }

                        foreach (var parc in emp.Parcelas)
                        {
                            if (parc.DataPagamento != null && parc.ValorPagamento >= parc.ValorParcela)
                                QtdeParcelasPagas += 1;
                            else
                            {
                                if ((parc.DataParcela < DateTime.Today) && (!parc.ValorPagamento.HasValue || (parc.ValorPagamento < parc.ValorParcela)))    // ############# assim mesmo que faço a comparação? e as horas?
                                    QtdeParcelasAtrasadas += 1;
                            }
                        }
                        objAtivo.QtdeParcelasPagas = QtdeParcelasPagas.ToString();
                        objAtivo.QtdeParcelasAtrasadas = QtdeParcelasAtrasadas.ToString();

                        viewModel.EmprestimosAtivos.Add(objAtivo);
                    }
                    else
                    {
                        objHistorico.EmprestimoInativoIdData = emp.Data.Value.ToString("dd/MM/yyyy");
                        objHistorico.EmprestimoInativoIdValor = emp.Valor.ToString("N2");

                        foreach (var parc in emp.Parcelas)
                        {
                            EmprestimoInativoIdValorDasParcelas += parc.ValorParcela;
                            EmprestimoInativoIdNumeroDeParcelas += 1;
                            if (parc.DataPagamento > parc.DataParcela)              // ############# comparar só os dias (não considerar horas!!!)
                                EmprestimoInativoIdNumeroDeParcelasEmAtraso += 1;

                        }
                        if (EmprestimoInativoIdNumeroDeParcelas != 0)
                            objHistorico.EmprestimoInativoIdValorDasParcelas = (EmprestimoInativoIdValorDasParcelas / EmprestimoInativoIdNumeroDeParcelas).ToString();
                        else
                            objHistorico.EmprestimoInativoIdValorDasParcelas = "0";

                        objHistorico.EmprestimoInativoIdNumeroDeParcelas = EmprestimoInativoIdNumeroDeParcelas.ToString();
                        objHistorico.EmprestimoInativoIdNumeroDeParcelasEmAtraso = EmprestimoInativoIdNumeroDeParcelasEmAtraso.ToString();

                        viewModel.EmprestimosHistoricos.Add(objHistorico);
                    }
                }
            }

            return viewModel;
        }
        #endregion

        #region Novo Empréstimo
        [HttpGet]
        public async Task<IActionResult> Novo(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioDto = await _usuarioService.GetById(id);

            if (usuarioDto == null)
                return NotFound();

            var novoEmpestimoViewModel = new NovoEmpestimoViewModel();

            novoEmpestimoViewModel.IdUsuario = (int)id;
            novoEmpestimoViewModel.Nome = usuarioDto.Nome;
            novoEmpestimoViewModel.LimiteDisponivel = (decimal)usuarioDto.LimiteDisponivel;
            novoEmpestimoViewModel.NumeroParcelas = 60;

            return View(novoEmpestimoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Novo(NovoEmpestimoViewModel novoEmpestimoViewModel)
        {
            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> retorno;

                if (novoEmpestimoViewModel.Valor > 0)
                {
                    retorno = await _emprestimoService.CriaNovoEmprestimo(novoEmpestimoViewModel.IdUsuario, novoEmpestimoViewModel.Valor, novoEmpestimoViewModel.NumeroParcelas, 1, true, novoEmpestimoViewModel.Operador);

                    var _usuario = await _usuarioService.GetById(novoEmpestimoViewModel.IdUsuario);
                    novoEmpestimoViewModel.LimiteDisponivel = _usuario.LimiteDisponivel.Value;
                    novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = retorno.Key;
                    novoEmpestimoViewModel.CustomMessagePartial.Mensagem = retorno.Value;
                }
                else
                {
                    novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = 3;
                    novoEmpestimoViewModel.CustomMessagePartial.Mensagem = "Valor inválido";
                }

                return View(novoEmpestimoViewModel);
            }

            novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = 3;
            novoEmpestimoViewModel.CustomMessagePartial.Mensagem = "Invalid ModelState!";

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Novo Empréstimo - Simular
        [HttpPost]
        public async Task<IActionResult> Simular(NovoEmpestimoViewModel novoEmpestimoViewModel)
        {
            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> retorno;

                if (novoEmpestimoViewModel.Valor > 0)
                {
                    retorno = await _emprestimoService.CriaNovoEmprestimo(novoEmpestimoViewModel.IdUsuario, novoEmpestimoViewModel.Valor, novoEmpestimoViewModel.NumeroParcelas, 1, false, "Simulação");

                    var _usuario = await _usuarioService.GetById(novoEmpestimoViewModel.IdUsuario);
                    novoEmpestimoViewModel.LimiteDisponivel = _usuario.LimiteDisponivel.Value;
                    novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = retorno.Key;
                    novoEmpestimoViewModel.CustomMessagePartial.Mensagem = retorno.Value;
                }
                else
                {
                    novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = 3;
                    novoEmpestimoViewModel.CustomMessagePartial.Mensagem = "Valor inválido";
                }

                return View(novoEmpestimoViewModel);
            }

            novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = 3;
            novoEmpestimoViewModel.CustomMessagePartial.Mensagem = "Invalid ModelState!";

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Pagamento de parcela - Pagamento
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> Pagamento(int? id, int? tipoMensagem, string? mensagem)
        {
            if (tipoMensagem == null)
                tipoMensagem = 0;

            if (mensagem == null)
                mensagem = "";

            if (id == null) return NotFound("Id nulo.");

            var cliente = await _usuarioService.GetHistoricoById(id);

            if (cliente == null) return NotFound("Não existe cliente com esse Id.");

            var wwwroot = _appEnvironment.WebRootPath;
            var imagem = Path.Combine(wwwroot, "Imagens\\" + cliente.Foto);
            var exists = System.IO.File.Exists(imagem);
            ViewBag.ImageExist = exists;

            var listaEmprestimosAtivos = new List<SelectListItem>();
            //int IdEmprestimo = 0;

            listaEmprestimosAtivos.Add(new SelectListItem { Value = "0", Text = "Selecione um empréstimo" });

            foreach (var item in cliente.Emprestimos)
            {
                if (item.Ativo)
                {
                    listaEmprestimosAtivos.Add(new SelectListItem()
                    {
                        Value = item.Id.Value.ToString(),
                        Text = "Empréstimo de R$ " + item.Valor.ToString("N2") + " contratado em " + item.Data.Value.ToString("dd/MM/yyyy")
                    });

                    if (listaEmprestimosAtivos.Count > 1)
                        ViewBag.EmprestimosExists = exists;
                }
            }

            ViewBag.Emprestimos = listaEmprestimosAtivos;

            InformePagamentoClienteViewModel viewModel = new InformePagamentoClienteViewModel();

            viewModel.Foto = cliente.Foto;
            viewModel.Nome = cliente.Nome;
            viewModel.Limite = cliente.Limite;
            viewModel.LimiteDisponivel = cliente.LimiteDisponivel;
            viewModel.IdUsuario = cliente.Id.Value;
            viewModel.CustomMessagePartial.TipoMensagem = (int)tipoMensagem;
            viewModel.CustomMessagePartial.Mensagem = mensagem;

            return View(viewModel);
        }
        #endregion

        #region Pagamento de parcela - ListaParcela

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> ListaParcela(int? id, int? idEmprestimo, int? tipoMensagem, string? mensagem)
        {
            if (tipoMensagem == null)
                tipoMensagem = 0;

            if (mensagem == null)
                mensagem = "";

            if (id == null) return NotFound("Id nulo.");

            if (idEmprestimo == null) return NotFound("IdEmprestimo nulo.");

            var cliente = await _usuarioService.GetHistoricoById(id);

            if (cliente == null) return NotFound("Não existe cliente com esse Id.");

            return PartialView("_PagamentoParcelas", DtoToViewModelPagamento(cliente, idEmprestimo, tipoMensagem, mensagem));
        }
        #endregion

        #region Pagamento de parcela - Mostra lista de parcelas em aberto

        // estamos no métogo Get, ou seja, só mostramos a lista de parcelas em aberto
        private InformePagamentoClienteViewModel DtoToViewModelPagamento(UsuarioDTO usuarioDto, int? idEmprestimo, int? tipoMensagem, string mensagem)
        {
            InformePagamentoClienteViewModel viewModel = new InformePagamentoClienteViewModel();
            viewModel.Foto = usuarioDto.Foto;
            viewModel.Nome = usuarioDto.Nome;
            viewModel.Limite = usuarioDto.Limite;
            viewModel.LimiteDisponivel = usuarioDto.LimiteDisponivel;
            viewModel.IdUsuario = usuarioDto.Id.Value;
            viewModel.CustomMessagePartial.TipoMensagem = (int)tipoMensagem;
            viewModel.CustomMessagePartial.Mensagem = mensagem;

            foreach (var emp in usuarioDto.Emprestimos)
            {
                var objAtivo = new EmprestimoListaViewModel();

                //decimal EmprestimoInativoIdValorDasParcelas = 0;
                //int EmprestimoInativoIdNumeroDeParcelas = 0;
                //int EmprestimoInativoIdNumeroDeParcelasEmAtraso = 0;
                int QtdeParcelasPagas = 0;
                int QtdeParcelasAtrasadas = 0;

                if (emp.Ativo)
                {
                    objAtivo.Data = emp.Data.Value.ToString("dd/MM/yyyy");
                    objAtivo.Valor = emp.Valor.ToString("N2");

                    if (emp.NumeroParcelas != 0 && emp.Parcelas.Count > 0)
                    {
                        objAtivo.ValorParcela = (emp.Parcelas[0].ValorParcela).ToString("N2");
                    }

                    if (emp.Id == idEmprestimo)
                    {
                        foreach (var parc in emp.Parcelas.OrderBy(o => o.DataParcela))
                        {
                            var objParcelas = new ParcelasViewModel();
                            decimal valorPendente;
                            // dentro do loop de parcelas: acumula informações para empréstimos ativos
                            if (parc.DataPagamento != null && parc.ValorPagamento >= parc.ValorParcela)
                                QtdeParcelasPagas += 1;
                            else
                            {
                                if ((parc.DataParcela < DateTime.Today) && (!parc.ValorPagamento.HasValue || (parc.ValorPagamento < parc.ValorParcela)))    // ############# assim mesmo que faço a comparação? e as horas?
                                    QtdeParcelasAtrasadas += 1;
                            }

                            // dentro do loop de parcelas: preenche ViewModel com pacelas não quitadas
                            if (parc.ValorPagamento < parc.ValorParcela || parc.ValorPagamento == null) // parcala sem pagamento ou com valor parcial
                            {
                                objParcelas.IdParcela = parc.Id;
                                objParcelas.IdEmprestimo = parc.IdEmprestimo;
                                objParcelas.EmprestimoInativoIdData = parc.DataParcela.ToString("dd/MM/yyyy");  // View Model compartilhado, o nome da variável não ficou bom: EmprestimoInativoIdData
                                objParcelas.ValorEmprestimo = emp.Valor.ToString("N2");
                                valorPendente = Convert.ToDecimal(parc.ValorParcela) - Convert.ToDecimal(parc.ValorPagamento);
                                objParcelas.EmprestimoInativoIdValor = valorPendente.ToString("N2");

                                viewModel.PagamentoParcelas.Add(objParcelas);
                            }
                        }

                    }

                    objAtivo.QtdeParcelasPagas = QtdeParcelasPagas.ToString();
                    objAtivo.QtdeParcelasAtrasadas = QtdeParcelasAtrasadas.ToString();
                    viewModel.EmprestimosAtivos.Add(objAtivo);
                }

            }

            return viewModel;
        }

        #endregion

        #region Pagamento de parcela - ExecutaPagamento

        [HttpPost()]
        public async Task<IActionResult> ExecutaPagamento(decimal valorPago, int idEmprestimo, int UsuarioId, string Operador)
        {
            decimal emAberto = 0;
            decimal abaterLimite = valorPago;
            int tipoMensagem = 3;                                       // tipo padrão para algo inesperado
            string mensagem = "Verifique as informações fornecidas.";   // mensagem padrão para algo inesperado
            var usuarioDto = await _usuarioService.GetHistoricoById(UsuarioId);

            if (usuarioDto == null) return NotFound("Não existe cliente com esse Id.");

            foreach (var emp in usuarioDto.Emprestimos)
            {
                if (emp.Id == idEmprestimo)
                {
                    bool ativo = true;
                    var last = emp.Parcelas.Last();

                    foreach (var item in emp.Parcelas.OrderBy(o => o.DataParcela))
                    {
                        if (valorPago > 0)
                        {
                            // se a parcela mais antiga do emprestimo tem valor pendente (parcial ou nulo)
                            if (item.ValorPagamento < item.ValorParcela || item.ValorPagamento == null)
                            {
                                emAberto = Convert.ToDecimal(item.ValorParcela) - Convert.ToDecimal(item.ValorPagamento);

                                if (emAberto >= valorPago)
                                {
                                    if (item.ValorPagamento == null)
                                        item.ValorPagamento = valorPago;
                                    else
                                        item.ValorPagamento = item.ValorPagamento + valorPago;

                                    valorPago = valorPago - emAberto;

                                    if (abaterLimite == item.ValorParcela)
                                    {
                                        tipoMensagem = 1;
                                        mensagem = "Parcela paga com sucesso.";
                                    }
                                    else if (abaterLimite >= item.ValorParcela)
                                    {
                                        tipoMensagem = 1;
                                        mensagem = "Parcela paga com sucesso e outra(s) abatida(s).";
                                    }
                                    else
                                    {
                                        tipoMensagem = 2;
                                        mensagem = "Pagamento parcial.";
                                    }

                                }
                                else
                                {
                                    item.ValorPagamento = item.ValorParcela;

                                    valorPago = valorPago - emAberto;

                                    tipoMensagem = 1;
                                    mensagem = "Pagamento realizado com sucesso.";
                                }

                                // se houve alteração, atualiza a base de dados com a informação da parcela paga
                                try
                                {
                                    var parcelaDto = await _parcelaService.GetById(item.Id);

                                    parcelaDto.DataPagamento = DateTime.Now;
                                    parcelaDto.ValorPagamento = item.ValorPagamento;
                                    //                                    parcelaDto.DataAlteracao = DateTime.Now;
                                    parcelaDto.UsuarioAlteracao = Operador;

                                    await _parcelaService.Update2(parcelaDto);

                                    if (item == last && parcelaDto.ValorPagamento == parcelaDto.ValorParcela) // última parcela quitada
                                        ativo = false;
                                    // atualiza informação do empréstimo
                                }
                                catch (Exception ex)  // colocar "catch (Exception ex)" e mostar ex (mensagem de exceção
                                {
                                    return NotFound(ex);
                                }
                            }
                        }
                        else
                            break;
                    }

                    // pagamento com sucesso
                    // Se emprestimo quitado - desativar
                    if (!ativo)
                    {
                        var emprestimoDto = await _emprestimoService.GetEmprestimoById(idEmprestimo);

                        emprestimoDto.Ativo = false;

                        await _emprestimoService.Update2(emprestimoDto);

                        if (valorPago == 0)
                        {
                            tipoMensagem = 1;
                            mensagem = "Empréstimo quitado.";
                        }
                        else
                        {
                            tipoMensagem = 2;
                            mensagem = "Sobrou R$ " + valorPago.ToString("N2") + " após a quitação do empréstimo.";
                            // valorPago disponível = valorDisponivel - valorPago , pois está inadvertidamente abatendo o valor do troco no limite disponivel
                            // resolvido na próxima linha
                        }
                    }
                }
            }

            //atualiza limite disponível com o valor pago
            if (valorPago < 0) valorPago = 0; // se faltou valor (valorPago negativo) não deve incrementar o Limite Disponível

            usuarioDto.LimiteDisponivel = usuarioDto.LimiteDisponivel + abaterLimite - valorPago;   //// abater eventual troco, aqui o valorPago é uma eventual sobra (troco)

            if (usuarioDto.LimiteDisponivel > usuarioDto.Limite) usuarioDto.LimiteDisponivel = usuarioDto.Limite; // só por garantia em arredondamentos

            await _usuarioService.Update2(usuarioDto);

            return RedirectToAction("Pagamento", new { id = UsuarioId, tipoMensagem = tipoMensagem, mensagem = mensagem });
        }
        #endregion

        #region EnviarArquivo
        public string SobeArquivo(IFormFile arquivo)
        {
            //verifica se foi passado arquivo
            if (arquivo == null || arquivo.Length == 0)
            {
                //retorna a viewdata com erro
                //ViewData["Erro"] = "Error: Arquivo não selecionado";
                //return View(ViewData);
                return "Imagem não informada";
            }
            //verifica o tipo de arquivo
            if (arquivo.FileName.Contains(".jpg") || arquivo.FileName.Contains(".gif") ||
                arquivo.FileName.Contains(".png") || arquivo.FileName.Contains(".jpeg"))
            {
                //< obtém o caminho físico da pasta wwwroot >
                string caminho_WebRoot = _appEnvironment.WebRootPath;

                // monta o caminho onde vamos salvar o arquivo : 
                string caminhoDestinoArquivo = caminho_WebRoot + "\\Imagens\\";

                // incluir a pasta o nome do arquivo enviado : 
                string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + arquivo.FileName;
                
                //copia o arquivo para o local de destino original, mas renomeia caso já exista
                while (System.IO.File.Exists(caminhoDestinoArquivoOriginal))
                {
                    string extensao = caminhoDestinoArquivoOriginal.Substring(caminhoDestinoArquivoOriginal.Length - 4, 4);
                    string nome = caminhoDestinoArquivoOriginal.Substring(0, caminhoDestinoArquivoOriginal.Length - 4);
                    nome += DateTime.Now.ToString("yyyyMMddHHmmss");
                    caminhoDestinoArquivoOriginal = nome + extensao;
                }
                //salva o arquivo
                using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
                {
                    arquivo.CopyToAsync(stream);
                }

                int a = caminhoDestinoArquivo.Length;
                int b = caminhoDestinoArquivoOriginal.Length;

                string nomeFoto = caminhoDestinoArquivoOriginal.Substring(a, b - a);

                return (nomeFoto);
            }
            else
            {
                return "Tipo de arquivo inválido";
            }
        }
        #endregion
    }
}
