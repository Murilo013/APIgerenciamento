using apiGBM.View;
using apiGBM.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiGBM.Controllers
{
    [ApiController]
    [Route("api/Entrega")]
    public class EntregaController : ControllerBase
    {
        private readonly IEntregaRepository _entregaRepository;
        private readonly ICaminhaoRepository _caminhaoRepository;
        private readonly IMotoristaRepository _motoristaRepository;

        public EntregaController(ICaminhaoRepository caminhaoRepository, IMotoristaRepository motoristaRepository, IEntregaRepository entregaRepository)
        {
            _entregaRepository = entregaRepository ?? throw new ArgumentException();
            _caminhaoRepository = caminhaoRepository ?? throw new ArgumentNullException();
            _motoristaRepository = motoristaRepository ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Obter todos as entregas 
        /// </summary>
        /// <returns>Entregas cadastradas </returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var entrega = _entregaRepository.GetAll();    // Chama o repositório para obter todos os motoristas
            return Ok(entrega);
        }

        /// <summary>
        /// Obter uma entrega pelo id
        /// </summary>
        /// <param name="cd_entrega"></param>
        /// <returns>Objeto Entrega</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{cd_entrega}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid cd_entrega)
        { 
            var entrega = _entregaRepository.GetById(cd_entrega);
            if(entrega == null)
            {
                return NotFound("Entrega não encontrada");
            }
            return Ok(entrega); 
        }

        /// <summary>
        /// Adicionar Entrega
        /// </summary>
        /// <returns>Entrega Criada</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="409">Conflito</response>
        /// <response code="404">Não Encontrado</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Add(EntregaView entregaView)
        {
            if (entregaView.ds_Origem.Length > 50 || String.IsNullOrEmpty(entregaView.ds_Origem))
            {
                return Conflict("Origem Inválida");
            }
            if(entregaView.ds_Destino.Length > 50 || String.IsNullOrEmpty(entregaView.ds_Destino))
            {
                return Conflict("Destino Inválido");
            }
            if (entregaView.ds_CargaTransportada.Length > 50 || String.IsNullOrEmpty(entregaView.ds_CargaTransportada))
            {
                return Conflict("Carga Inválida");
            }
            if (entregaView.fk_PlacaCaminhao.Length != 7)
            {
                return Conflict("Placa Inválida");
            }
            if (entregaView.ds_StatusEntrega.ToUpper() != "CRIADA" && entregaView.ds_StatusEntrega.ToUpper() != "EM EXECUÇÃO" && entregaView.ds_StatusEntrega.ToUpper() != "FINALIZADA" && entregaView.ds_StatusEntrega.ToUpper() != "CANCELADA")
            {
                return Conflict("Status Inválido... --> CRIADA, EM EXECUÇÃO, FINALIZADA, CANCELADA");
            }
            if (entregaView.dt_Entrega.Length < 10 || String.IsNullOrEmpty(entregaView.dt_Entrega))
            {
                return Conflict("Data de entrega Inválida");
            }
            if (entregaView.fk_CPFmotorista.Length != 11)
            { 
                return Conflict("CPF Inválido");
            }
            var motorista = _motoristaRepository.GetByCPF(entregaView.fk_CPFmotorista);
            if(motorista == null)
            {
                return NotFound("CPF NÃO EXISTENTE");
            }

            var caminhaoMotorista = _caminhaoRepository.GetByCPF(entregaView.fk_CPFmotorista);
            if(caminhaoMotorista == null)
            {
                return Conflict("ESSE MOTORISTA NÃO POSSUE UM CAMINHÃO ASSOCIADO");
            }

            if (motorista.fk_PlacaCaminhao != entregaView.fk_PlacaCaminhao)
            { 
                return Conflict("PLACA NÃO PERTENCE AO CPF");
            }

            var entregasMotorista = _entregaRepository.GetByCPF(entregaView.fk_CPFmotorista);

            if (entregasMotorista != null && entregasMotorista.Any())
            {
                foreach (var entregas in entregasMotorista)
                {
                    if (entregas.ds_StatusEntrega == "CRIADA" && entregas.dt_Entrega == entregaView.dt_Entrega)
                    {
                        return Conflict($"Motorista já possui uma entrega na data de {entregaView.dt_Entrega}");
                    }
                    if (entregas.ds_StatusEntrega != "FINALIZADA" && entregas.ds_StatusEntrega != "CANCELADA" && entregas.ds_StatusEntrega != "CRIADA")
                    {
                        return Conflict($"Motorista já possui uma entrega {entregas.ds_StatusEntrega}");
                    }
                }
            }


            var entrega = new Entrega(entregaView.dt_Entrega, entregaView.ds_Origem, entregaView.ds_Destino, entregaView.ds_CargaTransportada,entregaView.ds_StatusEntrega.ToUpper(),entregaView.fk_PlacaCaminhao, entregaView.fk_CPFmotorista);

            _entregaRepository.Add(entrega);
            return Ok("Entrega Cadastrada");
        }

        /// <summary>
        /// Atualizar entrega
        /// </summary>
        /// <param name="cd_entrega"></param>
        /// <param name="entregaView"></param>
        /// <returns>Entrega atualizada</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="409">Conflito</response>
        [HttpPut("{cd_entrega}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(Guid cd_entrega, EntregaView entregaView)
        {
            var entrega = _entregaRepository.GetById(cd_entrega);

            if (entregaView.ds_Origem.Length > 50 || String.IsNullOrEmpty(entregaView.ds_Origem))
            {
                return Conflict("Origem Inválida");
            }
            if (entregaView.ds_Destino.Length > 50 || String.IsNullOrEmpty(entregaView.ds_Destino))
            {
                return Conflict("Destino Inválido");
            }
            if (entregaView.ds_CargaTransportada.Length > 50 || String.IsNullOrEmpty(entregaView.ds_CargaTransportada))
            {
                return Conflict("Carga Inválida");
            }
            if (entregaView.ds_StatusEntrega.ToUpper() != "CRIADA" && entregaView.ds_StatusEntrega.ToUpper() != "EM EXECUÇÃO" && entregaView.ds_StatusEntrega.ToUpper() != "FINALIZADA" && entregaView.ds_StatusEntrega.ToUpper() != "CANCELADA")
            {
                return Conflict("Status Inválido... --> CRIADA, EM EXECUÇÃO, FINALIZADA, CANCELADA");
            }
            if (entregaView.fk_PlacaCaminhao.Length != 7)
            {
                return Conflict("Placa Inválida");
            }
            if (entregaView.dt_Entrega.Length < 10)
            {
                return Conflict("Data de entrega Inválida");
            }
            if (entregaView.fk_CPFmotorista.Length != 11)
            {
                return Conflict("CPF Inválido");
            }

            var entregasMotorista = _entregaRepository.GetByCPF(entregaView.fk_CPFmotorista);

            if (entregasMotorista != null && entregasMotorista.Any())
            {
                foreach (var entregas in entregasMotorista)
                {
                    if (entregas.ds_StatusEntrega == "CRIADA" && entregas.dt_Entrega == entregaView.dt_Entrega && entregas.cd_Entrega != entrega.cd_Entrega)
                    {
                        return Conflict($"Motorista já possui uma entrega na data de {entregaView.dt_Entrega}");
                    }
                    if (entregas.ds_StatusEntrega != "FINALIZADA" && entregas.ds_StatusEntrega != "CANCELADA" && entregas.ds_StatusEntrega != "CRIADA" && entregas.cd_Entrega != entrega.cd_Entrega)
                    {
                        return Conflict($"Motorista já possui uma entrega {entregas.ds_StatusEntrega}");
                    }
                }
            }

            entrega.ds_Origem = entregaView.ds_Origem;
            entrega.ds_Destino = entregaView.ds_Destino;
            entrega.ds_CargaTransportada = entregaView.ds_CargaTransportada;
            entrega.ds_StatusEntrega = entregaView.ds_StatusEntrega.ToUpper();
            entrega.fk_PlacaCaminhao = entregaView.fk_PlacaCaminhao;
            entrega.dt_Entrega = entregaView.dt_Entrega;
            entrega.fk_CPFmotorista = entregaView.fk_CPFmotorista;

            _entregaRepository.Update(entrega);
            return Ok("Entrega Atualizada");
        }

        /// <summary>
        /// Deletar uma entrega
        /// </summary>
        /// <param name="cd_Entrega">Identificador da entrega</param>
        /// <returns>Nada.</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="409">Conflito</response>
        [HttpDelete("{cd_Entrega}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Delete(Guid cd_Entrega)
        {
            var entrega = _entregaRepository.GetById(cd_Entrega); // Chama o repositório para obter o motorista específico pelo CPF
            if (entrega == null)
            {
                return NotFound("Entrega não encontrada"); // RETORNA "NÃO ENCONTRADO" SE NÃO EXISTIR
            }
            if(entrega.ds_StatusEntrega == "EM EXECUÇÃO")
            {
                return Conflict("A entrega está EM EXECUÇÃO");
            }
            // Chama o repositório para deletar o motorista do banco de dados
            _entregaRepository.Delete(entrega);
            return Ok("Entrega deletada com sucesso");
        }

    }
}
