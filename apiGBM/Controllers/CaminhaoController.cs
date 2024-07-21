using apiGBM.Infra;
using apiGBM.Models;
using apiGBM.View;
using Microsoft.AspNetCore.Mvc;

namespace apiGBM.Controllers
{
    [ApiController]
    [Route("api/Caminhao")]
    public class CaminhaoController : ControllerBase
    {
        int anoAtual = DateTime.Now.Year;
        private readonly ICaminhaoRepository _caminhaoRepository;
        private readonly IMotoristaRepository _motoristaRepository;
        private readonly IEntregaRepository _entregaRepository;

        public CaminhaoController(ICaminhaoRepository caminhaoRepository, IMotoristaRepository motoristaRepository,IEntregaRepository entregaRepository)
        {
            _caminhaoRepository = caminhaoRepository ?? throw new ArgumentNullException(); 
            _motoristaRepository = motoristaRepository ?? throw new ArgumentNullException();
            _entregaRepository = entregaRepository ?? throw new ArgumentNullException();
        }


        
        /// <summary>
        /// Obter todos os caminhões cadastrados
        /// </summary>
        /// <returns>Caminhões cadastrados</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var caminhao = _caminhaoRepository.GetAll();  // Chama o repositório para obter todos os motoristas
            return Ok(caminhao);
        }


        /// <summary>
        /// Obter dados de um caminhao
        /// </summary>
        /// <param name="cd_Placa">Identificador</param>
        /// <returns>Dados do caminhão que pertence a placa informado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{cd_Placa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(string cd_Placa)
        {
            var caminhao = _caminhaoRepository.GetByPlaca(cd_Placa);  // Chama o repositório para obter um caminhão específico pela placa
            if (caminhao == null)
            {
                return NotFound("CAMINHÃO NÃO CADASTRADO"); // RETORNA "NÃO ENCONTRADO" SE NÃO EXISTIR
            }
            return Ok(caminhao);
        }
        

        /// <summary>
        /// Adicionar um caminhão
        /// </summary>
        /// <param name="caminhaoView">Dados do caminhão</param>
        /// <returns>Objeto do caminhão Criado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="409">Conflito</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Add(CaminhaoView caminhaoView)
        {
            if (caminhaoView.cd_Placa.Length != 7)
            {
                return Conflict("Placa Inválida");
            }
            if (caminhaoView.ds_Modelo.Length > 30 || String.IsNullOrEmpty(caminhaoView.ds_Modelo))
            {
                return Conflict("Modelo inválido");
            }
            if (caminhaoView.aa_Fabricacao > anoAtual || caminhaoView.aa_Fabricacao < 1700 )
            {
                return Conflict("Ano Inválido");
            }
            if (caminhaoView.ds_CorPrincipal.Length > 20 || String.IsNullOrEmpty(caminhaoView.ds_CorPrincipal))
            {
                return Conflict("Cor Inválida");
            }
            if (caminhaoView.qt_EixosTracao > 6 || caminhaoView.qt_EixosTracao < 0)
            {
                return Conflict("Quantidade de Eixos Inválida");
            }
            if (caminhaoView.fk_CPFmotorista.Length != 11)
            {
                return Conflict("CPF do motorista Inválido");
            }

            var verificarPlaca = _caminhaoRepository.GetByPlaca(caminhaoView.cd_Placa);    // Verifica se a placa já está cadastrado
            if (verificarPlaca != null)
            {
                return Conflict("PLACA JÁ EXISTENTE");
            }


            var verificarAtribuicao = _caminhaoRepository.GetByCPF(caminhaoView.fk_CPFmotorista);
            if (verificarAtribuicao != null)
            {
                return Conflict("CPF JÁ ATRIBUIDO A OUTRO CAMINHÃO");
            }

            var verificarCPF = _motoristaRepository.GetByCPF(caminhaoView.fk_CPFmotorista);
            if (verificarCPF == null)
            {
                return Conflict("CPF NÃO EXISTENTE");
            }
            
            // Cria um novo objeto Caminhão com os dados recebidos
            var caminhao = new Caminhao(caminhaoView.cd_Placa, caminhaoView.ds_Modelo, caminhaoView.aa_Fabricacao, caminhaoView.ds_CorPrincipal, caminhaoView.qt_EixosTracao, caminhaoView.fk_CPFmotorista);


            // Atribui a placa do caminhão para o CPF informado
            verificarCPF.fk_PlacaCaminhao = caminhaoView.cd_Placa;
            _motoristaRepository.Update(verificarCPF);

            _caminhaoRepository.Add(caminhao);
            return Ok("CAMINHÃO CADASTRADO COM SUCESSO");
        }


