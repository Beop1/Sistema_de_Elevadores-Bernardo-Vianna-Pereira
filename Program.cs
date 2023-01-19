// See https://aka.ms/new-console-template for more information

using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.Text;
using System.Reflection;

namespace ProvaAdmissionalCSharpApisul;

public interface IElevadorService
{
    /// <summary> Deve retornar uma List contendo o(s) andar(es) menos utilizado(s). </summary> 
    List<int> andarMenosUtilizado();

    /// <summary> Deve retornar uma List contendo o(s) elevador(es) mais frequentado(s). </summary> 
    List<char> elevadorMaisFrequentado();

    /// <summary> Deve retornar uma List contendo o período de maior fluxo de cada um dos elevadores mais frequentados (se houver mais de um). </summary> 
    List<char> periodoMaiorFluxoElevadorMaisFrequentado();

    /// <summary> Deve retornar uma List contendo o(s) elevador(es) menos frequentado(s). </summary> 
    List<char> elevadorMenosFrequentado();

    /// <summary> Deve retornar uma List contendo o período de menor fluxo de cada um dos elevadores menos frequentados (se houver mais de um). </summary> 
    List<char> periodoMenorFluxoElevadorMenosFrequentado();

    /// <summary> Deve retornar uma List contendo o(s) periodo(s) de maior utilização do conjunto de elevadores. </summary> 
    List<char> periodoMaiorUtilizacaoConjuntoElevadores();

    /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador A em relação a todos os serviços prestados. </summary> 
    float percentualDeUsoElevadorA();

    /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador B em relação a todos os serviços prestados. </summary> 
    float percentualDeUsoElevadorB();

    /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador C em relação a todos os serviços prestados. </summary> 
    float percentualDeUsoElevadorC();

    /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador D em relação a todos os serviços prestados. </summary> 
    float percentualDeUsoElevadorD();

    /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador E em relação a todos os serviços prestados. </summary> 
    float percentualDeUsoElevadorE();

}
public struct Struct_Input
{
    public int andar { get; set; }
    public string elevador { get; set; }
    public string turno { get; set; }

    public Struct_Input()
    {
        andar = 0;
        elevador = "";
        turno = "";
    }
}

public class Pesquisa : IElevadorService
{
    const int NUMERO_DE_ANDARES = 16;
    
    public List<Struct_Input> lista_Inputs;

    List<string> lista_Inputs_Elevadores = new();
    Dictionary<char, int> elevadores_Uso = new Dictionary<char, int>()
        {
            {'A', 0 },
            {'B', 0 },
            {'C', 0 },
            {'D', 0 },
            {'E', 0 }
        };

    List<string> periodos_De_Uso = new();

    Dictionary<char, int> periodos = new() { { 'M', 0 }, { 'V', 0 }, { 'N', 0 } };

    public List<char> elevadores_Mais_Frequentados = new();
    public List<char> periodos_Maior_Fluxo_Elevador_Mais_Frequentado = new();
    public List<char> elevadores_Menos_Frequentados = new();
    public List<char> periodos_Menor_Fluxo_Elevador_Menos_Frequentado = new();
    public List<char> períodos_De_Maior_Uso_Conjunto = new();


    public List<int> andarMenosUtilizado()
    {
        List<int> andares_Menos_Utilizados = new();
        List<int> lista_Inputs_Andares = new();
        int[] usos_Por_Andar = new int[NUMERO_DE_ANDARES];

        foreach (Struct_Input input in lista_Inputs)
            lista_Inputs_Andares.Add(input.andar);

        for (int i = 0; i < NUMERO_DE_ANDARES; i++)
        {
            foreach (int andar in lista_Inputs_Andares)
                if (andar.Equals(i)) usos_Por_Andar[i]++;
        }
        int menor_Uso = usos_Por_Andar.Min<int>();

        foreach (int uso in usos_Por_Andar)
            if (uso.Equals(menor_Uso))
                andares_Menos_Utilizados.Add(Array.IndexOf(usos_Por_Andar, uso));

        return andares_Menos_Utilizados;
    }

    public List<char> elevadorMaisFrequentado()
    {
        foreach (Struct_Input input in lista_Inputs)
            lista_Inputs_Elevadores.Add(input.elevador);

        foreach (string input_Elevador in lista_Inputs_Elevadores)
        {
            switch (input_Elevador)
            {
                case "A":
                    elevadores_Uso['A']++;
                    break;
                case "B":
                    elevadores_Uso['B']++;
                    break;
                case "C":
                    elevadores_Uso['C']++;
                    break;
                case "D":
                    elevadores_Uso['D']++;
                    break;
                case "E":
                    elevadores_Uso['E']++;
                    break;
            }
        }
        int maior_Uso = elevadores_Uso.Values.Max<int>();

        foreach (var uso in elevadores_Uso)
            if (uso.Value.Equals(maior_Uso))
            {
                elevadores_Mais_Frequentados.Add(uso.Key);
            }
        return elevadores_Mais_Frequentados;
    }

