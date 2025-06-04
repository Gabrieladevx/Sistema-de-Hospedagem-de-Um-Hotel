using System;
using System.Collections.Generic;

class Hospede
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Documento { get; set; }
}

class Quarto
{
    public int Numero { get; set; }
    public string? Tipo { get; set; }
    public bool Ocupado { get; set; }
}

class Reserva
{
    public int Id { get; set; }
    public Hospede? Hospede { get; set; }
    public Quarto? Quarto { get; set; }
    public DateTime DataEntrada { get; set; }
    public DateTime? DataSaida { get; set; }
}

class Pagamento
{
    public int Id { get; set; }
    public Reserva? Reserva { get; set; }
    public decimal Valor { get; set; }
    public string? FormaPagamento { get; set; }
    public DateTime DataPagamento { get; set; }
}

class SistemaHotel
{
    private List<Hospede> hospedes = new();
    private List<Quarto> quartos = new();
    private List<Reserva> reservas = new();
    private List<Pagamento> pagamentos = new();
    private int proximoHospedeId = 1;
    private int proximaReservaId = 1;
    private int proximoPagamentoId = 1;

    public void Menu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Sistema de Hospedagem de Hotel ---");
            Console.WriteLine("1. Cadastrar hóspede");
            Console.WriteLine("2. Cadastrar quarto");
            Console.WriteLine("3. Realizar reserva");
            Console.WriteLine("4. Listar reservas");
            Console.WriteLine("5. Registrar pagamento");
            Console.WriteLine("6. Listar pagamentos");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");
            var opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1": CadastrarHospede(); break;
                case "2": CadastrarQuarto(); break;
                case "3": RealizarReserva(); break;
                case "4": ListarReservas(); break;
                case "5": RegistrarPagamento(); break;
                case "6": ListarPagamentos(); break;
                case "0": return;
                default: Console.WriteLine("Opção inválida!"); break;
            }
        }
    }

    private void CadastrarHospede()
    {
        Console.Write("Nome do hóspede: ");
        var nome = Console.ReadLine();
        Console.Write("Documento: ");
        var doc = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(doc))
        {
            Console.WriteLine("Dados inválidos!");
            return;
        }
        hospedes.Add(new Hospede { Id = proximoHospedeId++, Nome = nome, Documento = doc });
        Console.WriteLine("Hóspede cadastrado!");
    }

    private void CadastrarQuarto()
    {
        Console.Write("Número do quarto: ");
        var inputNumero = Console.ReadLine();
        if (!int.TryParse(inputNumero, out int numero))
        {
            Console.WriteLine("Número inválido!");
            return;
        }
        Console.Write("Tipo do quarto: ");
        var tipo = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(tipo))
        {
            Console.WriteLine("Tipo inválido!");
            return;
        }
        quartos.Add(new Quarto { Numero = numero, Tipo = tipo, Ocupado = false });
        Console.WriteLine("Quarto cadastrado!");
    }

    private void RealizarReserva()
    {
        Console.Write("ID do hóspede: ");
        var inputHospede = Console.ReadLine();
        if (!int.TryParse(inputHospede, out int idHospede))
        {
            Console.WriteLine("ID inválido!");
            return;
        }
        var hospede = hospedes.Find(h => h.Id == idHospede);
        if (hospede == null)
        {
            Console.WriteLine("Hóspede não encontrado!");
            return;
        }
        Console.Write("Número do quarto: ");
        var inputQuarto = Console.ReadLine();
        if (!int.TryParse(inputQuarto, out int numQuarto))
        {
            Console.WriteLine("Número inválido!");
            return;
        }
        var quarto = quartos.Find(q => q.Numero == numQuarto && !q.Ocupado);
        if (quarto == null)
        {
            Console.WriteLine("Quarto não disponível!");
            return;
        }
        quarto.Ocupado = true;
        reservas.Add(new Reserva
        {
            Id = proximaReservaId++,
            Hospede = hospede,
            Quarto = quarto,
            DataEntrada = DateTime.Now
        });
        Console.WriteLine("Reserva realizada!");
    }

    private void RegistrarPagamento()
    {
        Console.Write("ID da reserva: ");
        var inputReserva = Console.ReadLine();
        if (!int.TryParse(inputReserva, out int idReserva))
        {
            Console.WriteLine("ID inválido!");
            return;
        }
        var reserva = reservas.Find(r => r.Id == idReserva);
        if (reserva == null)
        {
            Console.WriteLine("Reserva não encontrada!");
            return;
        }
        Console.Write("Valor do pagamento: ");
        var inputValor = Console.ReadLine();
        if (!decimal.TryParse(inputValor, out decimal valor) || valor <= 0)
        {
            Console.WriteLine("Valor inválido!");
            return;
        }
        Console.Write("Forma de pagamento (dinheiro/cartão/pix): ");
        var forma = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(forma))
        {
            Console.WriteLine("Forma de pagamento inválida!");
            return;
        }
        pagamentos.Add(new Pagamento
        {
            Id = proximoPagamentoId++,
            Reserva = reserva,
            Valor = valor,
            FormaPagamento = forma,
            DataPagamento = DateTime.Now
        });
        Console.WriteLine("Pagamento registrado!");
    }

    private void ListarReservas()
    {
        Console.WriteLine("\n--- Lista de Reservas ---");
        if (reservas.Count == 0)
        {
            Console.WriteLine("Nenhuma reserva encontrada.");
        }
        else
        {
            foreach (var r in reservas)
            {
                string dataSaida = r.DataSaida.HasValue ? r.DataSaida.Value.ToString() : "-";
                Console.WriteLine($"Reserva {r.Id}: Hóspede {r.Hospede?.Nome}, Quarto {r.Quarto?.Numero}, Entrada: {r.DataEntrada}, Saída: {dataSaida}");
            }
        }
        Console.WriteLine("Pressione ENTER para voltar ao menu...");
        Console.ReadLine();
    }

    private void ListarPagamentos()
    {
        Console.WriteLine("\n--- Lista de Pagamentos ---");
        if (pagamentos.Count == 0)
        {
            Console.WriteLine("Nenhum pagamento registrado.");
        }
        else
        {
            foreach (var p in pagamentos)
            {
                Console.WriteLine($"Pagamento {p.Id}: Reserva {p.Reserva?.Id}, Valor: R${p.Valor:F2}, Forma: {p.FormaPagamento}, Data: {p.DataPagamento}");
            }
        }
        Console.WriteLine("Pressione ENTER para voltar ao menu...");
        Console.ReadLine();
    }
}

// Ponto de entrada principal
class Program
{
    static void Main()
    {
        var sistema = new SistemaHotel();
        sistema.Menu();
    }
}
