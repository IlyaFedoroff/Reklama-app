
public interface IReklamaService
{
    void LoadReklamasFromFile(string path);
    List<string> SearchByLocation(string location);
}
