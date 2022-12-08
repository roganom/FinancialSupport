using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using FinancialSupport.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FinancialSupport.WebUI.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmprestimoService _emprestimoService;
        private readonly IParcelaService _parcelaService;
        private readonly IWebHostEnvironment _environment;
        public ClienteController(IUsuarioService usuarioService, IWebHostEnvironment environment, IEmprestimoService emprestimo, IParcelaService parcelaService)
        {
            _usuarioService = usuarioService;
            _parcelaService = parcelaService;
            _emprestimoService = emprestimo;
            _environment = environment;
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
        public async Task<IActionResult> Create(UsuarioDTO usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuarioService.Add(usuario);
                return RedirectToAction(nameof(Index));
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

            var wwwroot = _environment.WebRootPath;
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
            viewModel.Limite = (decimal)usuarioDto.Limite;
            viewModel.LimiteDisponivel = (decimal)usuarioDto.LimiteDisponivel;

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

            return viewModel;
        }
        #endregion

        #region Novo Empréstimo
        [HttpGet]
        public async Task<IActionResult> Novo(int? id)
        {
            if (id == null) return NotFound();

            var usuarioDto = await _usuarioService.GetById(id);

            if (usuarioDto == null) return NotFound();

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

                retorno = await _emprestimoService.CriaNovoEmprestimo(novoEmpestimoViewModel.IdUsuario, novoEmpestimoViewModel.Valor, novoEmpestimoViewModel.NumeroParcelas, 1, true);

                novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = retorno.Key;
                novoEmpestimoViewModel.CustomMessagePartial.Mensagem = retorno.Value;

                //return RedirectToAction("Novo", new { novoEmpestimoViewModel });
                return View(novoEmpestimoViewModel);
            }
            novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = 3;
            novoEmpestimoViewModel.CustomMessagePartial.Mensagem = "Invalid ModelState!";

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Simular(NovoEmpestimoViewModel novoEmpestimoViewModel)
        {
            if (ModelState.IsValid)
            {
                KeyValuePair<int, string> retorno;

                retorno = await _emprestimoService.CriaNovoEmprestimo(novoEmpestimoViewModel.IdUsuario, novoEmpestimoViewModel.Valor, novoEmpestimoViewModel.NumeroParcelas, 1, false);

                novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = retorno.Key;
                novoEmpestimoViewModel.CustomMessagePartial.Mensagem = retorno.Value;

//                return RedirectToAction("Novo.cshtml");
                return View(novoEmpestimoViewModel);
            }
            novoEmpestimoViewModel.CustomMessagePartial.TipoMensagem = 3;
            novoEmpestimoViewModel.CustomMessagePartial.Mensagem = "Invalid ModelState!";

            return NotFound();
        }
        #endregion

        #region Pagamento de parcela
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> Pagamento(int? id, int? tipoMensagem, string? mensagem)
        {
            //var customMessagePartial = new CustomMessagePartialViewModel();

            if (tipoMensagem == null)
                tipoMensagem = 0;

            if (mensagem == null)
                mensagem = "";

            //customMessagePartial.TipoMensagem = (int)tipoMensagem;
            //customMessagePartial.Mensagem = mensagem;

            if (id == null) return NotFound("Id nulo.");

            var cliente = await _usuarioService.GetHistoricoById(id);

            if (cliente == null) return NotFound("Não existe cliente com esse Id.");

            var wwwroot = _environment.WebRootPath;
            var imagem = Path.Combine(wwwroot, "Imagens\\" + cliente.Foto);
            var exists = System.IO.File.Exists(imagem);
            ViewBag.ImageExist = exists;

            return View(DtoToViewModelPagamento(cliente, tipoMensagem, mensagem));
        }

        // estamos no métogo Get, ou seja, só mostramos a lista de parcelas em aberto
        private InformePagamentoClienteViewModel DtoToViewModelPagamento(UsuarioDTO usuarioDto, int? tipoMensagem, string mensagem)
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
                        objAtivo.ValorParcela = (emp.Parcelas[0].ValorParcela).ToString("N2");
                    }

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

                    objAtivo.QtdeParcelasPagas = QtdeParcelasPagas.ToString();
                    objAtivo.QtdeParcelasAtrasadas = QtdeParcelasAtrasadas.ToString();

                    viewModel.EmprestimosAtivos.Add(objAtivo);
                }
            }

            return viewModel;
        }

        [HttpPost()]//, ActionName("Pagamento")]
        public async Task<IActionResult> ExecutaPagamento(decimal valorPago, int idEmprestimo, int UsuarioId)
        {
            decimal emAberto = 0;
            decimal abaterLimite = valorPago;
            int tipoMensagem = 3;
            string mensagem = "Verifique as informações fornecidas.";

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
                                        tipoMensagem = 2;
                                        mensagem = "Parcela paga com sucesso e outra abatida.";
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
                            mensagem = "Sobrou valor após a quitação do empréstimo.";
                        }
                    }
                }
            }

            //atualiza limite disponível com o valor pago
            usuarioDto.LimiteDisponivel = usuarioDto.LimiteDisponivel + abaterLimite;   //// abater eventual troco
            await _usuarioService.Update2(usuarioDto);

            return RedirectToAction("Pagamento", new { id = UsuarioId, tipoMensagem = tipoMensagem, mensagem = mensagem });
        }
        #endregion
    }
}
