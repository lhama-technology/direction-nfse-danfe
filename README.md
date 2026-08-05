# Direction.NFSe.Danfe

Biblioteca **.NET** para gerar **DANFSe (PDF)** a partir do XML da **NFSe Nacional (DPS/NFSe)**, **sem depender de endpoint externo**, permitindo **alto volume**, **baixa latência** e **customização completa de layout**.

> **Status:** operacional em produção nos cenários do autor.  
> A biblioteca é **open source** e contribuições são fortemente incentivadas para ampliar compatibilidade com variações reais de XML e regras municipais.

---

## Principais recursos

- ✅ Geração de **HTML e PDF** da DANFSe localmente  
- ✅ Sem chamadas externas (ideal para alto volume)  
- ✅ Layout HTML totalmente customizável  
- ✅ Observabilidade: **warnings padronizados** para campos ausentes/fallbacks  
- ✅ Testes de regressão de layout (**golden tests**)  
- ✅ Compatível com **.NET Standard / .NET Framework / .NET moderno**  

---

## Instalação

Via NuGet:

```bash
dotnet add package Direction.NFSe.Danfe
```

---

## Uso básico (API recomendada)

### Gerando a DANFSe a partir do XML

```csharp
using Direction.NFSe.Danfe;

var xml = File.ReadAllText("nfse.xml");

var danfe = new DanfeService(new DanfeOptions
{
    // Opcional: diretório base para templates e dados
    // BasePath = AppContext.BaseDirectory,

    // Opcional: template customizado
    // TemplatePath = @"C:\meu-layout\Danfe.html"
});

DanfeResult result = danfe.Generate(
    xml,
    DanfeEnvironment.Production
);

File.WriteAllBytes("danfse.pdf", result.PdfBytes);
```

### Gerando a DANFSe **CANCELADA** a partir do XML

- Como no **XML** não vem uma informação se a NFSe está cancelada, você é que tem que informar via parâmetro como no exemplo abaixo:


```csharp
using Direction.NFSe.Danfe;

var xml = File.ReadAllText("nfse.xml");

var danfe = new DanfeService();

DanfeResult result = danfe.Generate(
    xml,
    DanfeEnvironment.Production,
    true // Informa que é uma NFSe CANCELADA
);

File.WriteAllBytes("danfse.pdf", result.PdfBytes);
```

---

## Para inserir logos do seu município
- O repositório não tem a logo de todos os municípios. Você pode criar a do seu município obedecendo o padrão de **400x400** pixels e colocando na pasta **"Assets\Logos"**.
- Você deve ainda modificar o arquivo **municipios.csv**, localizando seu município e complementando as informações de logo_path e logo_name, como no exemplo abaixo para **Recife**:
```csv
2611606,Recife,-8.04666,-34.8771,1,26,2531,81,America/Sao_Paulo,Assets\Logos\logoRecife.png,Prefeitura do Recife<br>Secretaria de Finanças<br>faleconosco@recife.pe.gov.br
```
- Se você usar o código do munícipio, por exemplo: 2611606.png, nesse caso não precisa modificar o arquivo de municípios. Esse arquivo terá precedência a opção anterior.

---

## Acessando HTML e warnings

```csharp
Console.WriteLine(result.Html);

foreach (var warning in result.Warnings)
{
    Console.WriteLine($"{warning.Code}: {warning.Message} ({warning.Path})");
}
```

Exemplo de warning retornado:

```
NFSE_FIELD_MISSING: Campo vLiq ausente; usando '-' (infNFSe.valores.vLiq)
```

Esses warnings facilitam:
- diagnóstico de problemas no XML  
- abertura de issues  
- contribuição da comunidade  

---

## Overloads disponíveis

### A partir de `NFSeSchema`

```csharp
DanfeResult result = danfe.Generate(
    nfseSchema,
    DanfeEnvironment.Production
);
```

### A partir de `Stream`

```csharp
using var stream = File.OpenRead("nfse.xml");

DanfeResult result = danfe.Generate(
    stream,
    DanfeEnvironment.RestrictedProduction
);
```

---

## Ambientes

Em vez de `bool isProd`, a biblioteca utiliza:

```csharp
DanfeEnvironment.Production
DanfeEnvironment.Restricted
```

Isso melhora legibilidade, evita erros e facilita evolução futura.

---

## Customização de layout

O template HTML padrão está em:

```
src/Danfe/Assets/Templates/Danfe.html
```

Você pode:

- editar o template padrão (fork)  
- fornecer um template próprio via `DanfeOptions.TemplatePath`  
- propor melhorias via PR mantendo compatibilidade de placeholders  

---

## Observabilidade e warnings

A biblioteca gera warnings sempre que:

- um campo relevante do XML estiver ausente  
- ocorrer fallback (`"-"`, `0,00`, `string.Empty`)  
- um município não for encontrado  
- um placeholder não for resolvido no template  

Códigos de warning padronizados:

- `NFSE_FIELD_MISSING`  
- `MUNICIPIO_NOT_FOUND`  
- `TEMPLATE_PLACEHOLDER_EMPTY`  

---

## Testes e regressão de layout

O projeto utiliza **golden tests** para evitar regressões visuais:

- snapshot do HTML normalizado  
- validação básica do PDF (sanity check)  

Para rodar os testes:

```bash
dotnet test
```

---

## Desenvolvimento

### Build

```bash
dotnet restore
dotnet build -c Release
```

### Testes

```bash
dotnet test -c Release
```

### Formatação

```bash
dotnet format
npm run format
```

---

## Contribuindo

Contribuições são muito bem-vindas, especialmente para:

- novos layouts municipais  
- variações reais de XML  
- validações adicionais  
- melhorias de desempenho  
- testes adicionais  

Veja [CONTRIBUTING.md](CONTRIBUTING.md).

---

## Licença

MIT. Veja [LICENSE](LICENSE).
