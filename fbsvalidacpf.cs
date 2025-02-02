using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

public static class fbsvalidacpf
{
    [FunctionName("fbsvalidacpf")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Processando uma requisição para validação de CPF.");

        // Obtém o CPF da query string ou do corpo da requisição
        string cpf = req.Query["cpf"];

        if (string.IsNullOrEmpty(cpf))
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            cpf = data?.cpf;
        }

        if (string.IsNullOrEmpty(cpf))
        {
            return new BadRequestObjectResult("Por favor, forneça um CPF para validação.");
        }

        // Chama o ValidadorCPF para validar o CPF
        bool isValid = ValidadorCPF.Validar(cpf);

        // Retorna a resposta com o resultado da validação
        return new OkObjectResult(new
        {
            cpf = cpf,
            valid = isValid
        });
    }
}
