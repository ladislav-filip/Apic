namespace Apic.Common.Exceptions
{
    public interface IApiException
    {
        string Title { get; set; }
        int StatusCode { get; set; }
    }
}