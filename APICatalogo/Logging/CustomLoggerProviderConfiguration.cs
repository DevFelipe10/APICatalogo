namespace APICatalogo.Logging;

//Classe de configuração do provedor de log customizado
public class CustomLoggerProviderConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
    public int EventId { get; set; } = 0;
}
