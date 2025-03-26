
public interface IReklamaService
{
    Task LoadReklamasFromFileAsync(string path);
    List<string> SearchByLocation(string location);
}