    public List<char> periodoMaiorFluxoElevadorMaisFrequentado()
    {
        periodos_De_Uso.Clear();

        foreach (Struct_Input input in lista_Inputs)
            foreach (char elevador in elevadores_Mais_Frequentados)
                if (Char.Parse(input.elevador).Equals(elevador))
                    periodos_De_Uso.Add(input.turno);

        foreach (string periodo in periodos_De_Uso)
        {
            switch (periodo)
            {
                case "M":
                    periodos['M'] += 1;
                    break;
                case "V":
                    periodos['V'] += 1;
                    break;
                case "N":
                    periodos['N'] += 1;
                    break;
            }
        }

        foreach (KeyValuePair<char, int> periodo in periodos)
            if (periodo.Value.Equals(periodos.Values.Max()))
                periodos_Maior_Fluxo_Elevador_Mais_Frequentado.Add(periodo.Key);

        return periodos_Maior_Fluxo_Elevador_Mais_Frequentado;
    }

    public List<char> elevadorMenosFrequentado()
    {
        int menor_Uso = elevadores_Uso.Values.Min<int>();

        foreach (var uso in elevadores_Uso)
            if (uso.Value.Equals(menor_Uso))
            {
                elevadores_Menos_Frequentados.Add(uso.Key);
            }
        return elevadores_Menos_Frequentados;
    }

    public List<char> periodoMenorFluxoElevadorMenosFrequentado()
    {
        periodos_De_Uso.Clear();

        foreach (Struct_Input input in lista_Inputs)
            foreach (char elevador in elevadores_Menos_Frequentados)
                if (Char.Parse(input.elevador).Equals(elevador))
                    periodos_De_Uso.Add(input.turno);

        foreach (string periodo in periodos_De_Uso)
        {
            switch (periodo)
            {
                case "M":
                    periodos['M'] += 1;
                    break;
                case "V":
                    periodos['V'] += 1;
                    break;
                case "N":
                    periodos['N'] += 1;
                    break;
            }
        }

        foreach (KeyValuePair<char, int> periodo in periodos)
            if (periodo.Value.Equals(periodos.Values.Min()))
                periodos_Menor_Fluxo_Elevador_Menos_Frequentado.Add(periodo.Key);

        return periodos_Menor_Fluxo_Elevador_Menos_Frequentado;
    }

    public List<char> periodoMaiorUtilizacaoConjuntoElevadores()
    {
        periodos_De_Uso.Clear();
        Dictionary<char, int> usos_Períodos = new Dictionary<char, int>()
        {
            {'M', 0 },
            {'V', 0 },
            {'N', 0 },
        };

        foreach (Struct_Input input in lista_Inputs)
            periodos_De_Uso.Add(input.turno);


        foreach (string input_Período in periodos_De_Uso)
        {
            switch (input_Período)
            {
                case "M":
                    usos_Períodos['M']++;
                    break;
                case "V":
                    usos_Períodos['V']++;
                    break;
                case "N":
                    usos_Períodos['N']++;
                    break;
            }
        }
        int maior_Uso = usos_Períodos.Values.Max<int>();

        foreach (var uso in usos_Períodos)
            if (uso.Value.Equals(maior_Uso))
            {
                períodos_De_Maior_Uso_Conjunto.Add(uso.Key);
            }
        return períodos_De_Maior_Uso_Conjunto;
    }

    public float percentualDeUsoElevadorA()
    {
        decimal uso_Elevador;
        int usos = 0;
        foreach (string elevador in lista_Inputs_Elevadores)
            if (elevador.Equals("A"))
            {
                usos++;
            }
        uso_Elevador = ((decimal)usos /  (decimal)lista_Inputs_Elevadores.Count) * 100.00m;
        
        return (float)Decimal.Round(uso_Elevador, 2);
    }

    public float percentualDeUsoElevadorB()
    {
        decimal uso_Elevador;
        int usos = 0;
        foreach (string elevador in lista_Inputs_Elevadores)
            if (elevador.Equals("B"))
            {
                usos++;
            }
        uso_Elevador = ((decimal)usos / (decimal)lista_Inputs_Elevadores.Count) * 100.00m;

        return (float)Decimal.Round(uso_Elevador, 2);

    }

    public float percentualDeUsoElevadorC()
    {
        decimal uso_Elevador;
        int usos = 0;
        foreach (string elevador in lista_Inputs_Elevadores)
            if (elevador.Equals("C"))
            {
                usos++;
            }
        uso_Elevador = ((decimal)usos / (decimal)lista_Inputs_Elevadores.Count) * 100.00m;

        return (float)Decimal.Round(uso_Elevador, 2);
    }

    public float percentualDeUsoElevadorD()
    {
        decimal uso_Elevador;
        int usos = 0;
        foreach (string elevador in lista_Inputs_Elevadores)
            if (elevador.Equals("D"))
            {
                usos++;
            }
        uso_Elevador = ((decimal)usos / (decimal)lista_Inputs_Elevadores.Count) * 100.00m;

        return (float)Decimal.Round(uso_Elevador, 2);
    }

