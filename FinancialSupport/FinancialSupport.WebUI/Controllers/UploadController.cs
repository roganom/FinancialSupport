using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FinancialSupport.WebUI.Controllers
{
    public class UploadController : Controller
    {
        //Define uma instância de IHostingEnvironment
        IHostingEnvironment _appEnvironment;
        //Injeta a instância no construtor para poder usar os recursos
        public UploadController(IHostingEnvironment env)
        {
            _appEnvironment = env;
        }
        //Retorna a View Index.cshtml que será o formulário para
        //selecionar os arquivos a serem enviados 
        public IActionResult Index()
        {
            return View();
        }
        //método para enviar os arquivos usando a interface IFormFile
        public async Task<IActionResult> EnviarArquivo(IFormFile arquivo)
        {
            //verifica se foi passado arquivo
            if (arquivo == null || arquivo.Length == 0)
            {
                //retorna a viewdata com erro
                ViewData["Erro"] = "Error: Arquivo não selecionado";
                return View(ViewData);
            }
            //verifica o tipo de arquivo
            if (arquivo.FileName.Contains(".jpg") || arquivo.FileName.Contains(".gif") ||
                arquivo.FileName.Contains(".png") || arquivo.FileName.Contains(".jpeg"))
            {
                //< obtém o caminho físico da pasta wwwroot >
                string caminho_WebRoot = _appEnvironment.WebRootPath;
                // monta o caminho onde vamos salvar o arquivo : 
                // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos
                string caminhoDestinoArquivo = caminho_WebRoot + "\\Imagens\\";
                // incluir a pasta Recebidos e o nome do arquivo enviado : 
                // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos\
                string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + arquivo.FileName;
                //copia o arquivo para o local de destino original

                while (System.IO.File.Exists(caminhoDestinoArquivoOriginal))
                {
                    string extensao = caminhoDestinoArquivoOriginal.Substring(caminhoDestinoArquivoOriginal.Length - 4, 4);
                    string nome = caminhoDestinoArquivoOriginal.Substring(0, caminhoDestinoArquivoOriginal.Length - 4);
                    //var agora = string.Format()
                    nome += DateTime.Now.ToString("yyyyMMddHHmmss");
                    caminhoDestinoArquivoOriginal = nome + extensao;
                }

                using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                int a = caminhoDestinoArquivo.Length;
                int b = caminhoDestinoArquivoOriginal.Length;

                string nomeFoto = caminhoDestinoArquivoOriginal.Substring(a, b - a);

                //monta a ViewData que será exibida na view como resultado do envio 
                ViewData["Resultado"] = $"Arquivo carregado com sucesso e salvo como  {nomeFoto}  .";
                //retorna a viewdata
                //return RedirectToAction(nameof(Create));
                return View(ViewData);
            }
            else
            {
                //retorna a viewdata com erro
                ViewData["Erro"] = "Error: Tipo de arquivo inválido";
                return View(ViewData);
            }
        }
    }
}
