using apiGBM.Infra;
using apiGBM.Models;
using apiGBM.View;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace apiGBM.Controllers
{
    [ApiController]
    [Route("api/Motorista")]
    public class MotoristaController : ControllerBase
    {
        private readonly IMotoristaRepository _motoristaRepository;
        private readonly ICaminhaoRepository _caminhaoRepository;

        public MotoristaController(IMotoristaRepository motoristaRepository,ICaminhaoRepository caminhaoRepository)

        {   
            _motoristaRepository = motoristaRepository ?? throw new ArgumentNullException();
            _caminhaoRepository = caminhaoRepository ?? throw new ArgumentNullException();
        }
        
        /// <summary>
        /// Obter todos os motoristas cadastrados
        /// </summary>
        /// <returns>Motoristas cadastrados</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var motorista = _motoristaRepository.GetAll();    // Chama o repositório para obter todos os motoristas
            return Ok(motorista);
        }


        /// <summary>
        /// Obter dados de um motorista pelo CPF
        /// </summary>
        /// <param name="cd_CPF">Identificador</param>
        /// <returns>Dados do motorista que pertence ao CPF informado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{cd_CPF}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(string cd_CPF)
        {
            var motorista = _motoristaRepository.GetByCPF(cd_CPF);  // Chama o repositório para obter um motorista específico pelo CPF
            if (motorista == null)
            {
                return NotFound("CPF NÃO CADASTRADO"); // RETORNA "NÃO ENCONTRADO" SE NÃO EXISTIR
            }
            return Ok(motorista);
        }


        /// <summary>
        /// Adicionar um motorista
        /// </summary>
        /// <param name="motoristaView">Dados do motorista</param>
        /// <returns>Objeto do Motorista Criado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="409">Conflito</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Add(MotoristaView motoristaView)
        {
            if(motoristaView.nm_Nome.Length > 250 || String.IsNullOrEmpty(motoristaView.nm_Nome))
            {
                return Conflict("Nome Inválido");
            }
            if(motoristaView.cd_CPF.Length != 11)
            {
                return Conflict("CPF inválido");
            }
            if (motoristaView.ds_CategoriaCNH.Length > 2 || String.IsNullOrEmpty(motoristaView.ds_CategoriaCNH))
            {
                return Conflict("Categoria Inválida");
            }
            if(motoristaView.dt_Nascimento.Length > 10 || String.IsNullOrEmpty(motoristaView.dt_Nascimento))
            {
                return Conflict("Data de Nascimento Inválida");
            }
            if(motoristaView.ds_Telefone.Length > 15 || String.IsNullOrEmpty(motoristaView.ds_Telefone))
            {
                return Conflict("Telefone Inválido");
            }

            var verificarID = _motoristaRepository.GetByCPF(motoristaView.cd_CPF);    // Verifica se o CPF já está cadastrado
            if (verificarID != null)
            {
                return Conflict("CPF JÁ EXISTENTE");
            }

            // Cria um novo objeto Motorista com os dados recebidos
            var motorista = new Motorista(motoristaView.cd_CPF, motoristaView.nm_Nome, motoristaView.ds_CategoriaCNH.ToUpper(), motoristaView.dt_Nascimento, motoristaView.ds_Telefone);
            _motoristaRepository.Add(motorista);
            return Ok("MOTORISTA CADASTRADO COM SUCESSO");
        }


        /// <summary>
        /// Atualiza Informações do motorista
        /// </summary>
        /// <param name="cd_CPF">Identificador do motorista</param>
        /// <param name="motoristaView">Dados do motorista</param>
        /// <returns>Nada.</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="409">Conflito</response>
        /// <response code="404">Não Encontrado</response>
        [HttpPut("{cd_CPF}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(string cd_CPF, MotoristaView motoristaView)
        {
            var motorista = _motoristaRepository.GetByCPF(cd_CPF);  // Chama o repositório para obter o motorista específico pelo CPF
            if (motorista == null)
            {
                return NotFound("CPF NÃO CADASTRADO"); // RETORNA "NÃO ENCONTRADO" SE NÃO EXISTIR
            }

            if (motoristaView.nm_Nome.Length > 250 || String.IsNullOrEmpty(motoristaView.nm_Nome))
            {
                return Conflict("Nome inválido");
            }
            if (motoristaView.cd_CPF.Length != 11)
            {
                return Conflict("CPF inválido");
            }
            if (motoristaView.ds_CategoriaCNH.Length > 2 || String.IsNullOrEmpty(motoristaView.ds_CategoriaCNH))
            {
                return Conflict("Categoria Inválida");
            }
            if (motoristaView.dt_Nascimento.Length > 10 || String.IsNullOrEmpty(motoristaView.dt_Nascimento))
            {
                return Conflict("Data de Nascimento Inválida");
            }
            if (motoristaView.ds_Telefone.Length > 15 || String.IsNullOrEmpty(motoristaView.ds_Telefone))
            {
                return Conflict("Telefone Inválido");
            }

            // Atualiza os dados do motorista com as novas informações recebidas
            motorista.nm_Nome = motoristaView.nm_Nome;
            motorista.ds_CategoriaCNH = motoristaView.ds_CategoriaCNH;
            motorista.dt_Nascimento = motoristaView.dt_Nascimento;
            motorista.ds_Telefone = motoristaView.ds_Telefone;

            _motoristaRepository.Update(motorista);
            return Ok("Motorista atualizado com sucesso.");
        }


        /// <summary>
        /// Deletar um motorista
        /// </summary>
        /// <param name="cd_CPF">Identificador do motorista</param>
        /// <returns>Nada.</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpDelete("{cd_CPF}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string cd_CPF)
        {
            var motorista = _motoristaRepository.GetByCPF(cd_CPF); // Chama o repositório para obter o motorista específico pelo CPF
            if (motorista == null)
            {
                return NotFound("CPF NÃO CADASTRADO"); // RETORNA "NÃO ENCONTRADO" SE NÃO EXISTIR
            }

            var caminhaomotorista = _caminhaoRepository.GetByCPF(cd_CPF);
            if(caminhaomotorista != null) 
            {
                return Conflict("Não foi possivel deletar, o motorista possue um caminhão cadastrado");
            } 

            // Chama o repositório para deletar o motorista do banco de dados
            _motoristaRepository.Delete(motorista);
            return Ok("Motorista deletado com sucesso");
        }
    }
}