    public float percentualDeUsoElevadorE()
    {
        decimal uso_Elevador;
        int usos = 0;
        foreach (string elevador in lista_Inputs_Elevadores)
            if (elevador.Equals("E"))
            {
                usos++;
            }
        uso_Elevador = ((decimal)usos / (decimal)lista_Inputs_Elevadores.Count) * 100.00m;

        return (float)Decimal.Round(uso_Elevador, 2);
    }
}

public class Program
{
    static void Main()
    {
        Console.WriteLine("            BERNARDO VIANNA PEREIRA - DESENVOLVEDOR SOFTWARE C#\n" +
                          "            LinkedIn: https://www.linkedin.com/in/bernardovpereira/"+"\n\n");

        Pesquisa pesquisa = new();
        Console.WriteLine("Insira o local do Input.json no seu computador");

        string inputjson = File.ReadAllText(Console.ReadLine().Replace("\"", ""));

        pesquisa.lista_Inputs = JsonSerializer.Deserialize<List<Struct_Input>>(inputjson);

        //Exibição dos Resultados

        Console.WriteLine("\n\nExibição dos Resultados:\n\n");

        //Andar(es) menos utilizado(s)
        Console.WriteLine("## Andar(es) menos utilizado(s) ##:\n" +
                          String.Join(" - ", pesquisa.andarMenosUtilizado().ToArray()) + "\n");


        //Elevador(es) mais frequentado(s) e Período de maior fluxo
        List<string> periodo = new();

        Console.WriteLine("## Elevador(es) mais utilizado(s) e período(s) de maior fluxo##:\n" +
                          String.Join(" / ", pesquisa.elevadorMaisFrequentado().ToArray()));

        if (pesquisa.periodoMaiorFluxoElevadorMaisFrequentado().Contains('M')) periodo.Add("Matutino");
        if (pesquisa.periodoMaiorFluxoElevadorMaisFrequentado().Contains('V')) periodo.Add("Vespertino");
        if (pesquisa.periodoMaiorFluxoElevadorMaisFrequentado().Contains('N')) periodo.Add("Noturno");

        Console.WriteLine("No(s) período(s) " + String.Join(" / ", periodo.ToArray()) + "\n");


        //Elevador(es) menos frequentado(s) e Período de menor fluxo
        periodo = new();

        Console.WriteLine("## Elevador(es) menos utilizadoa(s) e período(s) de menor fluxo##:\n" +
                          String.Join(" - ", pesquisa.elevadorMenosFrequentado().ToArray()));

        if (pesquisa.periodoMenorFluxoElevadorMenosFrequentado().Contains('M')) periodo.Add("Matutino");
        if (pesquisa.periodoMenorFluxoElevadorMenosFrequentado().Contains('V')) periodo.Add("Vespertino");
        if (pesquisa.periodoMenorFluxoElevadorMenosFrequentado().Contains('N')) periodo.Add("Noturno");

        Console.WriteLine("No(s) período(s) " + String.Join(" / ", periodo.ToArray()) + "\n");


        //Periodo(s) de maior utilização do conjunto de elevadores
        Console.WriteLine("## Periodo(s) de maior utilização do conjunto de elevadores ##:\n" +
                          String.Join(" - ", pesquisa.periodoMaiorUtilizacaoConjuntoElevadores().ToArray()) + "\n");


        //Percentual de uso do elevador A em relação a todos os serviços prestados
        Console.WriteLine("## Percentual de uso do elevador A em relação a todos os serviços prestadoss ##:\n" + 
            pesquisa.percentualDeUsoElevadorA().ToString() + " %\n");


        //Percentual de uso do elevador B em relação a todos os serviços prestados
        Console.WriteLine("## Percentual de uso do elevador B em relação a todos os serviços prestadoss ##:\n" +
            pesquisa.percentualDeUsoElevadorB().ToString() + " %\n");


        //Percentual de uso do elevador C em relação a todos os serviços prestados
        Console.WriteLine("## Percentual de uso do elevador C em relação a todos os serviços prestadoss ##:\n" +
            pesquisa.percentualDeUsoElevadorC().ToString() + " %\n");


        //Percentual de uso do elevador D em relação a todos os serviços prestados
        Console.WriteLine("## Percentual de uso do elevador A em relação a todos os serviços prestadoss ##:\n" +
            pesquisa.percentualDeUsoElevadorD().ToString() + " %\n");


        //Percentual de uso do elevador E em relação a todos os serviços prestados
        Console.WriteLine("## Percentual de uso do elevador E em relação a todos os serviços prestadoss ##:\n" +
            pesquisa.percentualDeUsoElevadorE().ToString() + " %\n");

        Console.WriteLine("Espero que gostem =) !!!\n\n Atenciosamente\nBernardo Vianna Pereira");
        Console.ReadKey();
    }
}


