using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private static List<Pessoa> pessoas = new List<Pessoa>();

        // Rota de autenticação
        [HttpPost("autenticar")]
        public IActionResult Autenticar()
        {
            // Supondo que a autenticação foi bem-sucedida, poderíamos gerar um token JWT e retorná-lo aqui
            string token = "token_de_exemplo";
            return Ok(new { Token = token });
        }

        // Rota para consulta de todas as pessoas
        [HttpGet]
        
        public IActionResult ConsultarPessoas()
        {
            return Ok(pessoas);
        }

        // Rota para consulta de uma pessoa pelo código
        [HttpGet("{codigo}")]
        
        public IActionResult ConsultarPessoaPorCodigo(long codigo)
        {
            var pessoa = pessoas.Find(p => p.Codigo == codigo);

            if (pessoa == null)
            {
                return NotFound("Pessoa não encontrada.");
            }

            return Ok(pessoa);
        }


        // Rota para consulta de pessoas por UF
        [HttpGet("por-uf/{uf}")]
        
        public IActionResult ConsultarPessoasPorUF(string uf)
        {
            var pessoasPorUF = pessoas.FindAll(p => p != null && p.UF != null && p.UF.Equals(uf, StringComparison.OrdinalIgnoreCase));

            if (pessoasPorUF.Count == 0)
            {
                return NotFound("Nenhuma pessoa encontrada com a UF especificada.");
            }

            return Ok(pessoasPorUF);
        }


        // Rota para gravar uma nova pessoa, que deve retornar o objeto “salvo”;
        // O método deve ser capaz de validar as informações recebidas;
        [HttpPost]
        
        public IActionResult GravarPessoa(Pessoa pessoa)
        {
            // Verifica se a pessoa já existe na lista
            bool pessoaExiste = pessoas.Any(p => p != null && p.Nome != null && p.Nome.Equals(pessoa.Nome, StringComparison.OrdinalIgnoreCase)
                                                && p != null && p.CPF != null && p.CPF.Equals(pessoa.CPF, StringComparison.OrdinalIgnoreCase)
                                                && p != null && p.UF != null && p.UF.Equals(pessoa.UF, StringComparison.OrdinalIgnoreCase));

            if (pessoaExiste)
            {
                return BadRequest("Já existe uma pessoa com essas informações.");
            }

            // Simulando validação de informações recebidas
            if (string.IsNullOrEmpty(pessoa.Nome) || string.IsNullOrEmpty(pessoa.CPF) || string.IsNullOrEmpty(pessoa.UF))
            {
                return BadRequest("Nome, CPF e UF são campos obrigatórios.");
            }

            // Apenas para simular, em uma aplicação real, isso seria gerado pelo banco de dados.
            pessoa.Codigo = pessoas.Count + 1; 
            pessoas.Add(pessoa);
            return Ok(pessoa);
        }


        // Rota para atualizar os dados de uma pessoa
        [HttpPut("{codigo}")]
        
        public IActionResult AtualizarPessoa(long codigo, Pessoa pessoaAtualizada)
        {
            var pessoa = pessoas.Find(p => p.Codigo == codigo);
            if (pessoa == null)
                return NotFound();

            // Verifica se já existe outra pessoa com os mesmos dados (nome, CPF, UF)
            var pessoaDuplicada = pessoas.FirstOrDefault(p => p.Codigo != codigo &&
                                                              p != null && p.Nome != null && p.Nome.Equals(pessoaAtualizada.Nome, StringComparison.OrdinalIgnoreCase) &&
                                                              p != null && p.CPF != null && p.CPF.Equals(pessoaAtualizada.CPF, StringComparison.OrdinalIgnoreCase) &&
                                                              p != null && p.UF != null && p.UF.Equals(pessoaAtualizada.UF, StringComparison.OrdinalIgnoreCase));

            if (pessoaDuplicada != null)
            {
                return BadRequest("Já existe outra pessoa com esses dados.");
            }

            // Simulando validação de informações recebidas
            if (string.IsNullOrEmpty(pessoaAtualizada.Nome) || string.IsNullOrEmpty(pessoaAtualizada.CPF) || string.IsNullOrEmpty(pessoaAtualizada.UF))
                return BadRequest("Nome, CPF e UF são campos obrigatórios.");

            pessoa.Nome = pessoaAtualizada.Nome;
            pessoa.CPF = pessoaAtualizada.CPF;
            pessoa.UF = pessoaAtualizada.UF;
            pessoa.DataNascimento = pessoaAtualizada.DataNascimento;

            return Ok(pessoa);
        }


        // Rota para excluir uma pessoa
        [HttpDelete("{codigo}")]
        
        public IActionResult ExcluirPessoa(long codigo)
        {
            var pessoa = pessoas.Find(p => p.Codigo == codigo);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            if (!pessoas.Contains(pessoa))
                return NotFound("Pessoa já foi excluída anteriormente.");

            pessoas.Remove(pessoa);
            return Ok(new { Message = "Pessoa excluída com sucesso." });
        }
    }
}