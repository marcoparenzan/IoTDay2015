using DocumentDb.Demos;

namespace DocumentDb
{
    static class Program
    {
        static void Main(string[] args)
        {
            InsertDemo.RunAsync(args).Wait();
            //AttachmentsDemo.RunAsync(args).Wait();
            //DaemonDemo.RunAsync(args).Wait();
            //CollectionsDemo.RunAsync(args).Wait();
            //QueriesDemo.RunAsync(args).Wait();
            //JoinDemo.RunAsync(args).Wait();
            //RegionUserDefinedFunctionDemo.RunAsync(args).Wait();
            //CountStoredProcedureDemo.RunAsync(args).Wait();
            //NormalizeDocumentTriggerDemo.RunAsync(args).Wait();
            //PartitionDemo.RunAsync(args).Wait();
        }
    }
}
