using Models.Interfaces;

namespace Queue
{

    public class ManualQueue <T> : Queue.Queue<T> where T : IPendingOrder, new()
    {
    }
}