        /// <summary>
        /// Atualiza Informações do caminhão
        /// </summary>
        /// <param name="cd_Placa">Identificador do Caminhão</param>
        /// <param name="caminhaoView">Dados do Caminhão</param>
        /// <returns>Nada.</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não Encontrado</response>
        /// <response code="409">Conflito</response>
        [HttpPut("{cd_Placa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(string cd_Placa, CaminhaoView caminhaoView)
        {
            var caminhao = _caminhaoRepository.GetByPlaca(cd_Placa);  // Chama o repositório para obter o caminhão específico pela placa
            if (caminhao == null)
            {
                return NotFound("PLACA NÃO CADASTRADA"); // RETORNA "NÃO ENCONTRADO" SE NÃO EXISTIR
            }

            if (caminhaoView.ds_Modelo.Length > 30 || String.IsNullOrEmpty(caminhaoView.ds_Modelo))
            {
                return Conflict("Modelo inválido");
            }
            if (caminhaoView.aa_Fabricacao > anoAtual || caminhaoView.aa_Fabricacao < 1700)
            {
                return Conflict("Ano Inválido");
            }
            if (caminhaoView.ds_CorPrincipal.Length > 20 || String.IsNullOrEmpty(caminhaoView.ds_CorPrincipal))
            {
                return Conflict("Cor Inválida");
            }
            if (caminhaoView.qt_EixosTracao > 6 || caminhaoView.qt_EixosTracao < 0)
            {
                return Conflict("Quantidade de Eixos Inválida");
            }
            if (caminhaoView.fk_CPFmotorista.Length != 11)
            {
                return Conflict("CPF do motorista Inválido");
            }

            var verificarPlaca = _caminhaoRepository.GetByPlaca(caminhaoView.cd_Placa);    // Verifica se a placa já está cadastrado
            if (verificarPlaca != null)
            {
                return Conflict("PLACA JÁ EXISTENTE");
            }


            var verificarAtribuicao = _caminhaoRepository.GetByCPF(caminhaoView.fk_CPFmotorista);
            if (verificarAtribuicao != null)
            {
                return Conflict("CPF JÁ ATRIBUIDO A OUTRO CAMINHÃO");
            }

            var verificarCPF = _motoristaRepository.GetByCPF(caminhaoView.fk_CPFmotorista);
            if (verificarCPF == null)
            {
                return Conflict("CPF NÃO EXISTENTE");
            }

            // Atualiza os dados do motorista com as novas informações recebidas
            caminhao.ds_Modelo = caminhaoView.ds_Modelo;
            caminhao.aa_Fabricacao = caminhaoView.aa_Fabricacao;
            caminhao.ds_CorPrincipal = caminhaoView.ds_CorPrincipal;
            caminhao.qt_EixosTracao = caminhaoView.qt_EixosTracao;
            caminhao.fk_CPFmotorista = caminhaoView.fk_CPFmotorista;

            _caminhaoRepository.Update(caminhao);
            return Ok("Caminhão atualizado com sucesso.");
        }


        /// <summary>
        /// Deleta um Caminhão
        /// </summary>
        /// <param name="cd_Placa">Identificador do caminhão</param>
        /// <returns>Nada.</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="409">Conflito</response>
        [HttpDelete("{cd_Placa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Delete(string cd_Placa)
        {
            var caminhao = _caminhaoRepository.GetByPlaca(cd_Placa);
            if (caminhao == null)
            {
                return NotFound("PLACA NÃO CADASTRADA"); // RETORNA "NÃO ENCONTRADO" SE NÃO EXISTIR
            }

            // REMOVE A ASSOCIAÇÃO DO CAMINHÃO COM O MOTORISTA
            var motorista = _motoristaRepository.GetByCPF(caminhao.fk_CPFmotorista);
            motorista.fk_PlacaCaminhao = null;
            _motoristaRepository.Update(motorista);

            var entregacaminhao = _entregaRepository.GetByPlaca(cd_Placa);
            if(entregacaminhao != null)
            {
                return Conflict("Não é possível deletar o caminhão porque o mesmo tem um registro de entrega");
            }

            // Chama o repositório para deletar o motorista do banco de dados
            _caminhaoRepository.Delete(caminhao);
            return Ok("Caminhão deletado com sucesso");
        }
    }
}

    

