namespace WorkerService.Configuration.Options;

public class RabbitMqOptions
{
	public string HostUrl { get; set; }

	public List<string> NodesAdresses { get; set; }

	public bool ConnectToNodes { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public string ApiClientGeneratorQueue { get; set; }

	public int ConcurrentMessageLimit { get; set; }

}

